using FluentValidation;

namespace WebApi.Models.Validators;

/// <summary>
/// Validator for the evaluation request that ensures all required data is present and valid.
/// Validates both the PDF document and the associated invoice details using FluentValidation.
/// </summary>
/// <remarks>
/// Validation rules include:
/// - PDF document must be present
/// - Document must be a valid PDF file
/// - File size must not exceed 5 MB
/// - Invoice details must be present and valid (validated by InvoiceValidator)
/// </remarks>
public class EvaluationRequestValidator : AbstractValidator<EvaluationRequest>
{
    /// <summary>
    /// Initializes a new instance of the EvaluationRequestValidator class.
    /// Sets up all validation rules for the evaluation request.
    /// </summary>
    public EvaluationRequestValidator()
    {
        RuleFor(x => x.Document)
            .NotNull().WithMessage("PDF document is required");

        RuleFor(x => x.Document)
            .Must(x => x?.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase) == true)
            .WithMessage("Only PDF documents are accepted")
            .When(x => x.Document != null);

        RuleFor(x => x.Document)
            .Must(x => x?.Length <= 5 * 1024 * 1024)
            .WithMessage("The maximum file size is 5 MB")
            .When(x => x.Document != null);

        RuleFor(x => x.Invoice)
            .NotNull().WithMessage("Invoice details are required")
            .SetValidator(new InvoiceValidator()!);
    }
}