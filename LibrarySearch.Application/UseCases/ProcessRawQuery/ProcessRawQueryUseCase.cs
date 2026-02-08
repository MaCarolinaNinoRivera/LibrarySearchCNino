using LibrarySearch.Application.DTOs;
using LibrarySearch.Application.Interfaces;

namespace LibrarySearch.Application.UseCases.ProcessRawQuery;

public class ProcessRawQueryUseCase
{
    private readonly IAiExtractionService _aiExtractionService;

    public ProcessRawQueryUseCase(IAiExtractionService aiExtractionService)
    {
        _aiExtractionService = aiExtractionService;
    }

    public async Task<ProcessRawQueryResult> ExecuteAsync(string rawQuery)
    {
        if (string.IsNullOrWhiteSpace(rawQuery))
            throw new ArgumentException("Query cannot be empty.", nameof(rawQuery));

        var extracted = await _aiExtractionService.ExtractAsync(rawQuery);

        return new ProcessRawQueryResult(
            extracted.Title,
            extracted.Author,
            extracted.Keywords,
            extracted.Reasoning);
    }
}
