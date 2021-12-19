using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.Books.Query
{
    public class SearchBooksQuery : IQuery
    {
        public string phrase { get; set; }

        public bool matchAll { get; set; }

        public class SearchBooksQueryHandler : IQueryHandler<SearchBooksQuery, List<BookDto>>
        {
            private Repo _repo { get; }

            public SearchBooksQueryHandler(Repo repo)
            {
                _repo = repo;
            }

            public List<BookDto> Handle(SearchBooksQuery query)
            {

                var listOfBooks = _repo.elasticClient.Search<BookDto>(s => s.Index("books.index")
                                                                               .Query(q => q.MultiMatch(qs => qs.Fields(p => p
                                                                               .Field(x => x.Title,4.0)
                                                                               .Field(x => x.Description,3.0)
                                                                               .Field(x => x.Authors.SelectMany(x => x.FirstName),1.0)
                                                                               .Field(x => x.Authors.SelectMany(x => x.SecondName), 2.0))
                                                                               .Fuzziness(Nest.Fuzziness.Auto) // PKT 3
                                                                               .Query("*" + query.phrase + "*"))));


                // PKT 2

                if (query.matchAll)
                {
                    return listOfBooks.Documents.ToList();

                } else
                {
                    return new List<BookDto> { listOfBooks.Documents.FirstOrDefault() };
                }
                                                                               
            }
        }
    }
}
