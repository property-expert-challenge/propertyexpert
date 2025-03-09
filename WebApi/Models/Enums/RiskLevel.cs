namespace WebApi.Models.Enums;

/// <summary>
/// Represents the risk assessment level associated with an invoice classification.
/// Used to determine the appropriate evaluation action.
/// </summary>
public enum RiskLevel
{
    /// <summary>
    /// Indicates minimal risk, typically leading to automatic approval.
    /// </summary>
    Low,

    /// <summary>
    /// Indicates moderate risk, may require additional verification.
    /// </summary>
    Medium,

    /// <summary>
    /// Indicates significant risk, typically requiring manual review.
    /// </summary>
    High
}