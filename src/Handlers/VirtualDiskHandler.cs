namespace Bicep.Extension.Netbox.Handlers;

public class VirtualDiskHandler : NetboxResourceHandlerBase<VirtualDisk, NameIdentifiers>
{
    protected override string ApiPath => "/api/virtualization/virtual-disks/";

    protected override string GetLookupQuery(VirtualDisk properties) => $"name={properties.Name}&virtual_machine_id={properties.VirtualMachine}";

    protected override NameIdentifiers GetIdentifiers(VirtualDisk properties) => new()
    {
        Name = properties.Name
    };
}
