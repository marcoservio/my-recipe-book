using Microsoft.AspNetCore.Mvc;

using MyRecipeBook.API.Filters;

namespace MyRecipeBook.API.Attributes;

public class AuthenticatedUserAttribute() : TypeFilterAttribute(typeof(AuthenticatedUserFilter))
{
}
