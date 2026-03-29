namespace Bicep.Extension.Netbox.Handlers;

public class RouteTargetHandler : NetboxResourceHandlerBase<RouteTarget, NameIdentifiers>
{
    protected override string ApiPath => "/api/ipam/route-targets/";

    protected override string GetLookupQuery(RouteTarget properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(RouteTarget properties) => new()
    {
        Name = properties.Name
    };
}
