using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryPattern
{
    public class BookRepository : IBookRepository
    {
        private ApplicationDbContext context;

        public void AddRateToBook(int Id, int rate)
        {
            throw new NotImplementedException();
        }

        public void CreateNewBook(Book newBook)
        {
            throw new NotImplementedException();
        }

        public void DeleteBook()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<BookDto> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public BookDto GetBookById(int id)
        {


            var foundBook = context.Books.Where(x => x.Id == id).FirstOrDefault();

            foreach (var author in foundBook.Authors)
            {

            }


            return new BookDto
            {
                Id = id,
                Title = foundBook.Title,
                ReleaseDate = foundBook.ReleaseDate,
                RatesCount = foundBook.Rates.Where(x => x.FkBook == id && x.Type == RateType.BookRate).Count(),

            };
        }
    }
}
