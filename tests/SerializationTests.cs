using System.Text.Json;
using Bicep.Extension.Netbox;

namespace Bicep.Extension.Netbox.Tests;

/// <summary>
/// Verifies that all models serialize to snake_case JSON that the NetBox API expects.
/// Catches the exact bug we hit: C# PascalCase properties must become snake_case for NetBox.
/// </summary>
public class SerializationTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    [Fact]
    public void Site_Serializes_To_SnakeCase()
    {
        var site = new Site
        {
            Name = "Test Site",
            Slug = "test-site",
            Status = "active",
            PhysicalAddress = "123 Main St",
            ShippingAddress = "456 Ship St",
            TimeZone = "Europe/Stockholm"
        };

        var json = JsonSerializer.Serialize(site, JsonOptions);
        var doc = JsonDocument.Parse(json);

        Assert.True(doc.RootElement.TryGetProperty("name", out _));
        Assert.True(doc.RootElement.TryGetProperty("slug", out _));
        Assert.True(doc.RootElement.TryGetProperty("physical_address", out _));
        Assert.True(doc.RootElement.TryGetProperty("shipping_address", out _));
        Assert.True(doc.RootElement.TryGetProperty("time_zone", out _));

        // Must NOT contain PascalCase keys
        Assert.False(doc.RootElement.TryGetProperty("PhysicalAddress", out _));
        Assert.False(doc.RootElement.TryGetProperty("TimeZone", out _));
    }

    [Fact]
    public void Device_Serializes_FK_Fields_As_SnakeCase()
    {
        var device = new Device
        {
            Name = "switch-01",
            DeviceType = "1",
            Role = "2",
            Site = "3",
            AssetTag = "ABC123"
        };

        var json = JsonSerializer.Serialize(device, JsonOptions);
        var doc = JsonDocument.Parse(json);

        Assert.True(doc.RootElement.TryGetProperty("device_type", out _));
        Assert.True(doc.RootElement.TryGetProperty("asset_tag", out _));
        Assert.False(doc.RootElement.TryGetProperty("DeviceType", out _));
        Assert.False(doc.RootElement.TryGetProperty("AssetTag", out _));
    }

    [Fact]
    public void IPRange_Serializes_Correctly()
    {
        var range = new IPRange
        {
            StartAddress = "10.0.0.100/24",
            EndAddress = "10.0.0.200/24",
            Status = "active"
        };

        var json = JsonSerializer.Serialize(range, JsonOptions);
        var doc = JsonDocument.Parse(json);

        Assert.True(doc.RootElement.TryGetProperty("start_address", out _));
        Assert.True(doc.RootElement.TryGetProperty("end_address", out _));
        Assert.False(doc.RootElement.TryGetProperty("StartAddress", out _));
    }

    [Fact]
    public void VirtualMachine_Serializes_Correctly()
    {
        var vm = new VirtualMachine
        {
            Name = "web-01",
            Vcpus = "4",
            Memory = "8192",
            PrimaryIp4 = "1"
        };

        var json = JsonSerializer.Serialize(vm, JsonOptions);
        var doc = JsonDocument.Parse(json);

        Assert.True(doc.RootElement.TryGetProperty("primary_ip4", out _));
        Assert.False(doc.RootElement.TryGetProperty("PrimaryIp4", out _));
    }

    [Fact]
    public void VMInterface_Serializes_VirtualMachine_FK()
    {
        var iface = new VMInterface
        {
            Name = "eth0",
            VirtualMachine = "1",
            UntaggedVlan = "100"
        };

        var json = JsonSerializer.Serialize(iface, JsonOptions);
        var doc = JsonDocument.Parse(json);

        Assert.True(doc.RootElement.TryGetProperty("virtual_machine", out _));
        Assert.True(doc.RootElement.TryGetProperty("untagged_vlan", out _));
        Assert.False(doc.RootElement.TryGetProperty("VirtualMachine", out _));
    }

    [Fact]
    public void DeviceRole_VmRole_Serializes_As_SnakeCase()
    {
        var role = new DeviceRole
        {
            Name = "Router",
            Slug = "router",
            VmRole = "true"
        };

        var json = JsonSerializer.Serialize(role, JsonOptions);
        var doc = JsonDocument.Parse(json);

        Assert.True(doc.RootElement.TryGetProperty("vm_role", out _));
        Assert.False(doc.RootElement.TryGetProperty("VmRole", out _));
    }

    [Fact]
    public void Null_Optional_Fields_Are_Excluded()
    {
        var site = new Site
        {
            Name = "Test",
            Slug = "test"
        };

        var json = JsonSerializer.Serialize(site, JsonOptions);
        var doc = JsonDocument.Parse(json);

        Assert.False(doc.RootElement.TryGetProperty("status", out _));
        Assert.False(doc.RootElement.TryGetProperty("description", out _));
        Assert.False(doc.RootElement.TryGetProperty("comments", out _));
    }

    [Fact]
    public void Configuration_Serializes_Correctly()
    {
        var config = new Configuration
        {
            Url = "http://localhost:8000",
            Token = "nbt_test.token123"
        };

        var json = JsonSerializer.Serialize(config, JsonOptions);
        Assert.Contains("\"url\"", json);
        Assert.Contains("\"token\"", json);
        Assert.DoesNotContain("Url", json);
    }
}
