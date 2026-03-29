namespace Bicep.Extension.Netbox.Handlers;

public class UserHandler : NetboxResourceHandlerBase<User, UsernameIdentifiers>
{
    protected override string ApiPath => "/api/users/users/";

    protected override string GetLookupQuery(User properties) => $"username={properties.Username}";

    protected override UsernameIdentifiers GetIdentifiers(User properties) => new()
    {
        Username = properties.Username
    };
}
