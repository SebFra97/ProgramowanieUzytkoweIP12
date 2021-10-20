using Models.DTO;
using System.Collections.Generic;

namespace RepositoryPattern
{
    public interface IBookRepository
    {
        List<BookDto> GetAllBooks();
        BookDto GetBookById(int Id);
        void CreateNewBook(BookDto newBook);
        void DeleteBook(int Id);
        void AddRateToBook(int Id, int rate);
    }
}
