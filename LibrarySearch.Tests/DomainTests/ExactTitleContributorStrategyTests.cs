using LibrarySearch.Domain.Entities;
using LibrarySearch.Domain.Matching;
using Xunit;

namespace LibrarySearch.Tests.DomainTests;

public class ExactTitleContributorStrategyTests
{
    private readonly ExactTitleContributorStrategy _strategy;

    public ExactTitleContributorStrategyTests()
    {
        _strategy = new ExactTitleContributorStrategy();
    }

    [Fact]
    public void IsMatch_ShouldReturnTrue_WhenTitleIsExactAndAuthorIsContributor()
    {
        var authors = new List<Author>
        {
            new Author("J.R.R. Tolkien", isMainAuthor: true),
            new Author("David Wenzel", isMainAuthor: false)
        };
        var book = new Book("The Hobbit", authors, 1989, "OL1");

        var result = _strategy.IsMatch(book, "The Hobbit", "David Wenzel");

        Assert.True(result);
    }

    [Fact]
    public void IsMatch_ShouldReturnFalse_WhenAuthorIsMainAuthor()
    {
        var authors = new List<Author>
        {
            new Author("J.R.R. Tolkien", isMainAuthor: true),
            new Author("David Wenzel", isMainAuthor: false)
        };
        var book = new Book("The Hobbit", authors, 1989, "OL1");

        var result = _strategy.IsMatch(book, "The Hobbit", "J.R.R. Tolkien");

        Assert.False(result);
    }

    [Fact]
    public void IsMatch_ShouldReturnFalse_WhenTitleDoesNotMatch()
    {
        var authors = new List<Author> { new Author("Ilustrador", isMainAuthor: false) };
        var book = new Book("El Hobbit", authors, 1937, "OL1");

        var result = _strategy.IsMatch(book, "Un Titulo Diferente", "Ilustrador");

        Assert.False(result);
    }

    [Fact]
    public void GetExplanation_ShouldReturnCorrectFormat()
    {
        var authors = new List<Author> { new Author("Ilustrador", false) };
        var book = new Book("Titulo", authors, 2020, "ID");

        var explanation = _strategy.GetExplanation(book, "Titulo", "Ilustrador");

        Assert.Contains("listed as a contributor", explanation);
        Assert.Contains("Ilustrador", explanation);
    }
}