using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebApi.Models.Enums;

namespace WebApi.Models;

/// <summary>
/// Represents the response from the third-party classification service.
/// Contains the classification type and risk level assessment of the invoice.
/// </summary>
public record ClassificationResponse
{
    /// <summary>
    /// The type of work or service classification for the invoice (e.g., WaterLeakDetection, RoofingTileReplacement).
    /// </summary>
    [JsonProperty("classification")]
    [JsonConverter(typeof(StringEnumConverter))]
    public Classification Classification { get; init; }

    /// <summary>
    /// The assessed risk level associated with the invoice classification.
    /// </summary>
    [JsonProperty("riskLevel")]
    [JsonConverter(typeof(StringEnumConverter))]
    public RiskLevel RiskLevel { get; init; }
}