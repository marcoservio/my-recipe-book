using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.UseCase.Dashboard;
using MyRecipeBook.Application.UseCase.Login.DoLogin;
using MyRecipeBook.Application.UseCase.Recipe.Delete;
using MyRecipeBook.Application.UseCase.Recipe.Filter;
using MyRecipeBook.Application.UseCase.Recipe.Generate;
using MyRecipeBook.Application.UseCase.Recipe.GetById;
using MyRecipeBook.Application.UseCase.Recipe.Image;
using MyRecipeBook.Application.UseCase.Recipe.Register;
using MyRecipeBook.Application.UseCase.Recipe.Update;
using MyRecipeBook.Application.UseCase.User.ChangePassword;
using MyRecipeBook.Application.UseCase.User.Delete.Delete;
using MyRecipeBook.Application.UseCase.User.Delete.Request;
using MyRecipeBook.Application.UseCase.User.Profile;
using MyRecipeBook.Application.UseCase.User.Register;
using MyRecipeBook.Application.UseCase.User.Update;

using Sqids;

namespace MyRecipeBook.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services);
        AddIdEncoder(services, configuration);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option => new AutoMapper.MapperConfiguration(autoMapperOption =>
        {
            var sqids = option.GetService<SqidsEncoder<long>>()!;

            autoMapperOption.AddProfile(new AutoMapping(sqids));
        }).CreateMapper());
    }

    private static void AddIdEncoder(IServiceCollection services, IConfiguration configuration)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });

        services.AddSingleton(sqids);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();
        services.AddScoped<IRequestDeleteUserUseCase, RequestDeleteUserUseCase>();
        
        services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
        services.AddScoped<IFilterRecipeUseCase, FilterRecipeUseCase>();
        services.AddScoped<IGetRecipeByIdUserCase, GetRecipeByIdUserCase>();
        services.AddScoped<IDeleteRecipeUseCase, DeleteRecipeUseCase>();
        services.AddScoped<IUpdateRecipeUseCase, UpdateRecipeUseCase>();
        services.AddScoped<IGenerateRecipeUseCase, GenerateRecipeUseCase>();
        services.AddScoped<IAddUpdateImageCoverUseCase, AddUpdateImageCoverUseCase>();
        
        services.AddScoped<IGetDashboardUseCase, GetDashboardUseCase>();
    }    
}
