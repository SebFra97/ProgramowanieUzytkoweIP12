using Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Books.Query
{
    public class GetAllBooksQuery : IRequest<List<BookDto>>
    {
        public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookDto>>
        {
            private ApplicationDbContext context;
            private IBookHelpers _bookHelper;

            public GetAllBooksQueryHandler(ApplicationDbContext context, IBookHelpers bookHelper)
            {
                this.context = context;
                _bookHelper = bookHelper;
            }

            public Task<List<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
            {
                List<BookDto> resultList = new List<BookDto>();
                List<Book> tempBooks = context.Books.Include(x => x.Authors)
                                                    .Include(x => x.Rates)
                                                    .ToList();

                foreach (var book in tempBooks)
                {
                    var authors = _bookHelper.GetAuthorsOfBook(book);

                    resultList.Add(new BookDto
                    {
                        Id = book.Id,
                        Authors = authors.Result,
                        Title = book.Title,
                        RatesCount = _bookHelper.CountBookRates(book),
                        ReleaseDate = book.ReleaseDate,
                        AverageRate = _bookHelper.CountBookAverageRate(book.Rates),

                    });
                }

                return Task.FromResult(resultList);
            }
        }
    }
}
