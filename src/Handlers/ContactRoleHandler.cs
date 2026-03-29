namespace Bicep.Extension.Netbox.Handlers;

public class ContactRoleHandler : SlugResourceHandler<ContactRole>
{
    protected override string ApiPath => "/api/tenancy/contact-roles/";
}
