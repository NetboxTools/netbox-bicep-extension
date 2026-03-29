namespace Bicep.Extension.Netbox.Handlers;

public class ContactHandler : NetboxResourceHandlerBase<Contact, NameIdentifiers>
{
    protected override string ApiPath => "/api/tenancy/contacts/";

    protected override string GetLookupQuery(Contact properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(Contact properties) => new()
    {
        Name = properties.Name
    };
}
