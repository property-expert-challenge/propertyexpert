using Microsoft.AspNetCore.Mvc;

namespace WebApi.Models;

/// <summary>
/// Represents a request for invoice evaluation, containing both the document and invoice details.
/// </summary>
public record EvaluationRequest
{
    /// <summary>
    /// The PDF document to be evaluated. Must be provided in form-data format.
    /// </summary>
    [FromForm] 
    public IFormFile? Document { get; init; }

    /// <summary>
    /// The invoice details associated with the document. Must be provided in form-data format.
    /// </summary>
    [FromForm] 
    public InvoiceDetails? Invoice { get; init; }
}