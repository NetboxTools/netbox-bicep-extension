using System.Text.RegularExpressions;

namespace Bicep.Extension.Netbox.Handlers;

/// <summary>
/// Base handler for resources that use a name + auto-generated slug.
/// Inheritors only need to provide ApiPath — lookup, identifiers, and slug
/// generation are all handled here.
/// </summary>
public abstract partial class SlugResourceHandler<TProperties>
    : NetboxResourceHandlerBase<TProperties, NameSlugIdentifiers>
    where TProperties : NameSlugIdentifiers
{
    protected override string GetLookupQuery(TProperties properties)
    {
        var slug = properties.Slug ?? GenerateSlug(properties.Name);
        return $"slug={slug}";
    }

    protected override NameSlugIdentifiers GetIdentifiers(TProperties properties) => new()
    {
        Name = properties.Name
    };

    protected override Task<ResourceResponse> Preview(ResourceRequest request, CancellationToken cancellationToken)
    {
        EnsureSlug(request.Properties);
        return base.Preview(request, cancellationToken);
    }

    protected override async Task<ResourceResponse> CreateOrUpdate(ResourceRequest request, CancellationToken cancellationToken)
    {
        EnsureSlug(request.Properties);
        return await base.CreateOrUpdate(request, cancellationToken);
    }

    private static void EnsureSlug(TProperties properties)
    {
        if (string.IsNullOrEmpty(properties.Slug))
            properties.Slug = GenerateSlug(properties.Name);
    }

    /// <summary>
    /// Generates a URL-friendly slug from a name.
    /// "Stockholm DC1" → "stockholm-dc1", "Cisco Systems, Inc." → "cisco-systems-inc"
    /// </summary>
    protected static string GenerateSlug(string value) =>
        SlugRegex().Replace(value.ToLowerInvariant(), "-").Trim('-');

    [GeneratedRegex(@"[^a-z0-9]+")]
    private static partial Regex SlugRegex();
}
