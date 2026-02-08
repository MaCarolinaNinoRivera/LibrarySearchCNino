using LibrarySearch.Domain.ValueObjects;
using Xunit;

namespace LibrarySearch.Tests.DomainTests;

public class NormalizedTextTests
{
    [Theory]
    [InlineData("Hóbbït", "hobbit")]
    [InlineData("TOLKIEN", "tolkien")]
    [InlineData("El   Hobbit  ", "el hobbit")]
    [InlineData("¿El Hobbit?", "el hobbit")]
    [InlineData("  Cervantes, Miguel de!  ", "cervantes miguel de")]
    public void From_ShouldNormalizeTextCorrecty(string input, string expected)
    {
        var result = NormalizedText.From(input);

        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void From_ShouldReturnEmptyString_WhenInputIsNull()
    {
        var result = NormalizedText.From(null!);

        Assert.Equal(string.Empty, result.Value);
    }

    [Fact]
    public void ToString_ShouldReturnInternalValue()
    {
        var text = "Hola Mundo";
        var normalized = NormalizedText.From(text);

        Assert.Equal("hola mundo", normalized.ToString());
    }
}