using AutoMapper;

using FileTypeChecker.Extensions;
using FileTypeChecker.Types;

using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Register;

public class RegisterRecipeUseCase(IRecipeWriteOnlyRepository writeOnlyRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork, IMapper mapper, IBlobStorageService blobStorageService) : IRegisterRecipeUseCase
{
    private readonly IRecipeWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IBlobStorageService _blobStorageService = blobStorageService;

    public async Task<ResponseRegisteredRecipeJson> Execute(RequestRegisterRecipeFormData request)
    {
        Validate(request);

        var authenticatedUser = await _loggedUser.User();

        var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
        recipe.UserId = authenticatedUser.Id;

        var instruction = request.Instructions.OrderBy(i => i.Step).ToList();
        for (var i = 0; i < instruction.Count; i++)
            instruction[i].Step = i + 1;

        recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instruction);

        if (request.Image is not null)
        {
            var fileStream = request.Image.OpenReadStream();

            (var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

            if(isValidImage.IsFalse())
                throw new OnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

            recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}";

            await _blobStorageService.Upload(authenticatedUser, fileStream, recipe.ImageIdentifier!);
        }

        await _writeOnlyRepository.Add(recipe);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisteredRecipeJson>(recipe);
    }

    private static void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if (result.IsValid.IsFalse())
            throw new OnValidationException([.. result.Errors.Select(e => e.ErrorMessage).Distinct()]);
    }
}
