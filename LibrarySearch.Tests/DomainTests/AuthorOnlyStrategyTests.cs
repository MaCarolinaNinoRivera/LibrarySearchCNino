using LibrarySearch.Domain.Entities;
using LibrarySearch.Domain.Matching;
using Xunit;

namespace LibrarySearch.Tests.DomainTests;

public class AuthorOnlyStrategyTests
{
    private readonly AuthorOnlyStrategy _strategy;

    public AuthorOnlyStrategyTests()
    {
        _strategy = new AuthorOnlyStrategy();
    }

    [Fact]
    public void IsMatch_ShouldReturnTrue_WhenAuthorMatchesAnyInBookList()
    {
        var authors = new List<Author>
        {
            new Author("J.R.R. Tolkien", isMainAuthor: true),
            new Author("Christopher Tolkien", isMainAuthor: false)
        };
        var book = new Book("The Silmarillion", authors, 1977, "OL2");

        var result = _strategy.IsMatch(book, null, "Christopher Tolkien");

        Assert.True(result);
    }

    [Fact]
    public void IsMatch_ShouldHandleNormalization_WhenCasingIsDifferent()
    {
        var authors = new List<Author> { new Author("Miguel de Cervantes", true) };
        var book = new Book("Don Quijote", authors, 1605, "OL3");

        var result = _strategy.IsMatch(book, "Cualquier Titulo", "miguEL DE cervanteS");

        Assert.True(result);
    }

    [Fact]
    public void IsMatch_ShouldReturnFalse_WhenAuthorDoesNotExistInBook()
    {
        var authors = new List<Author> { new Author("George Orwell", true) };
        var book = new Book("1984", authors, 1949, "OL4");

        var result = _strategy.IsMatch(book, "1984", "Aldous Huxley");

        Assert.False(result);
    }

    [Fact]
    public void IsMatch_ShouldReturnFalse_WhenExtractedAuthorIsMissing()
    {
        var book = new Book("Título", new List<Author> { new Author("Autor", true) }, 2024, "ID");

        Assert.False(_strategy.IsMatch(book, "Título", null));
        Assert.False(_strategy.IsMatch(book, "Título", "   "));
    }
}