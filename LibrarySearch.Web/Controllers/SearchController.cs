using LibrarySearch.Application.UseCases.MatchBooks;
using LibrarySearch.Application.UseCases.ProcessRawQuery;
using LibrarySearch.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySearch.Web.Controllers;

public class SearchController : Controller
{
    private readonly ProcessRawQueryUseCase _processRawQuery;
    private readonly MatchBooksUseCase _matchBooks;

    public SearchController(
        ProcessRawQueryUseCase processRawQuery,
        MatchBooksUseCase matchBooks)
    {
        _processRawQuery = processRawQuery;
        _matchBooks = matchBooks;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return View(new List<BookResultViewModel>());
        }

        var extracted = await _processRawQuery.ExecuteAsync(query);

        var matches = await _matchBooks.ExecuteAsync(
            query,
            extracted.Title,
            extracted.Author);

        var result = matches.Select(m => new BookResultViewModel
        {
            Title = m.Book.Title,
            Author = m.Book.GetMainAuthor()?.Name ?? "Autor Desconocido",
            Explanation = m.Explanation,
            FirstPublishYear = m.Book.FirstPublishYear,
            OpenLibraryId = m.Book.OpenLibraryId?.Split('/').Last()
        }).ToList();

        return View(result);
    }

    [HttpGet]
    public IActionResult Architecture()
    {
        return View();
    }
}