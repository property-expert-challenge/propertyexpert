using WebApi.Models;

namespace WebApi.Services.Abstractions;

/// <summary>
/// Defines the contract for a client that communicates with the third-party classification service.
/// </summary>
public interface IClassificationHttpClient
{
    /// <summary>
    /// Sends the evaluation request to the classification service and retrieves the classification response.
    /// </summary>
    /// <param name="request">The evaluation request containing the document and invoice details to be classified.</param>
    /// <returns>A task that represents the asynchronous operation, containing the classification response with type and risk level.</returns>
    Task<ClassificationResponse> ClassifyAsync(EvaluationRequest request);
}