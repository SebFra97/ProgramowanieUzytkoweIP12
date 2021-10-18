using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryPattern
{
    public class AuthorRepository : IAuthorRepository
    {
        private ApplicationDbContext context;

        public AuthorRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddRateToAuthor(int id, int rate)
        {
            var author = context.Authors.Where(x => x.Id == id).FirstOrDefault();

            if (author != null)
            {
                context.AuthorsRates.Add(new AuthorRate
                {
                    Type = RateType.AuthorRate,
                    Author = author,
                    FkAuthor = author.Id,
                    Date = DateTime.Now,
                    Value = (short)rate
                });

                context.SaveChanges();
            }
        }
        public void CreateNewAuthor(Author author)
        {
            context.Authors.Add(author);
            context.SaveChanges();
        }
        public void DeleteAuthor(int id)
        {
            var authorToDelete = context.Authors.Where(x => x.Id == id).FirstOrDefault();
            context.Authors.Remove(authorToDelete);
            context.SaveChanges();
        }
        public List<AuthorDto> GetAllAuthors()
        {
            var listofAuthors = context.Authors.Include(x => x.Books)
                                           .Select(author => new AuthorDto
                                           {
                                               Id = author.Id,
                                               FirstName = author.FirstName,
                                               SecondName = author.SecondName,
                                               Books = GetBooksOfAuthor(author),
                                               AverageRate = CountAuthorRateAverage(author.Rates),
                                               RatesCount = CountAuthorRates(author)
                                           })
                                           .ToList();

            return listofAuthors;
        }
        public void Dispose() { }

        // private methods

        private List<BookVM> GetBooksOfAuthor(Author inputAuthor)
        {
            var books = context.Books
                                              .Where(x => x.Authors.Contains(inputAuthor))
                                              .Select(book => new BookVM
                                              {
                                                  Id = book.Id,
                                                  Title = book.Title
                                              })
                                              .ToList();

            return books;
        }
        private string CountAuthorRateAverage(List<AuthorRate> inputData)
        {
            short rateSummary = 0;
            short rateCount = 0;

            foreach (var rec in inputData)
            {
                if (rec.Type == RateType.AuthorRate)
                {
                    rateSummary += rec.Value;
                    rateCount++;
                }
            }

            short rateAverage = (short)((rateSummary) / (rateCount));

            return rateAverage.ToString();
        }
        private int CountAuthorRates(Author inputAuthor)
        {
            int count = inputAuthor.Rates.Where(x => x.FkAuthor == inputAuthor.Id && x.Type == RateType.AuthorRate).Count();
            return count;
        }
    }
}
