using MyRecipeBook.Site;
using MyRecipeBook.Site.Components;
using MyRecipeBook.Site.Extensions;
using MyRecipeBook.Site.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpClient();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Backend"));

builder.Services.AddRefitClientCustom<ILoginService>();
builder.Services.AddRefitClientCustom<IUserService>();
builder.Services.AddRefitClientCustom<IRecipeService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

await app.RunAsync();