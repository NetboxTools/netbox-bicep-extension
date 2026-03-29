namespace Bicep.Extension.Netbox.Handlers;

public class RIRHandler : SlugResourceHandler<RIR>
{
    protected override string ApiPath => "/api/ipam/rirs/";
}
