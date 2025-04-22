using MyRecipeBook.CrossCutting;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilog();

builder.Services.AddApiConfiguration();
builder.Services.AddRateLimiting();
builder.Services.AddCustomCors();
builder.Services.AddCustomControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddFilters();
builder.Services.AddTokenProvider();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedServices(builder.Configuration);
builder.Services.AddGoogleAuthentication(builder.Configuration);
builder.Services.AddCustomHealthChecks();

var app = builder.Build();

app.UseRateLimiter();
app.UseCustomSwagger(app.Environment);
app.AddMiddlewares();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.EnsureDatabaseMigrated(builder.Configuration);
app.AddHealthCheck();

await app.RunAsync();

public partial class Program
{
    protected Program() { }
}