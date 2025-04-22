using Microsoft.AspNetCore.Mvc;

using MyRecipeBook.API.Controllers.Base;
using MyRecipeBook.Application.UseCases.Enums;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.CrossCutting.Attributes;

namespace MyRecipeBook.API.Controllers;

[AuthenticatedUser]
public class EnumsController : MyRecipeBookBaseController
{
    [HttpGet]
    [Route("{enumName}")]
    [ProducesResponseType(typeof(ResponseEnumsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public IActionResult GetEnum(
        [FromServices] IGetEnumUseCase useCase,
        [FromRoute] string enumName)
    {
        var response = useCase.Execute(enumName);

        return Ok(response);
    }
}
