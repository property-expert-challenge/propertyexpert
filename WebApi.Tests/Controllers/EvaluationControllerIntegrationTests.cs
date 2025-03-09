using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using WebApi.Models;

namespace WebApi.Tests.Controllers;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Ensure the content root path is set correctly
        builder.UseContentRoot(Directory.GetCurrentDirectory());
        return base.CreateHost(builder);
    }
}

[TestFixture]
public class EvaluationControllerIntegrationTests
{
    private CustomWebApplicationFactory _factory;
    private HttpClient _client;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task Evaluate_WithValidPdfAndInvoice_ShouldReturnOk()
    {
        // Arrange
        using var multipartContent = new MultipartFormDataContent();

        var pdfBytes =
            await File.ReadAllBytesAsync(Path.Combine(Directory.GetCurrentDirectory(), "Data", "example-file.pdf"));
        var pdfContent = new ByteArrayContent(pdfBytes);
        pdfContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
        multipartContent.Add(pdfContent, "Document", "example-file.pdf");

        multipartContent.Add(new StringContent("INV123"), "Invoice.InvoiceId");
        multipartContent.Add(new StringContent("S12345"), "Invoice.InvoiceNumber");
        multipartContent.Add(new StringContent(DateTime.UtcNow.ToString("o")), "Invoice.InvoiceDate");
        multipartContent.Add(new StringContent("1000.00"), "Invoice.Amount");
        multipartContent.Add(new StringContent("Test invoice"), "Invoice.Comment");

        // Act
        var response = await _client.PostAsync("/Evaluation/evaluate", multipartContent);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseContent = await response.Content.ReadAsStringAsync();
        var evaluationResponse = JsonConvert.DeserializeObject<EvaluationResponse>(responseContent);

        Assert.Multiple(() =>
        {
            Assert.That(evaluationResponse, Is.Not.Null);
            Assert.That(evaluationResponse!.InvoiceId, Is.EqualTo("INV123"));
            Assert.That(evaluationResponse.EvaluationId, Does.StartWith("EVAL_"));
            Assert.That(evaluationResponse.RulesApplied, Is.Not.Empty);
            Assert.That(evaluationResponse.EvaluationFile, Is.Not.Empty);
        });
    }
}