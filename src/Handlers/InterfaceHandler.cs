namespace Bicep.Extension.Netbox.Handlers;

public class InterfaceHandler : NetboxResourceHandlerBase<Interface, InterfaceIdentifiers>
{
    protected override string ApiPath => "/api/dcim/interfaces/";

    protected override string GetLookupQuery(Interface properties) => $"name={properties.Name}&device_id={properties.Device}";

    protected override InterfaceIdentifiers GetIdentifiers(Interface properties) => new()
    {
        Name = properties.Name
    };
}
