using AutoMapper;

using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.User.Profile;

public class GetUserProfileUseCase(ILoggedUser loogedUser, IMapper mapper) : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loogedUser = loogedUser;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loogedUser.User();

        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}
