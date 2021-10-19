using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS
{
    public class AuthorQueryRepository : IAuthorQueryRepository
    {
        private ApplicationDbContext context;

        public AuthorQueryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<AuthorDto> GetAllAuthors()
        {
            List<AuthorDto> resultList = new List<AuthorDto>();
            var tempAuthors = context.Authors.Include(x => x.Books)
                                           .Include(x => x.Rates)
                                           .ToList();


            foreach (var author in tempAuthors)
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
