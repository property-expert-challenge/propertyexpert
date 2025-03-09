namespace WebApi.Models;

/// <summary>
/// Represents the details of an invoice submitted for evaluation.
/// </summary>
public record InvoiceDetails
{
    /// <summary>
    /// The unique identifier for the invoice.
    /// </summary>
    public string? InvoiceId { get; init; }

    /// <summary>
    /// The invoice number assigned by the craftsman. Must start with "S" followed by 5 digits (e.g., S12345).
    /// </summary>
    public string? InvoiceNumber { get; init; }

    /// <summary>
    /// The date when the invoice was issued.
    /// </summary>
    public DateTime? InvoiceDate { get; init; }

    /// <summary>
    /// Additional comments provided by the insurance company.
    /// </summary>
    public string? Comment { get; init; }

    /// <summary>
    /// The total amount of the invoice.
    /// </summary>
    public decimal? Amount { get; init; }
}