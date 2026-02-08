using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LibrarySearch.Domain.ValueObjects;

public sealed class NormalizedText
{
    public string Value { get; }

    private NormalizedText(string value)
    {
        Value = value;
    }

    public static NormalizedText From(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new NormalizedText(string.Empty);

        var normalized = input.ToLowerInvariant();

        normalized = RemoveDiacritics(normalized);
        normalized = RemovePunctuation(normalized);
        normalized = NormalizeWhitespace(normalized);

        return new NormalizedText(normalized);
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    private static string RemovePunctuation(string text)
        => Regex.Replace(text, @"[^\w\s]", string.Empty);

    private static string NormalizeWhitespace(string text)
        => Regex.Replace(text, @"\s+", " ").Trim();

    public override string ToString() => Value;
}
