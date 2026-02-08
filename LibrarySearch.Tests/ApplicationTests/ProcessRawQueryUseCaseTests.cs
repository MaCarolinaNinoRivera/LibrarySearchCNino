using Moq;
using LibrarySearch.Application.DTOs;
using LibrarySearch.Application.Interfaces;
using LibrarySearch.Application.UseCases.ProcessRawQuery;
using Xunit;

namespace LibrarySearch.Tests.ApplicationTests;

public class ProcessRawQueryUseCaseTests
{
    private readonly Mock<IAiExtractionService> _mockAiService;
    private readonly ProcessRawQueryUseCase _useCase;

    public ProcessRawQueryUseCaseTests()
    {
        _mockAiService = new Mock<IAiExtractionService>();
        _useCase = new ProcessRawQueryUseCase(_mockAiService.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnMappedResult_WhenAiReturnsData()
    {
        var fakeAiResponse = new ExtractedQueryDto
        {
            Title = "El Hobbit",
            Author = "Tolkien",
            Keywords = new List<string> { "fantasía" },
            Reasoning = "Basado en la búsqueda del usuario."
        };

        _mockAiService.Setup(s => s.ExtractAsync(It.IsAny<string>()))
                     .ReturnsAsync(fakeAiResponse);

        var result = await _useCase.ExecuteAsync("Busca el hobbit");

        Assert.NotNull(result);
        Assert.Equal("El Hobbit", result.Title);
        Assert.Equal("Tolkien", result.Author);
        Assert.Equal("Basado en la búsqueda del usuario.", result.Reasoning);

        _mockAiService.Verify(s => s.ExtractAsync("Busca el hobbit"), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowException_WhenQueryIsEmpty()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(""));
    }
}