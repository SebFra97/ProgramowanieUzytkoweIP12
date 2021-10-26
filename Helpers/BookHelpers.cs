using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helpers
{
    public class BookHelpers : IBookHelpers
    {
        private ApplicationDbContext context;

        public BookHelpers(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<AuthorVM>> GetAuthorsOfBook(Book inputBook)
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
        public string CountBookAverageRate(List<BookRate> inputData)
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
        public int CountBookRates(Book inputBook)
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
