using Bicep.Extension.Netbox;
using Bicep.Extension.Netbox.Handlers;
using Bicep.Local.Extension.Host.Extensions;

var builder = WebApplication.CreateBuilder();

builder.AddBicepExtensionHost(args);
builder.Services
    .AddBicepExtension(
        name: "netbox",
        version: "0.1.0",
        isSingleton: true,
        typeAssembly: typeof(Program).Assembly,
        configurationType: typeof(Configuration))
    // DCIM
    .WithResourceHandler<SiteHandler>()
    .WithResourceHandler<ManufacturerHandler>()
    .WithResourceHandler<DeviceRoleHandler>()
    .WithResourceHandler<DeviceTypeHandler>()
    .WithResourceHandler<DeviceHandler>()
    .WithResourceHandler<RegionHandler>()
    .WithResourceHandler<SiteGroupHandler>()
    .WithResourceHandler<PlatformHandler>()
    .WithResourceHandler<InterfaceHandler>()
    .WithResourceHandler<RackTypeHandler>()
    .WithResourceHandler<RackRoleHandler>()
    .WithResourceHandler<LocationHandler>()
    .WithResourceHandler<RackHandler>()
    // Tenancy
    .WithResourceHandler<TenantHandler>()
    .WithResourceHandler<TenantGroupHandler>()
    .WithResourceHandler<ContactHandler>()
    .WithResourceHandler<ContactGroupHandler>()
    .WithResourceHandler<ContactRoleHandler>()
    // IPAM
    .WithResourceHandler<PrefixHandler>()
    .WithResourceHandler<IPAddressHandler>()
    .WithResourceHandler<VLANHandler>()
    .WithResourceHandler<VRFHandler>()
    .WithResourceHandler<RouteTargetHandler>()
    .WithResourceHandler<RIRHandler>()
    .WithResourceHandler<AggregateHandler>()
    .WithResourceHandler<IPAMRoleHandler>()
    .WithResourceHandler<IPRangeHandler>()
    .WithResourceHandler<ASNHandler>()
    .WithResourceHandler<ASNRangeHandler>()
    .WithResourceHandler<VLANGroupHandler>()
    .WithResourceHandler<VLANTranslationPolicyHandler>()
    // Virtualization
    .WithResourceHandler<ClusterTypeHandler>()
    .WithResourceHandler<ClusterGroupHandler>()
    .WithResourceHandler<ClusterHandler>()
    .WithResourceHandler<VirtualMachineHandler>()
    .WithResourceHandler<VMInterfaceHandler>()
    .WithResourceHandler<VirtualDiskHandler>()
    // Circuits
    .WithResourceHandler<CircuitTypeHandler>()
    .WithResourceHandler<ProviderHandler>()
    .WithResourceHandler<CircuitHandler>()
    // Extras
    .WithResourceHandler<TagHandler>()
    // VPN
    .WithResourceHandler<TunnelGroupHandler>()
    .WithResourceHandler<TunnelHandler>()
    .WithResourceHandler<IKEProposalHandler>()
    .WithResourceHandler<IKEPolicyHandler>()
    .WithResourceHandler<IPSecProposalHandler>()
    .WithResourceHandler<IPSecPolicyHandler>()
    .WithResourceHandler<IPSecProfileHandler>()
    .WithResourceHandler<L2VPNHandler>()
    // Wireless
    .WithResourceHandler<WirelessLANGroupHandler>()
    .WithResourceHandler<WirelessLANHandler>()
    .WithResourceHandler<WirelessLinkHandler>()
    // Users
    .WithResourceHandler<UserHandler>()
    .WithResourceHandler<UserGroupHandler>();

var app = builder.Build();
app.MapBicepExtension();
await app.RunAsync();
