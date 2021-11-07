using Helpers;
using Microsoft.EntityFrameworkCore;
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
            private ApplicationDbContext context;
            private IAuthorHelpers _authorHelpers;

            public GetAllAuthorsQueryHandler(ApplicationDbContext context, IAuthorHelpers authorHelpers)
            {
                this.context = context;
                _authorHelpers = authorHelpers;
            }

            public List<AuthorDto> Handle(GetAllAuthorsQuery query)
            {
                List<AuthorDto> resultList = new List<AuthorDto>();
                var tempAuthors = context.Authors.Include(x => x.Books)
                                               .Include(x => x.Rates)
                                               .Skip(query.page * query.count)
                                               .Take(query.count)
                                               .ToList();


                foreach (var author in tempAuthors)
                {
                    var books = _authorHelpers.GetBooksOfAuthor(author);

                    resultList.Add(new AuthorDto
                    {
                        Id = author.Id,
                        AverageRate = _authorHelpers.CountAuthorRateAverage(author.Rates),
                        RatesCount = _authorHelpers.CountAuthorRates(author),
                        Books = books.Result,
                        FirstName = author.FirstName,
                        SecondName = author.SecondName
                    });
                }

                return resultList;
            }

        }
    }
}
