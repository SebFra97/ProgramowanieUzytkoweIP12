using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.Books.Query
{
    public class SearchBooksQuery : IQuery
    {
        public string phrase { get; set; }

        public class SearchBooksQueryHandler : IQueryHandler<SearchBooksQuery, List<BookDto>>
        {
            private Repo _repo { get; }
            public SearchBooksQueryHandler(Repo repo)
            {
                _repo = repo;
            }

            public List<BookDto> Handle(SearchBooksQuery query)
            {

                var listOfAuthors = _repo.elasticClient.Search<BookDto>(s => s.Index("books.index")
                                                                               .Query(q => q.QueryString(qs => qs.Fields(p => p
                                                                               .Field(x => x.Title)
                                                                               .Field(x => x.Description)
                                                                               .Field(x => x.Authors.SelectMany(x => x.FirstName))
                                                                               .Field(x => x.Authors.SelectMany(x => x.SecondName)))
                                                                               .Query(query.phrase))))
                                                                               .Documents.ToList();

                return listOfAuthors;
            }
        }
    }
}
