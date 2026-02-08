using LibrarySearch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySearch.Domain.Matching
{
    public interface IMatchingStrategy
    {
        bool IsMatch(Book book, string? title, string? author);
        string GetExplanation(Book book, string? title, string? author);
    }
}


