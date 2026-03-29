namespace Bicep.Extension.Netbox.Handlers;

public class CircuitTypeHandler : SlugResourceHandler<CircuitType>
{
    protected override string ApiPath => "/api/circuits/circuit-types/";
}
