using LibrarySearch.Domain.Entities;
using LibrarySearch.Domain.ValueObjects;

namespace LibrarySearch.Domain.Matching;

public class FuzzyTitleAuthorStrategy : IMatchingStrategy
{
    public bool IsMatch(Book book, string? extractedTitle, string? extractedAuthor)
    {
        if (string.IsNullOrWhiteSpace(extractedTitle) || string.IsNullOrWhiteSpace(extractedAuthor))
            return false;

        var normalizedBookTitle = NormalizedText.From(book.Title).Value;
        var normalizedExtractedTitle = NormalizedText.From(extractedTitle).Value;

        if (!normalizedBookTitle.Contains(normalizedExtractedTitle) &&
            !normalizedExtractedTitle.Contains(normalizedBookTitle))
            return false;

        return book.Authors.Any(a =>
            NormalizedText.From(a.Name).Value ==
            NormalizedText.From(extractedAuthor).Value);
    }

    public string GetExplanation(Book book, string? extractedTitle, string? extractedAuthor)
    {
        return $"Approximate title match; author {extractedAuthor} matches.";
    }
}
