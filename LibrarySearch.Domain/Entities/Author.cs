namespace LibrarySearch.Domain.Entities;

public class Author
{
    public string Name { get; }
    public bool IsMainAuthor { get; }

    public Author(string name, bool isMainAuthor = true)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Author name cannot be empty.", nameof(name));

        Name = name;
        IsMainAuthor = isMainAuthor;
    }
}
