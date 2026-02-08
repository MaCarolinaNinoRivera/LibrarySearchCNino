using LibrarySearch.Application.DTOs;

namespace LibrarySearch.Application.Interfaces;

public interface IAiExtractionService
{
    Task<ExtractedQueryDto> ExtractAsync(string rawQuery);
}
