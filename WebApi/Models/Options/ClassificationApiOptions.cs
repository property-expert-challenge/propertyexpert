using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Options;

/// <summary>
/// Configuration options for the third-party classification API service.
/// This class is used to bind configuration values from appsettings.json or environment variables.
/// </summary>
public class ClassificationApiOptions
{
    /// <summary>
    /// Gets or sets the base URL of the classification API service.
    /// This value must be provided through configuration or environment variables.
    /// </summary>
    /// <remarks>
    /// The value should be a valid URL including the protocol (e.g., https://api.classification-service.com).
    /// This is configured through the ClassificationApiOptions:BaseUrl environment variable.
    /// </remarks>
    [Required(ErrorMessage = "Environment variable ClassificationApiOptions:BaseUrl is required")]
    public required string BaseUrl { get; set; }
}