using LibrarySearch.Domain.Entities;
using LibrarySearch.Domain.Matching;
using Xunit;

namespace LibrarySearch.Tests.DomainTests;

public class ExactTitleMainAuthorStrategyTests
{
    private readonly ExactTitleMainAuthorStrategy _strategy;

    public ExactTitleMainAuthorStrategyTests()
    {
        _strategy = new ExactTitleMainAuthorStrategy();
    }

    [Fact]
    public void IsMatch_ShouldReturnTrue_WhenTitleAndMainAuthorMatchExactly()
    {
        var authors = new List<Author>
        {
            new Author("J.R.R. Tolkien", isMainAuthor: true),
            new Author("Jude Fisher", isMainAuthor: false)
        };
        var book = new Book("El Hobbit", authors, 1937, "OL123W");

        var result = _strategy.IsMatch(book, "El Hobbit", "J.R.R. Tolkien");

        Assert.True(result);
    }

    [Fact]
    public void IsMatch_ShouldReturnFalse_WhenTitleMatchesButAuthorIsNotMain()
    {
        var authors = new List<Author>
        {
            new Author("J.R.R. Tolkien", isMainAuthor: true),
            new Author("Jude Fisher", isMainAuthor: false)
        };
        var book = new Book("El Hobbit", authors, 1937, "OL123W");

        var result = _strategy.IsMatch(book, "El Hobbit", "Jude Fisher");

        Assert.False(result);
    }

    [Fact]
    public void IsMatch_ShouldReturnFalse_WhenExtractedDataIsNull()
    {
        var authors = new List<Author> { new Author("Tolkien", true) };
        var book = new Book("El Hobbit", authors, 1937, "OL1");

        Assert.False(_strategy.IsMatch(book, null, "Tolkien"));
        Assert.False(_strategy.IsMatch(book, "El Hobbit", null));
    }
}