using LibrarySearch.Domain.Entities;
using LibrarySearch.Domain.ValueObjects;

namespace LibrarySearch.Domain.Matching;

public class ExactTitleMainAuthorStrategy : IMatchingStrategy
{
    public bool IsMatch(Book book, string? extractedTitle, string? extractedAuthor)
    {
        if (string.IsNullOrWhiteSpace(extractedTitle) || string.IsNullOrWhiteSpace(extractedAuthor))
            return false;

        var normalizedBookTitle = NormalizedText.From(book.Title).Value;
        var normalizedExtractedTitle = NormalizedText.From(extractedTitle).Value;

        if (normalizedBookTitle != normalizedExtractedTitle)
            return false;

        var mainAuthor = book.GetMainAuthor();
        if (mainAuthor == null)
            return false;

        var normalizedBookAuthor = NormalizedText.From(mainAuthor.Name).Value;
        var normalizedExtractedAuthor = NormalizedText.From(extractedAuthor).Value;

        return normalizedBookAuthor == normalizedExtractedAuthor;
    }

    public string GetExplanation(Book book, string? extractedTitle, string? extractedAuthor)
    {
        return $"Exact title match; {book.GetMainAuthor()?.Name} is the main author.";
    }
}
