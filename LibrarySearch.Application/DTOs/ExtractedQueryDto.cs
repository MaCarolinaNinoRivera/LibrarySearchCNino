namespace LibrarySearch.Application.DTOs;

public class ExtractedQueryDto
{
    public string? Title { get; init; }
    public string? Author { get; init; }
    public IReadOnlyCollection<string> Keywords { get; init; } = Array.Empty<string>();
    public string Reasoning { get; init; } = string.Empty;
}
