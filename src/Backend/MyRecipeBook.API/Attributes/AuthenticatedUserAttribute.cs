using Microsoft.AspNetCore.Mvc;

using MyRecipeBook.API.Filters;

namespace MyRecipeBook.API.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthenticatedUserAttribute() : TypeFilterAttribute(typeof(AuthenticatedUserFilter))
{
}
