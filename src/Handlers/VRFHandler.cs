namespace Bicep.Extension.Netbox.Handlers;

public class VRFHandler : NetboxResourceHandlerBase<VRF, NameIdentifiers>
{
    protected override string ApiPath => "/api/ipam/vrfs/";

    protected override string GetLookupQuery(VRF properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(VRF properties) => new()
    {
        Name = properties.Name
    };
}
