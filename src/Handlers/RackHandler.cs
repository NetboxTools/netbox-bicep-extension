namespace Bicep.Extension.Netbox.Handlers;

public class RackHandler : NetboxResourceHandlerBase<Rack, RackIdentifiers>
{
    protected override string ApiPath => "/api/dcim/racks/";

    // Name is unique per site, so include site_id in the lookup
    protected override string GetLookupQuery(Rack properties) => $"name={properties.Name}&site_id={properties.Site}";

    protected override RackIdentifiers GetIdentifiers(Rack properties) => new()
    {
        Name = properties.Name
    };
}
