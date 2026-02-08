using LibrarySearch.Application.Interfaces;
using LibrarySearch.Domain.Entities;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace LibrarySearch.Infrastructure.OpenLibrary;

public class OpenLibraryBookSearchService : IBookSearchService
{
    private readonly HttpClient _httpClient;

    public OpenLibraryBookSearchService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<Book>> SearchAsync(string query)
    {
        var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}";

        var response = await _httpClient.GetFromJsonAsync<OpenLibraryResponse>(url);

        if (response?.Docs == null)
            return Array.Empty<Book>();

        return response.Docs
            .Where(d => !string.IsNullOrWhiteSpace(d.Title) && d.AuthorName?.Any() == true)
            .Take(20)
            .Select(d =>
            {
                var authors = d.AuthorName!
                    .Select((name, index) =>
                        new Author(
                            name: name,
                            isMainAuthor: index == 0))
                    .ToList();

                return new Book(
                    d.Title!,
                    authors,
                    d.FirstPublishYear,
                    d.Key ?? string.Empty
                );
            })
            .ToList();
    }


    private class OpenLibraryResponse
    {
        public List<OpenLibraryDoc>? Docs { get; set; }
    }

    private class OpenLibraryDoc
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("author_name")]
        public List<string>? AuthorName { get; set; }

        [JsonPropertyName("first_publish_year")]
        public int? FirstPublishYear { get; set; }

        [JsonPropertyName("key")]
        public string? Key { get; set; }
    }

}
