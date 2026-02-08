namespace LibrarySearch.Application.UseCases.ProcessRawQuery;

public class ProcessRawQueryResult
{
    public string? Title { get; }
    public string? Author { get; }
    public IReadOnlyCollection<string> Keywords { get; }
    public string Reasoning { get; }

    public ProcessRawQueryResult(
        string? title,
        string? author,
        IReadOnlyCollection<string> keywords,
        string reasoning)
    {
        Title = title;
        Author = author;
        Keywords = keywords;
        Reasoning = reasoning;
    }
}
