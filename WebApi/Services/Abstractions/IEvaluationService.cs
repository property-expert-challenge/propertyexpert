using WebApi.Models;

namespace WebApi.Services.Abstractions;

/// <summary>
/// Defines the contract for a service that evaluates invoice documents and their associated details.
/// </summary>
public interface IEvaluationService
{
    /// <summary>
    /// Evaluates the provided invoice document and details, applying business rules and generating an assessment.
    /// </summary>
    /// <param name="request">The evaluation request containing the document and invoice details to be evaluated.</param>
    /// <returns>A task that represents the asynchronous operation, containing the evaluation response with results and summary.</returns>
    Task<EvaluationResponse> EvaluateAsync(EvaluationRequest request);
}