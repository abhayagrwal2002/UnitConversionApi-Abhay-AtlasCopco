using Microsoft.AspNetCore.Mvc;
using UnitConversion.Api.Models;
using UnitConversion.Api.Services;

namespace UnitConversion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ConversionsController : ControllerBase
{
    private readonly IConversionService _conversionService;

    public ConversionsController(IConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ConversionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<ConversionResponse> Convert(ConversionRequest request)
    {
        try
        {
            var result = _conversionService.Convert(
                request.Category,
                request.FromUnit,
                request.ToUnit,
                request.Value);

            return Ok(new ConversionResponse
            {
                InputValue = result.InputValue,
                FromUnit = result.FromUnit,
                ToUnit = result.ToUnit,
                Category = result.Category,
                Result = result.Result
            });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(ex.ParamName ?? "conversion", ex.Message);
            return ValidationProblem(ModelState);
        }
    }
}
