namespace Bicep.Extension.Netbox.Handlers;

public class ContactGroupHandler : SlugResourceHandler<ContactGroup>
{
    protected override string ApiPath => "/api/tenancy/contact-groups/";
}
