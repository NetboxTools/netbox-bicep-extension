using Bicep.Extension.Netbox;
using Bicep.Extension.Netbox.Handlers;

namespace Bicep.Extension.Netbox.Tests;

/// <summary>
/// Tests that each handler generates the correct API lookup query.
/// This is critical — wrong queries cause duplicate resources instead of updates.
/// </summary>
public class HandlerLookupTests
{
    // ── Slug-based handlers ────────────────────────────

    [Fact]
    public void SiteHandler_LookupQuery()
    {
        var handler = new SiteHandler();
        var query = InvokeLookup(handler, new Site { Name = "Test", Slug = "test-site" });
        Assert.Equal("slug=test-site", query);
    }

    [Fact]
    public void ManufacturerHandler_LookupQuery()
    {
        var handler = new ManufacturerHandler();
        var query = InvokeLookup(handler, new Manufacturer { Name = "Cisco", Slug = "cisco" });
        Assert.Equal("slug=cisco", query);
    }

    [Fact]
    public void DeviceRoleHandler_LookupQuery()
    {
        var handler = new DeviceRoleHandler();
        var query = InvokeLookup(handler, new DeviceRole { Name = "Router", Slug = "router" });
        Assert.Equal("slug=router", query);
    }

    [Fact]
    public void RIRHandler_LookupQuery()
    {
        var handler = new RIRHandler();
        var query = InvokeLookup(handler, new RIR { Name = "RIPE", Slug = "ripe" });
        Assert.Equal("slug=ripe", query);
    }

    [Fact]
    public void RackRoleHandler_LookupQuery()
    {
        var handler = new RackRoleHandler();
        var query = InvokeLookup(handler, new RackRole { Name = "Compute", Slug = "compute" });
        Assert.Equal("slug=compute", query);
    }

    // ── Name-based handlers ────────────────────────────

    [Fact]
    public void VRFHandler_LookupQuery()
    {
        var handler = new VRFHandler();
        var query = InvokeLookup(handler, new VRF { Name = "Global" });
        Assert.Equal("name=Global", query);
    }

    [Fact]
    public void ClusterHandler_LookupQuery()
    {
        var handler = new ClusterHandler();
        var query = InvokeLookup(handler, new Cluster { Name = "Azure West", Type = "1" });
        Assert.Equal("name=Azure West", query);
    }

    [Fact]
    public void UserGroupHandler_LookupQuery()
    {
        var handler = new UserGroupHandler();
        var query = InvokeLookup(handler, new UserGroup { Name = "Admins" });
        Assert.Equal("name=Admins", query);
    }

    // ── Scoped lookups (name + parent FK) ──────────────

    [Fact]
    public void RackHandler_LookupQuery_Includes_SiteId()
    {
        var handler = new RackHandler();
        var query = InvokeLookup(handler, new Rack { Name = "A-01", Site = "5" });
        Assert.Equal("name=A-01&site_id=5", query);
    }

    [Fact]
    public void VMInterfaceHandler_LookupQuery_Includes_VmId()
    {
        var handler = new VMInterfaceHandler();
        var query = InvokeLookup(handler, new VMInterface { Name = "eth0", VirtualMachine = "3" });
        Assert.Equal("name=eth0&virtual_machine_id=3", query);
    }

    [Fact]
    public void VirtualDiskHandler_LookupQuery_Includes_VmId()
    {
        var handler = new VirtualDiskHandler();
        var query = InvokeLookup(handler, new VirtualDisk { Name = "disk0", VirtualMachine = "7", Size = 1024 });
        Assert.Equal("name=disk0&virtual_machine_id=7", query);
    }

    // ── Special identifier handlers ────────────────────

    [Fact]
    public void PrefixHandler_LookupQuery()
    {
        var handler = new PrefixHandler();
        var query = InvokeLookup(handler, new Prefix { Prefix = "10.0.0.0/24" });
        Assert.Equal("prefix=10.0.0.0/24", query);
    }

    [Fact]
    public void IPAddressHandler_LookupQuery()
    {
        var handler = new IPAddressHandler();
        var query = InvokeLookup(handler, new IPAddress { Address = "10.0.0.1/32" });
        Assert.Equal("address=10.0.0.1/32", query);
    }

    [Fact]
    public void VLANHandler_LookupQuery()
    {
        var handler = new VLANHandler();
        var query = InvokeLookup(handler, new VLAN { Vid = 100, Name = "Mgmt" });
        Assert.Equal("vid=100", query);
    }

    [Fact]
    public void ASNHandler_LookupQuery()
    {
        var handler = new ASNHandler();
        var query = InvokeLookup(handler, new ASN { Asn = 65000, Rir = "1" });
        Assert.Equal("asn=65000", query);
    }

    [Fact]
    public void IPRangeHandler_LookupQuery()
    {
        var handler = new IPRangeHandler();
        var query = InvokeLookup(handler, new IPRange { StartAddress = "10.0.0.100/24", EndAddress = "10.0.0.200/24" });
        Assert.Equal("start_address=10.0.0.100/24", query);
    }

    [Fact]
    public void UserHandler_LookupQuery()
    {
        var handler = new UserHandler();
        var query = InvokeLookup(handler, new User { Username = "jane.doe", Password = "x" });
        Assert.Equal("username=jane.doe", query);
    }

    // ── Helper ─────────────────────────────────────────

    /// <summary>
    /// Invokes the protected GetLookupQuery method via reflection.
    /// </summary>
    private static string InvokeLookup<TProps, TIds>(NetboxResourceHandlerBase<TProps, TIds> handler, TProps properties)
        where TProps : class, TIds
        where TIds : class
    {
        var method = handler.GetType().GetMethod("GetLookupQuery",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(method);
        return (string)method!.Invoke(handler, [properties])!;
    }
}
