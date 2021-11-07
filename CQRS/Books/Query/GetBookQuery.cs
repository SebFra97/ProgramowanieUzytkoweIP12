using Helpers;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.Books.Query
{
    public class GetBookQuery : IQuery
    {
        public int id { get; set; }

        public class GetBookQueryHandler : IQueryHandler<GetBookQuery, BookDto>
        {
            private ApplicationDbContext context;
            private IBookHelpers _bookHelpers;

            public GetBookQueryHandler(ApplicationDbContext context, IBookHelpers bookHelpers)
            {
                this.context = context;
                _bookHelpers = bookHelpers;
            }

            BookDto IQueryHandler<GetBookQuery, BookDto>.Handle(GetBookQuery request)
            {
                List<AuthorDto> listOfAutors = new List<AuthorDto>();
                var foundBook = context.Books.Where(x => x.Id == request.id).FirstOrDefault();

                if (foundBook != null)
                {
                    var authors = _bookHelpers.GetAuthorsOfBook(foundBook);
                    var book = new BookDto
                    {
                        Id = request.id,
                        Title = foundBook.Title,
                        ReleaseDate = foundBook.ReleaseDate,
                        RatesCount = _bookHelpers.CountBookRates(foundBook),
                        Authors = authors.Result,
                        AverageRate = _bookHelpers.CountBookAverageRate(foundBook.Rates)
                    };

                    return book;
                }
                else return null;
            }
        }
    }
}
