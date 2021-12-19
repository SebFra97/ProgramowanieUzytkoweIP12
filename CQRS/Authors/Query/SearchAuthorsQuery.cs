using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.Authors.Query
{
    public class SearchAuthorsQuery : IQuery
    {
        public string phrase { get; set; }

        public bool matchAll { get; set; }

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
                                                                               .Query(q => q.MultiMatch(qs => qs.Fields(p => p
                                                                               .Field(x => x.FirstName, 3.0)
                                                                               .Field(x => x.SecondName, 2.0)
                                                                               //.Field(x => x.Books.SelectMany(x => x.Title))
                                                                               .Field(x => x.CV, 1.0))
                                                                               .Fuzziness(Nest.Fuzziness.Auto) // PKT 3
                                                                               .Query("*" + query.phrase + "*"))));

                if (query.matchAll)
                {
                    return listOfAuthors.Documents.ToList();

                }
                else
                {
                    return new List<AuthorDto> { listOfAuthors.Documents.FirstOrDefault() };
                }
            }
        }
    }
}
