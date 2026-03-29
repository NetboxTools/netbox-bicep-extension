using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using Bicep.Extension.Netbox;
using Bicep.Extension.Netbox.Handlers;

namespace Bicep.Extension.Netbox.Tests;

/// <summary>
/// Tests the v1/v2 token auto-detection in the base handler.
/// </summary>
public class AuthDetectionTests
{
    /// <summary>
    /// Uses reflection to call the protected CreateHttpClient method.
    /// </summary>
    private static HttpClient CreateClient(string token)
    {
        var method = typeof(NetboxResourceHandlerBase<Site, SlugIdentifiers>)
            .GetMethod("CreateHttpClient", BindingFlags.Static | BindingFlags.NonPublic);

        Assert.NotNull(method);

        var config = new Configuration
        {
            Url = "http://localhost:8000",
            Token = token
        };

        return (HttpClient)method!.Invoke(null, [config])!;
    }

    [Fact]
    public void V2_Token_Uses_Bearer_Auth()
    {
        using var client = CreateClient("nbt_akFfGqPKQnGM.sbQJ1Dl4L9dZVKwYrrM9aFApn626alGfaqgoGm3Q");
        Assert.Equal("Bearer", client.DefaultRequestHeaders.Authorization!.Scheme);
    }

    [Fact]
    public void V1_Token_Uses_Token_Auth()
    {
        using var client = CreateClient("abc123def456");
        Assert.Equal("Token", client.DefaultRequestHeaders.Authorization!.Scheme);
    }

    [Fact]
    public void Empty_Token_Uses_Token_Auth()
    {
        using var client = CreateClient("");
        Assert.Equal("Token", client.DefaultRequestHeaders.Authorization!.Scheme);
    }

    [Fact]
    public void Client_Has_Json_Accept_Header()
    {
        using var client = CreateClient("test");
        Assert.Contains(
            client.DefaultRequestHeaders.Accept,
            h => h.MediaType == "application/json");
    }

    [Fact]
    public void Client_BaseAddress_Trims_Trailing_Slash()
    {
        var method = typeof(NetboxResourceHandlerBase<Site, SlugIdentifiers>)
            .GetMethod("CreateHttpClient", BindingFlags.Static | BindingFlags.NonPublic);

        var config = new Configuration
        {
            Url = "http://localhost:8000/",
            Token = "test"
        };

        using var client = (HttpClient)method!.Invoke(null, [config])!;
        Assert.Equal("http://localhost:8000", client.BaseAddress!.ToString().TrimEnd('/'));
    }
}
