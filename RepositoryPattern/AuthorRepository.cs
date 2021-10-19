using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public void CreateNewAuthor(AuthorDto newAuthor)
        {
            context.Authors.Add(new Author
            {
                FirstName = newAuthor.FirstName,
                SecondName = newAuthor.SecondName,
            });
            context.SaveChanges();
        }
        public bool DeleteAuthor(int id)
        {
            var authorToDelete = context.Authors.Where(x => x.Id == id).FirstOrDefault();

            if (!authorToDelete.Books.Any())
            {
                context.Authors.Remove(authorToDelete);
                context.SaveChanges();
                return true;
            }

            return false;
        }
        public List<AuthorDto> GetAllAuthors()
        {
            List<AuthorDto> resultList = new List<AuthorDto>();
            var tempAuthors = context.Authors.Include(x => x.Books)
                                           .Include(x => x.Rates)
                                           .ToList();


            foreach(var author in tempAuthors)
            {
                var books = GetBooksOfAuthor(author);

                resultList.Add(new AuthorDto
                {
                    Id = author.Id,
                    AverageRate = CountAuthorRateAverage(author.Rates),
                    RatesCount = CountAuthorRates(author),
                    Books = books.Result,
                    FirstName = author.FirstName,
                    SecondName = author.SecondName
                });
            }

            return resultList;
        }
        public void Dispose() { }

        // private methods

        private async Task<List<BookVM>> GetBooksOfAuthor(Author inputAuthor)
        {
            var books = await context.Books
                                              .Where(x => x.Authors.Contains(inputAuthor))
                                              .Select(book => new BookVM
                                              {
                                                  Id = book.Id,
                                                  Title = book.Title
                                              })
                                              .ToListAsync();

            return books;
        }
        private string CountAuthorRateAverage(List<AuthorRate> inputData)
        {
            if (inputData != null)
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
        private int CountAuthorRates(Author inputAuthor)
        {
            if (inputAuthor.Rates != null)
            {
                int count = inputAuthor.Rates.Where(x => x.FkAuthor == inputAuthor.Id && x.Type == RateType.AuthorRate).Count();
                return count;
            }
            else return 0;
            
        }
    }
}
