using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.CodeToPerformAction;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.Email;

namespace MyRecipeBook.Application.UseCases.Login.RequestCode;

public class RequestCodeResetPasswordUseCase(IUserReadOnlyRepository userReadOnlyRepository, ICodeToPerformActionRepository codeRepository, IUnitOfWork unitOfWork, ISendCodeResetPassword emailSender) : IRequestCodeResetPasswordUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
    private readonly ICodeToPerformActionRepository _codeRepository = codeRepository;
    private readonly ISendCodeResetPassword _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(string email)
    {
        var user = await _userReadOnlyRepository.GetByEmail(email);

        if (user is not null)
        {
            var codeRandom = Guid.NewGuid();

            var code = new CodeToPerformAction
            {
                Value = codeRandom.ToString(),
                UserId = user.Id
            };

            await _codeRepository.Add(code);

            await _unitOfWork.Commit();

            await _emailSender.SendEmail(user, codeRandom.ToString());
        }
    }
}
