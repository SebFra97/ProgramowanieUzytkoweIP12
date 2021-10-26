using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Books.Query
{
    public class GetBookQuery : IQuery
    {
        public int id { get; set; }

        public class GetBookQueryHandler : IQueryHandler<GetBookQuery, BookDto>
        {
            private ApplicationDbContext context;

            public GetBookQueryHandler(ApplicationDbContext context)
            {
                this.context = context;
            }
            BookDto IQueryHandler<GetBookQuery, BookDto>.Handle(GetBookQuery request)
            {
                List<AuthorDto> listOfAutors = new List<AuthorDto>();
                var foundBook = context.Books.Where(x => x.Id == request.id).FirstOrDefault();

                if (foundBook != null)
                {
                    var authors = GetAuthorsOfBook(foundBook);
                    var book = new BookDto
                    {
                        Id = request.id,
                        Title = foundBook.Title,
                        ReleaseDate = foundBook.ReleaseDate,
                        RatesCount = CountBookRates(foundBook),
                        Authors = authors.Result,
                        AverageRate = CountBookAverageRate(foundBook.Rates)
                    };

                    return book;
                }
                else return null;
            }

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
}
