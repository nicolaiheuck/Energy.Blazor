using Blazored.Modal;
using Blazored.Toast;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using Radzen;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Energy.Blazor.Extensions;
using Mcc.Infrastructure.Mqtt.Configuration;
using Mcc.Infrastructure.Mqtt.Services;
using Microsoft.Extensions.Configuration;
using Mcc.Application.Features.Robotool.Workers;
using Mcc.Application.Contracts.Infrastructure;
using Mcc.Infrastructure.Robotool;
using Mcc.Application.Features.Robotool.Channels;
using Mcc.Infrastructure.Robotool.Channels;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();