using Helpers;
using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryPattern
{
    public class BookRepository : IBookRepository
    {
        private ApplicationDbContext context;
        private IBookHelpers _bookHelpers;

        public BookRepository(ApplicationDbContext context, IBookHelpers bookHelpers)
        {
            this.context = context;
            _bookHelpers = bookHelpers;
        }

        public void AddRateToBook(int Id, int rate)
        {
            var book = context.Books.Where(x => x.Id == Id).FirstOrDefault();

            if (book != null)
            {
                context.BookRate.Add(new BookRate
                {
                    Type = RateType.BookRate,
                    Book = book,
                    FkBook = book.Id,
                    Date = DateTime.Now,
                    Value = (short)rate
                });

                context.SaveChanges();
            }
        }
        public int CreateNewBook(BookDto newBook)
        {
            List<Author> bookAuthors = new List<Author>();

            foreach (var author in newBook.Authors)
            {
                bookAuthors.Add(new Author
                {
                    FirstName = author.FirstName,
                    SecondName = author.SecondName,
                });
            }

            var addedBook = new Book
            {
                Title = newBook.Title,
                Authors = bookAuthors,
                ReleaseDate = newBook.ReleaseDate
            };


            context.Books.Add(addedBook);
            context.SaveChanges();

            return addedBook.Id;
        }
        public void DeleteBook(int Id)
        {
            var bookToDelete = context.Books.Where(x => x.Id == Id).FirstOrDefault();

            context.Books.Remove(bookToDelete);
            context.SaveChanges();
        }
        public List<BookDto> GetAllBooks(PaginationDto pagination)
        {
            List<BookDto> resultList = new List<BookDto>();
            List<Book> tempBooks = context.Books.Include(x => x.Authors)
                                                .Include(x => x.Rates)
                                                .Skip(pagination.Page * pagination.Count)
                                                .Take(pagination.Count)
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
