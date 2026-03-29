namespace Bicep.Extension.Netbox.Handlers;

public class UserGroupHandler : NetboxResourceHandlerBase<UserGroup, NameIdentifiers>
{
    protected override string ApiPath => "/api/users/groups/";

    protected override string GetLookupQuery(UserGroup properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(UserGroup properties) => new()
    {
        Name = properties.Name
    };
}
