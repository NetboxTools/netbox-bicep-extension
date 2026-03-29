namespace Bicep.Extension.Netbox.Handlers;

public class VMInterfaceHandler : NetboxResourceHandlerBase<VMInterface, NameIdentifiers>
{
    protected override string ApiPath => "/api/virtualization/interfaces/";

    protected override string GetLookupQuery(VMInterface properties) => $"name={properties.Name}&virtual_machine_id={properties.VirtualMachine}";

    protected override NameIdentifiers GetIdentifiers(VMInterface properties) => new()
    {
        Name = properties.Name
    };
}
