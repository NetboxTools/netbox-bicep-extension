using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Bicep.Local.Extension.Host.Handlers;

namespace Bicep.Extension.Netbox.Handlers;

/// <summary>
/// Base handler for all NetBox resource types.
/// Provides shared HttpClient setup, authentication, and error handling.
/// New resource handlers only need to implement their specific API paths and lookup logic.
/// </summary>
public abstract class NetboxResourceHandlerBase<TProperties, TIdentifiers>
    : TypedResourceHandler<TProperties, TIdentifiers, Configuration>
    where TProperties : class, TIdentifiers
    where TIdentifiers : class
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        // Convert C# PascalCase property names to snake_case for the NetBox API.
        // Do NOT use [JsonPropertyName] on model properties — the Bicep SDK uses
        // C# property names for gRPC deserialization, and they must match.
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Returns the NetBox API path for this resource type (e.g. "/api/dcim/sites/").
    /// Must include leading and trailing slashes.
    /// </summary>
    protected abstract string ApiPath { get; }

    /// <summary>
    /// Returns the query string used to look up this resource (e.g. "slug=my-site" or "name=switch01").
    /// Used by FindExisting to determine create vs. update.
    /// </summary>
    protected abstract string GetLookupQuery(TProperties properties);

    protected override Task<ResourceResponse> Preview(ResourceRequest request, CancellationToken cancellationToken)
    {
        // What-if / dry-run: returns the desired state without making API calls.
        // The sample extensions (bicep-ext-github, bicep-ext-http) all do this.
        return Task.FromResult(GetResponse(request));
    }

    protected override async Task<ResourceResponse> CreateOrUpdate(ResourceRequest request, CancellationToken cancellationToken)
    {
        var config = request.Config!;
        using var client = CreateHttpClient(config);

        // Try to find existing resource by its unique lookup
        var existing = await FindExisting(client, request.Properties, cancellationToken);

        var json = System.Text.Json.JsonSerializer.Serialize(request.Properties, JsonOptions);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        JsonElement? apiResult;

        if (existing != null)
        {
            // Update existing resource via PATCH
            var id = existing.Value.GetProperty("id").GetInt32();
            var patchRequest = new HttpRequestMessage(HttpMethod.Patch, $"{ApiPath}{id}/") { Content = content };
            var patchResponse = await client.SendAsync(patchRequest, cancellationToken);
            await EnsureSuccess(patchResponse, cancellationToken);
            apiResult = await patchResponse.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);
        }
        else
        {
            // Create new resource via POST
            var postResponse = await client.PostAsync(ApiPath, content, cancellationToken);
            await EnsureSuccess(postResponse, cancellationToken);
            apiResult = await postResponse.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);
        }

        // Populate output properties from the API response
        if (apiResult.HasValue)
            PopulateOutputProperties(request.Properties, apiResult.Value);

        return GetResponse(request);
    }

    /// <summary>
    /// Sets output properties (like Id, Url) on the resource from the NetBox API response.
    /// Override in derived handlers for resource-specific output mapping.
    /// </summary>
    protected virtual void PopulateOutputProperties(TProperties properties, JsonElement apiResponse)
    {
        // Set Id if the model has the property (via INetboxResource)
        if (properties is INetboxResource netboxResource)
        {
            if (apiResponse.TryGetProperty("id", out var idProp))
                netboxResource.Id = idProp.GetRawText();
            if (apiResponse.TryGetProperty("url", out var urlProp))
                netboxResource.Url = urlProp.GetString();
            if (apiResponse.TryGetProperty("display", out var displayProp))
                netboxResource.Display = displayProp.GetString();
        }
    }

    /// <summary>
    /// Creates an HttpClient configured with the NetBox base URL and auth token.
    /// </summary>
    protected static HttpClient CreateHttpClient(Configuration config)
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri(config.Url.TrimEnd('/'))
        };
        // NetBox v4+ uses Bearer auth with nbt_ prefixed tokens; older versions use Token auth
        var scheme = config.Token.StartsWith("nbt_") ? "Bearer" : "Token";
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, config.Token);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    /// <summary>
    /// Looks up an existing resource using the handler's lookup query. Returns the JSON element if found.
    /// </summary>
    private async Task<JsonElement?> FindExisting(HttpClient client, TProperties properties, CancellationToken cancellationToken)
    {
        var query = GetLookupQuery(properties);
        var response = await client.GetAsync($"{ApiPath}?{query}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);

        if (json.TryGetProperty("results", out var results) && results.GetArrayLength() > 0)
            return results[0];

        return null;
    }

    /// <summary>
    /// Throws a structured error if the HTTP response indicates failure.
    /// </summary>
    protected static async Task EnsureSuccess(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new ResourceErrorException(new Error
        {
            Code = ((int)response.StatusCode).ToString(),
            Message = $"NetBox API returned {(int)response.StatusCode} {response.StatusCode}: {body}"
        });
    }
}
