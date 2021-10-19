using Models.DTO;

namespace CQRS
{
    public interface IBookCommandRepository
    {
        void CreateNewBook(BookDto newBook);
        void DeleteBook(int Id);
        void AddRateToBook(int Id, int rate);
    }
}
