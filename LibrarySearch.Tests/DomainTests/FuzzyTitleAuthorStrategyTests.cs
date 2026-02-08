using LibrarySearch.Domain.Entities;
using LibrarySearch.Domain.Matching;
using Xunit;

namespace LibrarySearch.Tests.DomainTests;

public class FuzzyTitleAuthorStrategyTests
{
    private readonly FuzzyTitleAuthorStrategy _strategy;

    public FuzzyTitleAuthorStrategyTests()
    {
        _strategy = new FuzzyTitleAuthorStrategy();
    }

    [Fact]
    public void IsMatch_ShouldReturnTrue_WhenBookTitleContainsExtractedTitle()
    {
        var authors = new List<Author> { new Author("J.R.R. Tolkien", true) };
        var book = new Book("The Hobbit: An Unexpected Journey", authors, 2012, "OL1");

        var result = _strategy.IsMatch(book, "Hobbit", "J.R.R. Tolkien");

        Assert.True(result);
    }

    [Fact]
    public void IsMatch_ShouldReturnTrue_WhenExtractedTitleContainsBookTitle()
    {
        var authors = new List<Author> { new Author("Tolkien", true) };
        var book = new Book("Hobbit", authors, 1937, "OL2");

        var result = _strategy.IsMatch(book, "El Hobbit de 1937", "Tolkien");

        Assert.True(result);
    }

    [Fact]
    public void IsMatch_ShouldReturnFalse_WhenTitlesAreCompletelyDifferent()
    {
        var authors = new List<Author> { new Author("Tolkien", true) };
        var book = new Book("El Hobbit", authors, 1937, "OL1");

        var result = _strategy.IsMatch(book, "El Señor de los Anillos", "Tolkien");

        Assert.False(result);
    }

    [Fact]
    public void IsMatch_ShouldReturnFalse_WhenTitleMatchesButAuthorDoesNot()
    {
        var authors = new List<Author> { new Author("Tolkien", true) };
        var book = new Book("El Hobbit", authors, 1937, "OL1");

        var result = _strategy.IsMatch(book, "Hobbit", "George Martin");

        Assert.False(result);
    }
}