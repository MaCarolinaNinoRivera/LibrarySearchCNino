using LibrarySearch.Application.Interfaces;
using LibrarySearch.Application.UseCases.MatchBooks;
using LibrarySearch.Application.UseCases.ProcessRawQuery;
using LibrarySearch.Domain.Matching;
using LibrarySearch.Infrastructure.AI;
using LibrarySearch.Infrastructure.OpenLibrary;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// --------------------
// HTTP CLIENTS
// --------------------
builder.Services.AddHttpClient<IAiExtractionService, GeminiAiExtractionService>();
builder.Services.AddHttpClient<IBookSearchService, OpenLibraryBookSearchService>();

// --------------------
// USE CASES
// --------------------
builder.Services.AddScoped<ProcessRawQueryUseCase>();
builder.Services.AddScoped<MatchBooksUseCase>();

// --------------------
// STRATEGIES (SOLID)
// --------------------
builder.Services.AddScoped<IMatchingStrategy, ExactTitleMainAuthorStrategy>();
builder.Services.AddScoped<IMatchingStrategy, ExactTitleContributorStrategy>();
builder.Services.AddScoped<IMatchingStrategy, FuzzyTitleAuthorStrategy>();
builder.Services.AddScoped<IMatchingStrategy, AuthorOnlyStrategy>();

var app = builder.Build();

// --------------------
// PIPELINE
// --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// --------------------
// ROUTES
// --------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Search}/{action=Index}/{id?}");

app.Run();
