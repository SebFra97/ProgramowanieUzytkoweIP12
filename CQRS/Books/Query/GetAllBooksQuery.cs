using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Books.Query
{
    public class GetAllBooksQuery : IQuery
    {

        public class GetAllBooksQueryHandler : IQueryHandler<GetAllBooksQuery, List<BookDto>>
        {
            private ApplicationDbContext context;

            public GetAllBooksQueryHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            List<BookDto> IQueryHandler<GetAllBooksQuery, List<BookDto>>.Handle(GetAllBooksQuery query)
            {
                List<BookDto> resultList = new List<BookDto>();
                List<Book> tempBooks = context.Books.Include(x => x.Authors)
                                                    .Include(x => x.Rates)
                                                    .ToList();

                foreach (var book in tempBooks)
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
