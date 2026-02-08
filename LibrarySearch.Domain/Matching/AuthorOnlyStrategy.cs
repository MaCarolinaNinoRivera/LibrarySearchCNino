using LibrarySearch.Domain.Entities;
using LibrarySearch.Domain.ValueObjects;

namespace LibrarySearch.Domain.Matching;

public class AuthorOnlyStrategy : IMatchingStrategy
{
    public bool IsMatch(Book book, string? extractedTitle, string? extractedAuthor)
    {
        if (string.IsNullOrWhiteSpace(extractedAuthor))
            return false;

        return book.Authors.Any(a =>
            NormalizedText.From(a.Name).Value ==
            NormalizedText.From(extractedAuthor).Value);
    }

    public string GetExplanation(Book book, string? extractedTitle, string? extractedAuthor)
    {
        return $"Author match; returning a representative work by {extractedAuthor}.";
    }
}
