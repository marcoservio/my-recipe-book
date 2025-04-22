using Microsoft.AspNetCore.Mvc;

using MyRecipeBook.CrossCutting.Filters;

namespace MyRecipeBook.CrossCutting.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
    {
    }
}
