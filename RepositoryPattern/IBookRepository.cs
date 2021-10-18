using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryPattern
{
    public interface IBookRepository : IDisposable
    {
        List<BookDto> GetAllBooks();
        BookDto GetBookById(int Id);
        void CreateNewBook(Book newBook);
        void DeleteBook(int Id);
        void AddRateToBook(int Id, int rate);
    }
}
