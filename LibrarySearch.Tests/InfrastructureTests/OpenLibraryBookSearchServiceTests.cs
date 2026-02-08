using System.Net;
using System.Text.Json;
using LibrarySearch.Infrastructure.OpenLibrary;
using Moq;
using Moq.Protected;
using Xunit;

namespace LibrarySearch.Tests.InfrastructureTests;

public class OpenLibraryBookSearchServiceTests
{
    [Fact]
    public async Task SearchAsync_ShouldMapJsonToBookEntities_Correcty()
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        var fakeApiResponse = new
        {
            docs = new[]
            {
                new {
                    title = "The Hobbit",
                    author_name = new[] { "J.R.R. Tolkien", "George Allen" },
                    first_publish_year = 1937,
                    key = "/works/OL27448W"
                }
            }
        };

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(fakeApiResponse))
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
        var service = new OpenLibraryBookSearchService(httpClient);

        var result = await service.SearchAsync("hobbit");

        Assert.NotEmpty(result);
        var book = result.First();

        Assert.Equal("The Hobbit", book.Title);

        Assert.Equal(2, book.Authors.Count);
        Assert.True(book.Authors.First(a => a.Name == "J.R.R. Tolkien").IsMainAuthor);
        Assert.False(book.Authors.First(a => a.Name == "George Allen").IsMainAuthor);
    }
}