using MyRecipeBook.API.Converters;
using MyRecipeBook.API.Extensions;
using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middleware;
using MyRecipeBook.API.Token;
using MyRecipeBook.Application;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwagger();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddApplication(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase(builder.Services, builder.Configuration);

await app.RunAsync();

public partial class Program
{
    protected Program() { }
}