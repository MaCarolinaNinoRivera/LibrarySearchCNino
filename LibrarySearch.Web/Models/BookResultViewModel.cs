namespace LibrarySearch.Web.Models;

public class BookResultViewModel
{
    public string Title { get; set; } = string.Empty;
    public string? Author { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public int? FirstPublishYear { get; set; }
    public string? OpenLibraryId { get; set; }

    public string? CoverUrl => !string.IsNullOrEmpty(OpenLibraryId)
        ? $"https://covers.openlibrary.org/b/id/{OpenLibraryId}-M.jpg"
        : null;
}