using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryPattern
{
    public class BookRepository : IBookRepository
    {
        private ApplicationDbContext context;

        public BookRepository(ApplicationDbContext context)
        {
            this.context = context;
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
        public void CreateNewBook(Book newBook)
        {
            context.Books.Add(newBook);
            context.SaveChanges();
        }
        public void DeleteBook(int Id)
        {
            var bookToDelete = context.Books.Where(x => x.Id == Id).FirstOrDefault();

            context.Books.Remove(bookToDelete);
            context.SaveChanges();
        }
        public List<BookDto> GetAllBooks()
        {
            var listofBooks = context.Books.Include(x => x.Authors)
                                           .Select(book => new BookDto
                                           {
                                               Id = book.Id,
                                               Authors = GetAuthorsOfBook(book),
                                               Title = book.Title,
                                               RatesCount = CountBookRates(book),
                                               ReleaseDate = book.ReleaseDate,
                                               AverageRate = CountBookAverageRate(book.Rates)
                                           })
                                           .ToList();

            return listofBooks;
        }
        public BookDto GetBookById(int Id)
        {
            List<AuthorDto> listOfAutors = new List<AuthorDto>();
            var foundBook = context.Books.Where(x => x.Id == Id).FirstOrDefault();

            return new BookDto
            {
                Id = Id,
                Title = foundBook.Title,
                ReleaseDate = foundBook.ReleaseDate,
                RatesCount = CountBookRates(foundBook),
                Authors = GetAuthorsOfBook(foundBook),
                AverageRate = CountBookAverageRate(foundBook.Rates)
            };
        }
        public void Dispose() { }

        // private methods 

        private List<AuthorVM> GetAuthorsOfBook(Book inputBook)
        {
            var authors = context.Authors
                                              .Where(x => x.Books.Contains(inputBook))
                                              .Select(author => new AuthorVM
                                              {
                                                  Id = author.Id,
                                                  FirstName = author.FirstName,
                                                  SecondName = author.SecondName
                                              })
                                              .ToList();

            return authors;
        }
        private string CountBookAverageRate(List<BookRate> inputData)
        {
            short rateSummary = 0;
            short rateCount = 0;

            foreach (var rec in inputData)
            {
                if (rec.Type == RateType.BookRate)
                {
                    rateSummary += rec.Value;
                    rateCount++;
                }
            }

            short rateAverage = (short)((rateSummary) / (rateCount));
            return rateAverage.ToString();
        }
        private int CountBookRates(Book inputBook)
        {
            int count = inputBook.Rates.Where(x => x.FkBook == inputBook.Id && x.Type == RateType.BookRate).Count();
            return count;
        }
    }
}
