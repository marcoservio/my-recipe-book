using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MyRecipeBook.Site;
using MyRecipeBook.Site.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddLocalStorage();
builder.Services.AddHandlers();
builder.AddServices();

await builder.Build().RunAsync();
