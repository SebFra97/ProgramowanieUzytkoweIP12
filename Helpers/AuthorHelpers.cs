using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helpers
{
    public class AuthorHelpers : IAuthorHelpers
    {
        private ApplicationDbContext context;

        public AuthorHelpers(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<BookVM>> GetBooksOfAuthor(Author inputAuthor)
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
        public string CountAuthorRateAverage(List<AuthorRate> inputData)
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
        public int CountAuthorRates(Author inputAuthor)
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
