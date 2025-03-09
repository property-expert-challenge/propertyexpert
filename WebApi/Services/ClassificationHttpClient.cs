using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RestSharp;
using WebApi.Models;
using WebApi.Models.Options;
using WebApi.Services.Abstractions;

namespace WebApi.Services;

/// <summary>
/// Implementation of the IClassificationHttpClient interface that communicates with the third-party classification service.
/// Uses RestSharp for HTTP communication and Polly for resilience patterns.
/// </summary>
public class ClassificationHttpClient : IClassificationHttpClient
{
    private readonly RestClient _client;
    private readonly AsyncRetryPolicy<RestResponse> _retryPolicy;

    /// <summary>
    /// Initializes a new instance of the ClassificationHttpClient class.
    /// Sets up the RestClient with the base URL and configures the retry policy.
    /// </summary>
    /// <param name="options">The configuration options containing the base URL for the classification service.</param>
    public ClassificationHttpClient(IOptions<ClassificationApiOptions> options)
    {
        _client = new RestClient(options.Value.BaseUrl);

        _retryPolicy = Policy<RestResponse>
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    /// <summary>
    /// Sends the evaluation request to the classification service and retrieves the classification response.
    /// Implements exponential backoff retry policy for resilience.
    /// </summary>
    /// <param name="request">The evaluation request containing the document and invoice details to be classified.</param>
    /// <returns>A task that represents the asynchronous operation, containing the classification response with type and risk level.</returns>
    /// <exception cref="HttpRequestException">Thrown when the request fails, returns invalid response, or deserialization fails.</exception>
    public async Task<ClassificationResponse> ClassifyAsync(EvaluationRequest request)
    {
        var restRequest = new RestRequest("classify", Method.Post)
            .AddJsonBody(request);

        var response = await _retryPolicy.ExecuteAsync(async () => await _client.ExecuteAsync(restRequest));
        ValidateClassificationResponse(response);

        try
        {
            return JsonConvert.DeserializeObject<ClassificationResponse>(response.Content!)!;
        }
        catch (JsonException ex)
        {
            throw new HttpRequestException($"Failed to deserialize classification response: {response.Content}", ex,
                response.StatusCode);
        }
    }

    /// <summary>
    /// Validates the response from the classification service.
    /// </summary>
    /// <param name="response">The response received from the classification service.</param>
    /// <exception cref="HttpRequestException">Thrown when the response is not successful or content is null.</exception>
    private static void ValidateClassificationResponse(RestResponse response)
    {
        if (!response.IsSuccessful)
        {
            throw new HttpRequestException(
                $"Classification request failed with status code {response.StatusCode}: {response.ErrorMessage}", null,
                response.StatusCode);
        }

        if (response.Content == null)
        {
            throw new HttpRequestException("Classification response content is null", null, response.StatusCode);
        }
    }
}