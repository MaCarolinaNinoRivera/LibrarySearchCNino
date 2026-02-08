using LibrarySearch.Application.Interfaces;
using LibrarySearch.Domain.Entities;
using LibrarySearch.Domain.Matching;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySearch.Application.UseCases.MatchBooks;

public class MatchBooksUseCase
{
    private readonly IBookSearchService _bookSearchService;
    private readonly IEnumerable<IMatchingStrategy> _matchingStrategies;

    public MatchBooksUseCase(
        IBookSearchService bookSearchService,
        IEnumerable<IMatchingStrategy> matchingStrategies)
    {
        _bookSearchService = bookSearchService;
        _matchingStrategies = matchingStrategies;
    }

    public async Task<IReadOnlyCollection<MatchedBookResult>> ExecuteAsync(
        string rawQuery,
        string? extractedTitle,
        string? extractedAuthor)
    {
        var searchTerm = !string.IsNullOrWhiteSpace(extractedTitle) ? extractedTitle : rawQuery;
        var candidates = await _bookSearchService.SearchAsync(searchTerm);

        var results = new List<MatchedBookResult>();

        foreach (var strategy in _matchingStrategies)
        {
            foreach (var book in candidates)
            {
                if (strategy.IsMatch(book, extractedTitle, extractedAuthor))
                {
                    results.Add(new MatchedBookResult(
                        book,
                        strategy.GetExplanation(book, extractedTitle, extractedAuthor)));
                }
            }
            if (results.Any())
                break;
        }
        if (!results.Any() && candidates.Any())
        {
            results.AddRange(candidates.Take(5).Select(book =>
                new MatchedBookResult(book, "Suggested match based on general search terms.")));
        }

        return results.Take(5).ToList().AsReadOnly();
    }
}