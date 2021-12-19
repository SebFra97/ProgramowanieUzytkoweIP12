using Helpers;
using Model;
using Models.DTO;
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
            private Repo _repo { get; }

            public GetAllBooksQueryHandler(ApplicationDbContext context, IBookHelpers bookHelpers, Repo repo)
            {
                this.context = context;
                _bookHelpers = bookHelpers;
                _repo = repo;
            }

            List<BookDto> IQueryHandler<GetAllBooksQuery, List<BookDto>>.Handle(GetAllBooksQuery query)
            {
                List<BookDto> listOfBooks = _repo.elasticClient.Search<BookDto>(x => x.Size(query.count).Skip(query.page * query.count)
                                           .Index("books.index")).Documents.ToList();

                return listOfBooks;
            }
        }
    }
}
