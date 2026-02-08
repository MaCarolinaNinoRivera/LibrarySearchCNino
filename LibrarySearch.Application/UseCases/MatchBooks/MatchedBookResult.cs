using LibrarySearch.Domain.Entities;

namespace LibrarySearch.Application.UseCases.MatchBooks;

public class MatchedBookResult
{
    public Book Book { get; }
    public string Explanation { get; }

    public MatchedBookResult(Book book, string explanation)
    {
        Book = book;
        Explanation = explanation;
    }
}
