using System.Text;
using WebApi.Models;
using WebApi.Services.Abstractions;

namespace WebApi.Services;

/// <summary>
/// Implementation of the IEvaluationService interface that processes invoice evaluations.
/// Coordinates with the classification service and applies business rules to generate evaluation results.
/// </summary>
/// <param name="classificationHttpClient">The client used to communicate with the classification service.</param>
public class EvaluationService(IClassificationHttpClient classificationHttpClient) : IEvaluationService
{
    /// <summary>
    /// Evaluates the provided invoice document and details by classifying it and applying business rules.
    /// </summary>
    /// <param name="request">The evaluation request containing the document and invoice details to be evaluated.</param>
    /// <returns>A task that represents the asynchronous operation, containing the evaluation response with results and summary.</returns>
    public async Task<EvaluationResponse> EvaluateAsync(EvaluationRequest request)
    {
        var classificationResponse = await classificationHttpClient.ClassifyAsync(request);
        return new EvaluationResponse
        {
            EvaluationId = $"EVAL_{Guid.NewGuid()}",
            InvoiceId = request.Invoice!.InvoiceId!,
            RulesApplied = ["Approved"], // TODO: Implement rule engine
            Classification = classificationResponse.Classification,
            EvaluationFile = GenerateEvaluationSummary(classificationResponse, request)
        };
    }

    /// <summary>
    /// Generates a text summary of the evaluation results and converts it to a base64 encoded string.
    /// </summary>
    /// <param name="classificationResponse">The classification results from the third-party service.</param>
    /// <param name="request">The original evaluation request containing invoice details.</param>
    /// <returns>A base64 encoded string containing the evaluation summary.</returns>
    private static string GenerateEvaluationSummary(ClassificationResponse classificationResponse,
        EvaluationRequest request)
    {
        var summary = new StringBuilder();
        summary.AppendLine("Evaluation ID: EVAL001");
        summary.AppendLine($"Invoice ID: {request.Invoice!.InvoiceId}");
        summary.AppendLine($"Classification: {classificationResponse.Classification}");
        summary.AppendLine($"Risk Level: {classificationResponse.RiskLevel}");
        summary.AppendLine("Rules Applied: Approved");

        var filePath = Path.GetTempFileName();
        File.WriteAllText(filePath, summary.ToString());

        var fileBytes = File.ReadAllBytes(filePath);
        return Convert.ToBase64String(fileBytes);
    }
}