using Application.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Ui.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});
builder.Services.AddScoped<IUserCsvService, UserApiCsvService>();
builder.Services.AddScoped<IUserService, UserApiService>();

await builder.Build().RunAsync();
