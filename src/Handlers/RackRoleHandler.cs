namespace Bicep.Extension.Netbox.Handlers;

public class RackRoleHandler : SlugResourceHandler<RackRole>
{
    protected override string ApiPath => "/api/dcim/rack-roles/";
}
