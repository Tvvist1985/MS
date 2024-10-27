using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PET;
using PET.Models;
using PET.Services.HttpClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClient(); //Фабрика HTTPClient
builder.Services.AddScoped<IHttpClient, PET.Services.HttpClient.HttpClient>();
builder.Services.AddScoped<MainModel>();


await builder.Build().RunAsync();
