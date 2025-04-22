using AutoMapper;

using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Update;

public class UpdateRecipeUseCase(ILoggedUser loggedUser, IUnitOfWork unitOfWork, IMapper mapper, IRecipeUpdateOnlyRepository updateOnlyRepository) : IUpdateRecipeUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IRecipeUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;

    public async Task Execute(long recipeId, RequestRecipeJson request)
    {
        Validate(request);

        var authenticatedUser = await _loggedUser.User();

        var recipe = await _updateOnlyRepository.GetById(authenticatedUser, recipeId) ?? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        recipe.Ingredients.Clear();
        recipe.Instructions.Clear();
        recipe.DishTypes.Clear();

        _mapper.Map(request, recipe);

        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for (int i = 0; i < instructions.Count; i++)
            instructions[i].Step = i + 1;

        recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

        _updateOnlyRepository.Update(recipe);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);

        if (result.IsValid.IsFalse())
            throw new OnValidationException([.. result.Errors.Select(e => e.ErrorMessage).Distinct()]);
    }
}
