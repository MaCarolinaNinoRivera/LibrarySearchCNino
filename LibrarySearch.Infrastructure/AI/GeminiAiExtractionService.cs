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
        return $"""
        You are a backend assistant specialized in structured data extraction.
        The user might provide multiple books or messy text. Your goal is to identify the SINGLE MOST PROBABLE book the user is looking for.

        STRICT RULES:
        1. Return ONLY ONE JSON object. 
        2. Do NOT return an array [].
        3. If multiple books are found (e.g., 'Twilight' and 'The Hobbit'), choose only the one that appears most complete or relevant.
        4. Do NOT include any conversational text or markdown blocks like ```json.

        Extract the fields from this query: 
        """ + rawQuery + """

        JSON Schema:
        {
            "title": "string or null",
            "author": "string or null",
            "keywords": ["string"],
            "reasoning": "Briefly explain why this book was chosen as the most probable match"
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
