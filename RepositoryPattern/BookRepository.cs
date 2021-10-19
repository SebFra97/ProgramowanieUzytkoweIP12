using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public void CreateNewBook(BookDto newBook)
        {
            List<Author> bookAuthors = new List<Author>();

            foreach(var author in newBook.Authors)
            {
                bookAuthors.Add(new Author
                {
                    FirstName = author.FirstName,
                    SecondName = author.SecondName,
                });
            }

            context.Books.Add(new Book
            {
                Title = newBook.Title,
                Authors = bookAuthors,
                ReleaseDate = newBook.ReleaseDate
            });
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
            List<BookDto> resultList = new List<BookDto>();
            List<Book> tempBooks = context.Books.Include(x => x.Authors)
                                                .Include(x => x.Rates)
                                           .ToList();

            foreach(var book in tempBooks)
            {
                var authors = GetAuthorsOfBook(book);

                resultList.Add(new BookDto
                {
                    Id = book.Id,
                    Authors = authors.Result,
                    Title = book.Title,
                    RatesCount = CountBookRates(book),
                    ReleaseDate = book.ReleaseDate,
                    AverageRate = CountBookAverageRate(book.Rates),
                    
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
                var authors = GetAuthorsOfBook(foundBook);
                return new BookDto
                {
                    Id = Id,
                    Title = foundBook.Title,
                    ReleaseDate = foundBook.ReleaseDate,
                    RatesCount = CountBookRates(foundBook),
                    Authors = authors.Result,
                    AverageRate = CountBookAverageRate(foundBook.Rates)
                };
            }
            else return null;
        }
        public void Dispose() { }

        // private methods 

        private async Task<List<AuthorVM>> GetAuthorsOfBook(Book inputBook)
        {
            var authors = await context.Authors
                                              .Where(x => x.Books.Contains(inputBook))
                                              .Select(author => new AuthorVM
                                              {
                                                  Id = author.Id,
                                                  FirstName = author.FirstName,
                                                  SecondName = author.SecondName
                                              })
                                              .ToListAsync();

            return authors;
        }
        private static string CountBookAverageRate(List<BookRate> inputData)
        {
            if (inputData != null)
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
                short rateAverage;
                if (rateCount > 0 && rateSummary > 0)
                {
                    rateAverage = (short)((rateSummary) / (rateCount));
                }
                else rateAverage = 0;

                return rateAverage.ToString();
            }
            else return "";
            
        }
        private static int CountBookRates(Book inputBook)
        {
            if (inputBook.Rates != null)
            {
                int count = inputBook.Rates.Where(x => x.FkBook == inputBook.Id && x.Type == RateType.BookRate).Count();
                return count;
            }
            else return 0;
            
        }
    }
}
