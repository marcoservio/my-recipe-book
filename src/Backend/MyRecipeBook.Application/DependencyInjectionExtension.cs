using Microsoft.Extensions.DependencyInjection;

using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.UseCase.Login.DoLogin;
using MyRecipeBook.Application.UseCase.User.ChangePassword;
using MyRecipeBook.Application.UseCase.User.Profile;
using MyRecipeBook.Application.UseCase.User.Register;
using MyRecipeBook.Application.UseCase.User.Update;

namespace MyRecipeBook.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }).CreateMapper());
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
    }    
}
