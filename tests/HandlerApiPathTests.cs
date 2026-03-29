using Bicep.Extension.Netbox;
using Bicep.Extension.Netbox.Handlers;

namespace Bicep.Extension.Netbox.Tests;

/// <summary>
/// Verifies every handler maps to the correct NetBox API path.
/// Wrong paths silently create resources in the wrong endpoint.
/// </summary>
public class HandlerApiPathTests
{
    private static string GetApiPath<TProps, TIds>(NetboxResourceHandlerBase<TProps, TIds> handler)
        where TProps : class, TIds
        where TIds : class
    {
        var prop = handler.GetType().GetProperty("ApiPath",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(prop);
        return (string)prop!.GetValue(handler)!;
    }

    // ── DCIM ───────────────────────────────────────────

    [Theory]
    [InlineData(typeof(SiteHandler), "/api/dcim/sites/")]
    [InlineData(typeof(ManufacturerHandler), "/api/dcim/manufacturers/")]
    [InlineData(typeof(DeviceRoleHandler), "/api/dcim/device-roles/")]
    [InlineData(typeof(DeviceTypeHandler), "/api/dcim/device-types/")]
    [InlineData(typeof(DeviceHandler), "/api/dcim/devices/")]
    [InlineData(typeof(RackRoleHandler), "/api/dcim/rack-roles/")]
    [InlineData(typeof(LocationHandler), "/api/dcim/locations/")]
    [InlineData(typeof(RackHandler), "/api/dcim/racks/")]
    // Tenancy
    [InlineData(typeof(TenantHandler), "/api/tenancy/tenants/")]
    // IPAM
    [InlineData(typeof(PrefixHandler), "/api/ipam/prefixes/")]
    [InlineData(typeof(IPAddressHandler), "/api/ipam/ip-addresses/")]
    [InlineData(typeof(VLANHandler), "/api/ipam/vlans/")]
    [InlineData(typeof(VRFHandler), "/api/ipam/vrfs/")]
    [InlineData(typeof(RouteTargetHandler), "/api/ipam/route-targets/")]
    [InlineData(typeof(RIRHandler), "/api/ipam/rirs/")]
    [InlineData(typeof(AggregateHandler), "/api/ipam/aggregates/")]
    [InlineData(typeof(IPAMRoleHandler), "/api/ipam/roles/")]
    [InlineData(typeof(IPRangeHandler), "/api/ipam/ip-ranges/")]
    [InlineData(typeof(ASNHandler), "/api/ipam/asns/")]
    [InlineData(typeof(ASNRangeHandler), "/api/ipam/asn-ranges/")]
    [InlineData(typeof(VLANGroupHandler), "/api/ipam/vlan-groups/")]
    [InlineData(typeof(VLANTranslationPolicyHandler), "/api/ipam/vlan-translation-policies/")]
    // Virtualization
    [InlineData(typeof(ClusterTypeHandler), "/api/virtualization/cluster-types/")]
    [InlineData(typeof(ClusterGroupHandler), "/api/virtualization/cluster-groups/")]
    [InlineData(typeof(ClusterHandler), "/api/virtualization/clusters/")]
    [InlineData(typeof(VirtualMachineHandler), "/api/virtualization/virtual-machines/")]
    [InlineData(typeof(VMInterfaceHandler), "/api/virtualization/interfaces/")]
    [InlineData(typeof(VirtualDiskHandler), "/api/virtualization/virtual-disks/")]
    // Users
    [InlineData(typeof(UserHandler), "/api/users/users/")]
    [InlineData(typeof(UserGroupHandler), "/api/users/groups/")]
    public void Handler_Has_Correct_ApiPath(Type handlerType, string expectedPath)
    {
        var handler = Activator.CreateInstance(handlerType);
        Assert.NotNull(handler);

        var prop = handlerType.GetProperty("ApiPath",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(prop);

        var actualPath = (string)prop!.GetValue(handler)!;
        Assert.Equal(expectedPath, actualPath);
    }

    [Theory]
    [InlineData(typeof(SiteHandler))]
    [InlineData(typeof(RackHandler))]
    [InlineData(typeof(VLANHandler))]
    [InlineData(typeof(UserHandler))]
    public void ApiPath_Has_Leading_And_Trailing_Slashes(Type handlerType)
    {
        var handler = Activator.CreateInstance(handlerType);
        var prop = handlerType.GetProperty("ApiPath",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var path = (string)prop!.GetValue(handler)!;

        Assert.StartsWith("/", path);
        Assert.EndsWith("/", path);
    }
}
