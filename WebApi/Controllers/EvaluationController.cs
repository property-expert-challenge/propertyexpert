using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services.Abstractions;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EvaluationController(
    IValidator<EvaluationRequest> evaluationRequestValidator,
    IEvaluationService evaluationService,
    ILogger<EvaluationController> logger) : ControllerBase
{
    [HttpPost("evaluate")]
    public async Task<IActionResult> Evaluate([FromForm] EvaluationRequest request)
    {
        using (logger.BeginScope("Evaluation request: {request}", request))
        {
            logger.LogInformation("Received evaluation request");
        }

        var validationResult = await evaluationRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            logger.LogError("Evaluation request validation failed: {errors}", errors);
            return BadRequest(errors);
        }

        var evaluationResponse = await evaluationService.EvaluateAsync(request);
        logger.LogInformation("Evaluation completed: {evaluationResponse}", evaluationResponse);

        return Ok(evaluationResponse);
    }
}