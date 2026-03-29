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
    // Tenancy
    .WithResourceHandler<TenantHandler>()
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
    .WithResourceHandler<VLANTranslationPolicyHandler>();

var app = builder.Build();
app.MapBicepExtension();
await app.RunAsync();
