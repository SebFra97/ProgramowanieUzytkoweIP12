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
            private Repo _repo { get; }

            public GetBookQueryHandler(ApplicationDbContext context, IBookHelpers bookHelpers, Repo repo)
            {
                this.context = context;
                _bookHelpers = bookHelpers;
                _repo = repo;
            }

            BookDto IQueryHandler<GetBookQuery, BookDto>.Handle(GetBookQuery request)
            {
                var listOfBooks = _repo.elasticClient.Search<BookDto>(s => s.Index("books.index")
                                                                                .Query(q => q.QueryString(qs => qs.Fields(p => p.Field(x => x.Id)).Query(request.id.ToString())))).Documents.First();

                return listOfBooks;
            }
        }
    }
}
