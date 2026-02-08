using LibrarySearch.Domain.Entities;

namespace LibrarySearch.Application.Interfaces;

public interface IBookSearchService
{
    Task<IReadOnlyCollection<Book>> SearchAsync(string query);
}
