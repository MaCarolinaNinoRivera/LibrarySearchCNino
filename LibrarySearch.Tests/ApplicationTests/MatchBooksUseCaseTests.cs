using Moq;
using LibrarySearch.Application.Interfaces;
using LibrarySearch.Application.UseCases.MatchBooks;
using LibrarySearch.Domain.Entities;
using LibrarySearch.Domain.Matching;
using Xunit;

namespace LibrarySearch.Tests.ApplicationTests;

public class MatchBooksUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldRespectHierarchy_AndStopAtFirstMatchingStrategy()
    {
        var mockSearchService = new Mock<IBookSearchService>();
        var book = new Book("The Hobbit", new List<Author> { new Author("Tolkien") }, 1937, "OL1");

        mockSearchService.Setup(s => s.SearchAsync(It.IsAny<string>()))
                         .ReturnsAsync(new List<Book> { book });

        var firstStrategy = new Mock<IMatchingStrategy>();
        var secondStrategy = new Mock<IMatchingStrategy>();

        firstStrategy.Setup(s => s.IsMatch(It.IsAny<Book>(), It.IsAny<string>(), It.IsAny<string>()))
                     .Returns(true);
        firstStrategy.Setup(s => s.GetExplanation(It.IsAny<Book>(), It.IsAny<string>(), It.IsAny<string>()))
                     .Returns("First Strategy Win");

        var strategies = new List<IMatchingStrategy> { firstStrategy.Object, secondStrategy.Object };

        var useCase = new MatchBooksUseCase(mockSearchService.Object, strategies);

        var results = await useCase.ExecuteAsync("query", "The Hobbit", "Tolkien");

        Assert.Single(results);
        Assert.Equal("First Strategy Win", results.First().Explanation);
        
        secondStrategy.Verify(s => s.IsMatch(It.IsAny<Book>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}