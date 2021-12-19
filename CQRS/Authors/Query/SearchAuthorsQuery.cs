using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.Authors.Query
{
    public class SearchAuthorsQuery : IQuery
    {
        public string phrase { get; set; }

        public class SearchAuthorsQueryHandler : IQueryHandler<SearchAuthorsQuery, List<AuthorDto>>
        {
            private Repo _repo { get; }
            public SearchAuthorsQueryHandler(Repo repo)
            {
                _repo = repo;
            }

            public List<AuthorDto> Handle(SearchAuthorsQuery query)
            {

                var listOfAuthors = _repo.elasticClient.Search<AuthorDto>(s => s.Index("authors.index")
                                                                               .Query(q => q.QueryString(qs => qs.Fields(p => p
                                                                               .Field(x => x.FirstName)
                                                                               .Field(x => x.SecondName)
                                                                               //.Field(x => x.Books.SelectMany(x => x.Title))
                                                                               .Field(x => x.CV))
                                                                               .Query(query.phrase))))
                                                                               .Documents.ToList();

                return listOfAuthors;
            }
        }
    }
}
