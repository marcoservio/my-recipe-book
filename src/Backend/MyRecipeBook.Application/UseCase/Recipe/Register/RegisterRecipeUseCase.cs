﻿using AutoMapper;

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

namespace MyRecipeBook.Application.UseCase.Recipe.Register;

public class RegisterRecipeUseCase(IRecipeWriteOnlyRepository repository, ILoggedUser loggedUser, IUnitOfWork unitOfWork, IMapper mapper, IBlobStorageService bobStorageService) : IRegisterRecipeUseCase
{
    private readonly IRecipeWriteOnlyRepository _repository = repository;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IBlobStorageService _bobStorageService = bobStorageService;

    public async Task<ResponseRegisterRecipeJson> Execute(RequestRegisterRecipeFormData request)
    {
        Validate(request);

        var logged = await _loggedUser.User();

        var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
        recipe.UserId = logged.Id;

        var instructionsOrderned = OrderIngredients(request);

        recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructionsOrderned);

        if(request.Image is not null)
        {
            var fileStream = request.Image.OpenReadStream();

            (var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

            if (isValidImage.IsFalse())
                throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

            recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}";

            await _bobStorageService.Upload(logged, fileStream, recipe.ImageIdentifier);
        }

        await _repository.Add(recipe);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisterRecipeJson>(recipe);
    }

    private static List<RequestInstructionJson> OrderIngredients(RequestRecipeJson request)
    {
        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();

        for (int index = 0; index < instructions.Count; index++)
            instructions[index].Step = index + 1;

        return instructions;
    }

    private static void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}
