using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Token;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Refresh;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Refresh;

public class RefreshTokenCreator(IRefreshTokenRepository tokenRepository, IUnitOfWork unitOfWork, IRefreshTokenGenerator tokenGenerator) : IRefreshTokenCreator
{
    private readonly IRefreshTokenRepository _tokenRepository = tokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IRefreshTokenGenerator _tokenGenerator = tokenGenerator;

    public async Task<string> CreateAndStore(User user)
    {
        var refreshToken = new RefreshToken
        {
            Value = _tokenGenerator.Generate(),
            UserId = user!.Id,
        };

        await _tokenRepository.SaveNew(refreshToken);

        await _unitOfWork.Commit();

        return refreshToken.Value;
    }
}
