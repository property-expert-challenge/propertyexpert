using FluentValidation;

namespace WebApi.Models.Validators;

/// <summary>
/// Validator for ensuring all invoice details are present and correctly formatted.
/// Implements validation rules for each field of the invoice using FluentValidation.
/// </summary>
/// <remarks>
/// Validation rules include:
/// - Invoice ID must be present
/// - Invoice number must be present and follow the format 'S' followed by 5 digits (e.g., S12345)
/// - Invoice date must be present
/// - Comment must be present
/// - Amount must be present and valid
/// </remarks>
public class InvoiceValidator : AbstractValidator<InvoiceDetails>
{
    /// <summary>
    /// Initializes a new instance of the InvoiceValidator class.
    /// Sets up all validation rules for the invoice details.
    /// </summary>
    public InvoiceValidator()
    {
        RuleFor(x => x.InvoiceId)
            .NotEmpty().WithMessage("Invoice ID is required");

        RuleFor(x => x.InvoiceNumber)
            .NotEmpty().WithMessage("Invoice number is required");

        RuleFor(x => x.InvoiceNumber)
            .Must(x => x!.StartsWith('S') && x.Length == 6 && int.TryParse(x[1..], out _))
            .WithMessage("Invoice number must start with 'S' followed by 5 digits")
            .When(x => !string.IsNullOrEmpty(x.InvoiceNumber));

        RuleFor(x => x.InvoiceDate)
            .NotEmpty().WithMessage("Invoice date is required");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required");

        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("Invoice amount is required");
    }
}