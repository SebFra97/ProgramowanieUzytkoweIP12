using Model;
using Models.DTO;
using System.Collections.Generic;

namespace RepositoryPattern
{
    public interface IBookRepository
    {
        List<BookDto> GetAllBooks(PaginationDto pagination);
        BookDto GetBookById(int Id);
        int CreateNewBook(BookDto newBook);
        void DeleteBook(int Id);
        void AddRateToBook(int Id, int rate);
    }
}
