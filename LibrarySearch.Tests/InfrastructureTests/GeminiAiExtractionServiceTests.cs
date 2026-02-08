using System.Net;
using System.Text.Json;
using LibrarySearch.Infrastructure.AI;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Xunit;

namespace LibrarySearch.Tests.InfrastructureTests;

public class GeminiAiExtractionServiceTests
{
    private readonly Mock<IConfiguration> _mockConfig;

    public GeminiAiExtractionServiceTests()
    {
        _mockConfig = new Mock<IConfiguration>();
        _mockConfig.Setup(c => c["Gemini:ApiKey"]).Returns("fake-api-key");
    }

    [Fact]
    public async Task ExtractAsync_ShouldCleanMarkdownJson_AndReturnDto()
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        var geminiRawResponse = new
        {
            candidates = new[] {
                new {
                    content = new {
                        parts = new[] {
                            new { text = "```json\n { \"title\": \"El Hobbit\", \"author\": \"Tolkien\", \"keywords\": [], \"reasoning\": \"test\" } \n```" }
                        }
                    }
                }
            }
        };

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(geminiRawResponse))
        };

        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(handlerMock.Object);
        var service = new GeminiAiExtractionService(httpClient, _mockConfig.Object);

        var result = await service.ExtractAsync("busqueda");

        Assert.Equal("El Hobbit", result.Title);
        Assert.Equal("Tolkien", result.Author);
    }
}