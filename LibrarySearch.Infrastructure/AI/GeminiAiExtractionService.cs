using System.Net.Http.Json;
using LibrarySearch.Application.DTOs;
using LibrarySearch.Application.Interfaces;
using Microsoft.Extensions.Configuration;


namespace LibrarySearch.Infrastructure.AI;

public class GeminiAiExtractionService : IAiExtractionService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiAiExtractionService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"]
                  ?? throw new InvalidOperationException("Gemini API key not configured.");
    }

    public async Task<ExtractedQueryDto> ExtractAsync(string rawQuery)
    {
        var prompt = BuildPrompt(rawQuery);

        var request = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            },
            generationConfig = new
            {
                temperature = 0.1,
                topP = 0.8,
                topK = 40,
                response_mime_type = "application/json"
            }
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3-flash-preview:generateContent?key={_apiKey}",
            request);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GeminiResponse>();

        var text = result?.Candidates?.FirstOrDefault()
            ?.Content?.Parts?.FirstOrDefault()?.Text;

        if (string.IsNullOrWhiteSpace(text))
            throw new InvalidOperationException("Empty response from Gemini.");

        if (text.Contains("```json"))
        {
            text = text.Replace("```json", "").Replace("```", "").Trim();
        }

        return System.Text.Json.JsonSerializer.Deserialize<ExtractedQueryDto>(text, new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new InvalidOperationException("Failed to parse Gemini response.");
    }

    private static string BuildPrompt(string rawQuery)
    {
        return """
        You are a backend assistant specialized in structured data extraction.
        Do NOT invent data. Prefer null over guessing.
        Return valid JSON only.

        Extract book search fields from the following query:
        """ + rawQuery + """

        JSON schema:
        {
          "title": string | null,
          "author": string | null,
          "keywords": string[],
          "reasoning": string
        }
        """;
    }

    private class GeminiResponse
    {
        public List<Candidate>? Candidates { get; set; }
    }

    private class Candidate
    {
        public GeminiContent? Content { get; set; }
    }

    private class GeminiContent
    {
        public List<GeminiPart>? Parts { get; set; }
    }

    private class GeminiPart
    {
        public string? Text { get; set; }
    }
}
