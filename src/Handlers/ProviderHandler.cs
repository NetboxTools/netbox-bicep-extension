namespace Bicep.Extension.Netbox.Handlers;

public class ProviderHandler : SlugResourceHandler<Provider>
{
    protected override string ApiPath => "/api/circuits/providers/";
}
