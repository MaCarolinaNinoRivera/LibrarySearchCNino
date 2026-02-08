using LibrarySearch.Domain.Entities;

namespace LibrarySearch.Application.Interfaces;

public interface IBookRepository
{
    Task SaveSearchAsync(string rawQuery, IReadOnlyCollection<Book> results);
}
