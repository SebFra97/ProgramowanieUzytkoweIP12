using Models.DTO;
using System.Collections.Generic;

namespace CQRS
{
    public interface IBookQueryRepository
    {
        List<BookDto> GetAllBooks();
        BookDto GetBookById(int Id);
    }
}
