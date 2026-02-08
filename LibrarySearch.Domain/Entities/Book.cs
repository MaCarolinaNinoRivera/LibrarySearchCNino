using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySearch.Domain.Entities;

public class Book
{
    public string Title { get; }
    public IReadOnlyCollection<Author> Authors { get; }
    public int? FirstPublishYear { get; }
    public string OpenLibraryId { get; }

    public Book(
        string title,
        IEnumerable<Author> authors,
        int? firstPublishYear,
        string openLibraryId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Book title cannot be empty.", nameof(title));

        if (authors == null || !authors.Any())
            throw new ArgumentException("A book must have at least one author.", nameof(authors));

        Title = title;
        Authors = authors.ToList().AsReadOnly();
        FirstPublishYear = firstPublishYear;
        OpenLibraryId = openLibraryId;
    }

    public Author? GetMainAuthor()
        => Authors.FirstOrDefault(a => a.IsMainAuthor);
}
