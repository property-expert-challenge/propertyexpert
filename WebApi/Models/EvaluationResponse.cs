using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebApi.Models.Enums;

namespace WebApi.Models;

/// <summary>
/// Represents the response returned by the evaluation endpoint containing the assessment results.
/// </summary>
public record EvaluationResponse
{
    /// <summary>
    /// A unique identifier for the evaluation (e.g., EVAL001).
    /// </summary>
    public required string EvaluationId { get; init; }

    /// <summary>
    /// The identifier of the invoice that was evaluated.
    /// </summary>
    public required string InvoiceId { get; init; }

    /// <summary>
    /// Collection of rules that were applied during the evaluation process (e.g., ["Approve", "Flag for Review"]).
    /// </summary>
    public required IReadOnlyCollection<string> RulesApplied { get; init; }

    /// <summary>
    /// The classification type determined for the invoice.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public required Classification Classification { get; init; }

    /// <summary>
    /// Base64 encoded string of the evaluation summary text file explaining the results in plain English.
    /// </summary>
    public required string EvaluationFile { get; init; }
}