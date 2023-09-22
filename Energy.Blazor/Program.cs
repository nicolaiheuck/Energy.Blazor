using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Localization;
using Radzen;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Energy.Blazor.Extensions;
using Energy.Services.Services.IoT.Workers;
using Energy.Services.Interfaces;
using Energy.Services.Services.IoT.Channels;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Energy.Infrastructure.Mqtt.Configuration;
using Energy.Infrastructure.Mqtt.Services;
using Energy.Infrastructure.IoT;
using Energy.Infrastructure.IoT.Channels;
using Energy.Repositories.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

// Add authentication and authorization for API endpoints
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Add authentication and authorization for Blazor Server app
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});


var keyVaultEndpoint = new Uri(builder.Configuration["VaultUri"]);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// configure cultures
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "dk" };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

// BG services
builder.Services.AddHostedService<MqttIotWorker>();
builder.Services.AddSingleton<IIotMqttCommandChannel, IotMqttCommandChannel>();
builder.Services.AddTransient<IMqttIotService, MqttIotService>();
builder.Services.AddSingleton<IMqttService, MqttService>();
builder.Services.Configure<MqttConfig>(builder.Configuration.GetSection(MqttConfig.MqttSection));


// UI services
builder.Services.AddI18nText();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();
builder.Services.AddHotKeys2();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.RegisterDependencies();
builder.Services.AddDbContext<EgonContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("EgonDb"), new MySqlServerVersion(new Version(10, 9, 0)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();