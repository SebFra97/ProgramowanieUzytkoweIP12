using Helpers;
using MediatR;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Books.Query
{
    public class GetBookQuery : IRequest<BookDto>
    {
        [Required]
        public int id { get; set; }

        public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookDto>
        {
            private ApplicationDbContext context;
            private IBookHelpers _bookHelper;

            public GetBookQueryHandler(ApplicationDbContext context, IBookHelpers bookHelper)
            {
                this.context = context;
                _bookHelper = bookHelper;
            }

            public Task<BookDto> Handle(GetBookQuery request, CancellationToken cancellationToken)
            {
                List<AuthorDto> listOfAutors = new List<AuthorDto>();
                var foundBook = context.Books.Where(x => x.Id == request.id).FirstOrDefault();

                if (foundBook != null)
                {
                    var authors = _bookHelper.GetAuthorsOfBook(foundBook);
                    var book = new BookDto
                    {
                        Id = request.id,
                        Title = foundBook.Title,
                        ReleaseDate = foundBook.ReleaseDate,
                        RatesCount = _bookHelper.CountBookRates(foundBook),
                        Authors = authors.Result,
                        AverageRate = _bookHelper.CountBookAverageRate(foundBook.Rates)
                    };

                    return Task.FromResult(book);
                }
                else return null;
            }
        }
    }
}
