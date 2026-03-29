using Bicep.Extension.Netbox;
using Bicep.Extension.Netbox.Handlers;

namespace Bicep.Extension.Netbox.Tests;

/// <summary>
/// Tests that GetIdentifiers correctly extracts the identifier from each resource.
/// </summary>
public class IdentifierTests
{
    [Fact]
    public void SiteHandler_Returns_Name()
    {
        var handler = new SiteHandler();
        var ids = InvokeGetIdentifiers(handler, new Site { Name = "Stockholm DC1" });
        Assert.IsType<NameSlugIdentifiers>(ids);
        Assert.Equal("Stockholm DC1", ((NameSlugIdentifiers)ids).Name);
    }

    [Fact]
    public void DeviceHandler_Returns_Name()
    {
        var handler = new DeviceHandler();
        var ids = InvokeGetIdentifiers(handler, new Device { Name = "sw-01", DeviceType = "1", Role = "1", Site = "1" });
        Assert.IsType<DeviceIdentifiers>(ids);
        Assert.Equal("sw-01", ((DeviceIdentifiers)ids).Name);
    }

    [Fact]
    public void VLANHandler_Returns_Vid()
    {
        var handler = new VLANHandler();
        var ids = InvokeGetIdentifiers(handler, new VLAN { Vid = 100, Name = "Mgmt" });
        Assert.IsType<VLANIdentifiers>(ids);
        Assert.Equal(100, ((VLANIdentifiers)ids).Vid);
    }

    [Fact]
    public void PrefixHandler_Returns_Prefix()
    {
        var handler = new PrefixHandler();
        var ids = InvokeGetIdentifiers(handler, new Prefix { Prefix = "10.0.0.0/24" });
        Assert.IsType<PrefixIdentifiers>(ids);
        Assert.Equal("10.0.0.0/24", ((PrefixIdentifiers)ids).Prefix);
    }

    [Fact]
    public void ASNHandler_Returns_Asn()
    {
        var handler = new ASNHandler();
        var ids = InvokeGetIdentifiers(handler, new ASN { Asn = 65000, Rir = "1" });
        Assert.IsType<ASNIdentifiers>(ids);
        Assert.Equal(65000, ((ASNIdentifiers)ids).Asn);
    }

    [Fact]
    public void UserHandler_Returns_Username()
    {
        var handler = new UserHandler();
        var ids = InvokeGetIdentifiers(handler, new User { Username = "jane", Password = "x" });
        Assert.IsType<UsernameIdentifiers>(ids);
        Assert.Equal("jane", ((UsernameIdentifiers)ids).Username);
    }

    [Fact]
    public void VirtualMachineHandler_Returns_Name()
    {
        var handler = new VirtualMachineHandler();
        var ids = InvokeGetIdentifiers(handler, new VirtualMachine { Name = "web-01" });
        Assert.IsType<NameIdentifiers>(ids);
        Assert.Equal("web-01", ((NameIdentifiers)ids).Name);
    }

    private static object InvokeGetIdentifiers<TProps, TIds>(NetboxResourceHandlerBase<TProps, TIds> handler, TProps properties)
        where TProps : class, TIds
        where TIds : class
    {
        var method = handler.GetType().GetMethod("GetIdentifiers",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(method);
        return method!.Invoke(handler, [properties])!;
    }
}
