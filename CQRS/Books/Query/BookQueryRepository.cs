using Helpers;
using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace CQRS
{
    public class BookQueryRepository : IBookQueryRepository
    {
        private ApplicationDbContext context;
        private IBookHelpers _bookHelpers;

        public BookQueryRepository(ApplicationDbContext context, IBookHelpers bookHelpers)
        {
            this.context = context;
            _bookHelpers = bookHelpers;
        }

        public List<BookDto> GetAllBooks()
        {
            List<BookDto> resultList = new List<BookDto>();
            List<Book> tempBooks = context.Books.Include(x => x.Authors)
                                                .Include(x => x.Rates)
                                           .ToList();

            foreach (var book in tempBooks)
            {
                var authors = _bookHelpers.GetAuthorsOfBook(book);

                resultList.Add(new BookDto
                {
                    Id = book.Id,
                    Authors = authors.Result,
                    Title = book.Title,
                    RatesCount = _bookHelpers.CountBookRates(book),
                    ReleaseDate = book.ReleaseDate,
                    AverageRate = _bookHelpers.CountBookAverageRate(book.Rates),

                });
            }

            return resultList;
        }
        public BookDto GetBookById(int Id)
        {
            List<AuthorDto> listOfAutors = new List<AuthorDto>();
            var foundBook = context.Books.Where(x => x.Id == Id).FirstOrDefault();

            if (foundBook != null)
            {
                var authors = _bookHelpers.GetAuthorsOfBook(foundBook);
                return new BookDto
                {
                    Id = Id,
                    Title = foundBook.Title,
                    ReleaseDate = foundBook.ReleaseDate,
                    RatesCount = _bookHelpers.CountBookRates(foundBook),
                    Authors = authors.Result,
                    AverageRate = _bookHelpers.CountBookAverageRate(foundBook.Rates)
                };
            }
            else return null;
        }
    }
}
