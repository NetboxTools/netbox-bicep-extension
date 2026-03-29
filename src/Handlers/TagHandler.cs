namespace Bicep.Extension.Netbox.Handlers;

public class TagHandler : SlugResourceHandler<Tag>
{
    protected override string ApiPath => "/api/extras/tags/";
}
