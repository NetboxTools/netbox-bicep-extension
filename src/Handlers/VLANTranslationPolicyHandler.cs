namespace Bicep.Extension.Netbox.Handlers;

public class VLANTranslationPolicyHandler : NetboxResourceHandlerBase<VLANTranslationPolicy, NameIdentifiers>
{
    protected override string ApiPath => "/api/ipam/vlan-translation-policies/";

    protected override string GetLookupQuery(VLANTranslationPolicy properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(VLANTranslationPolicy properties) => new()
    {
        Name = properties.Name
    };
}
