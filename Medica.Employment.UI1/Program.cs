using Medica.Employment.UI.Services;
using Medica.Employment.UI.Services.Contracts;
using Medica.Employment.UI;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Set the base address dynamically based on the environment
var baseAddress = builder.HostEnvironment.IsDevelopment()
    ? "http://localhost:16060"  // Development URL
    : "https://api.productionurl.com";  // Production URL

// Register the HttpClient with the dynamic base address
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(baseAddress) });



builder.Services.AddScoped<IEmployeeService, EmployeeService>();
await builder.Build().RunAsync();
