namespace Bicep.Extension.Netbox.Handlers;

public class VirtualMachineHandler : NetboxResourceHandlerBase<VirtualMachine, NameIdentifiers>
{
    protected override string ApiPath => "/api/virtualization/virtual-machines/";

    protected override string GetLookupQuery(VirtualMachine properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(VirtualMachine properties) => new()
    {
        Name = properties.Name
    };
}
