using Helpers;
using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.Books.Query
{
    public class GetAllBooksQuery : IQuery
    {
        public int page { get; set; }
        public int count { get; set; }

        public class GetAllBooksQueryHandler : IQueryHandler<GetAllBooksQuery, List<BookDto>>
        {
            private ApplicationDbContext context;
            private IBookHelpers _bookHelpers;
            private readonly IElasticClient _elasticClient;

            public GetAllBooksQueryHandler(ApplicationDbContext context, IBookHelpers bookHelpers, IElasticClient elasticClient)
            {
                this.context = context;
                _bookHelpers = bookHelpers;
                _elasticClient = elasticClient;
            }

            List<BookDto> IQueryHandler<GetAllBooksQuery, List<BookDto>>.Handle(GetAllBooksQuery query)
            {
                List<BookDto> resultList = new List<BookDto>();
                List<Book> tempBooks = context.Books.Include(x => x.Authors)
                                                    .Include(x => x.Rates)
                                                    .Skip(query.page * query.count)
                                                    .Take(query.count)
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

                foreach(var result in resultList)
                {
                    IndexResponse res = _elasticClient.IndexDocument<BookDto>(result);
                }

                return resultList;
            }
        }
    }
}
