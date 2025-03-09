using System.Net;
using AutoFixture;
using Moq;
using WebApi.Models;
using WebApi.Services;
using WebApi.Services.Abstractions;
using Microsoft.AspNetCore.Http;

namespace WebApi.Tests.Services;

[TestFixture]
public class EvaluationServiceUnitTests
{
    private readonly Mock<IClassificationHttpClient> _classificationHttpClientMock = new();
    private EvaluationService _evaluationService;
    private static readonly string[] RulesApplied = ["Approved"];
    private readonly Fixture _fixture;

    public EvaluationServiceUnitTests()
    {
        _fixture = new Fixture();
        ConfigureAutoFixture();
    }

    private void ConfigureAutoFixture()
    {
        // Mock IFormFile
        var fileMock = new Mock<IFormFile>();
        const string content = "Testing File";
        const string fileName = "test.pdf";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        fileMock.Setup(x => x.OpenReadStream()).Returns(ms);
        fileMock.Setup(x => x.FileName).Returns(fileName);
        fileMock.Setup(x => x.Length).Returns(ms.Length);

        // Register the IFormFile creation with AutoFixture
        _fixture.Register(() => fileMock.Object);
    }

    [SetUp]
    public void SetUp()
    {
        _evaluationService = new EvaluationService(_classificationHttpClientMock.Object);
    }

    [Test]
    public async Task EvaluateAsync_WhenApiCallSucceeds_ShouldReturnEvaluationResponse()
    {
        // Arrange
        var request = _fixture.Build<EvaluationRequest>()
            .Create();
        var classificationResponse = _fixture.Create<ClassificationResponse>();

        _classificationHttpClientMock.Setup(x => x.ClassifyAsync(request))
            .ReturnsAsync(classificationResponse);

        // Act
        var result = await _evaluationService.EvaluateAsync(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.EvaluationId, Is.Not.Null);
            Assert.That(result.InvoiceId, Is.EqualTo(request.Invoice!.InvoiceId));
            Assert.That(result.RulesApplied, Is.EquivalentTo(RulesApplied));
            Assert.That(result.Classification, Is.EqualTo(classificationResponse.Classification));
        });
        _classificationHttpClientMock.Verify(x => x.ClassifyAsync(request), Times.Once);
    }

    [Test]
    public void EvaluateAsync_WhenApiCallFails_ShouldThrowHttpRequestException()
    {
        // Arrange
        var request = _fixture.Build<EvaluationRequest>()
            .Create();
        var exception = new HttpRequestException("Failed to classify", null, HttpStatusCode.InternalServerError);

        _classificationHttpClientMock.Setup(x => x.ClassifyAsync(request))
            .ThrowsAsync(exception);

        // Act
        var exceptionThrown = Assert.ThrowsAsync<HttpRequestException>(() => _evaluationService.EvaluateAsync(request));

        // Assert
        Assert.That(exceptionThrown, Is.Not.Null);
        Assert.That(exceptionThrown.Message, Is.EqualTo(exception.Message));
        _classificationHttpClientMock.Verify(x => x.ClassifyAsync(request), Times.Once);
    }
}