using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.Authors.Query
{
    public class GetAllAuthorsQuery : IQuery
    {
        public int page { get; set; }
        public int count { get; set; }

        public class GetAllAuthorsQueryHandler : IQueryHandler<GetAllAuthorsQuery, List<AuthorDto>>
        {
            private Repo _repo { get; }
            public GetAllAuthorsQueryHandler(Repo repo)
            {
                _repo = repo;
            }

            public List<AuthorDto> Handle(GetAllAuthorsQuery query)
            {
                List<AuthorDto> listOfAuthors = _repo.elasticClient.Search<AuthorDto>(x => x.Size(query.count).Skip(query.page * query.count)
                                           .Index("authors.index")).Documents.ToList();

                return listOfAuthors;
            }
        }
    }
}
