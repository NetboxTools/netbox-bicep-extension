namespace Bicep.Extension.Netbox.Handlers;

public class AggregateHandler : NetboxResourceHandlerBase<Aggregate, AggregateIdentifiers>
{
    protected override string ApiPath => "/api/ipam/aggregates/";

    protected override string GetLookupQuery(Aggregate properties) => $"prefix={properties.Prefix}";

    protected override AggregateIdentifiers GetIdentifiers(Aggregate properties) => new()
    {
        Prefix = properties.Prefix
    };
}
