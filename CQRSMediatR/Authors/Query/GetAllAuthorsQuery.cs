using Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Authors.Query
{
    public class GetAllAuthorsQuery : IRequest<List<AuthorDto>>
    {
        public int page { get; set; }
        public int count { get; set; }

        public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, List<AuthorDto>>
        {
            private ApplicationDbContext context;
            private IAuthorHelpers _authorHelper;

            public GetAllAuthorsQueryHandler(ApplicationDbContext context, IAuthorHelpers authorHelper)
            {
                this.context = context;
                _authorHelper = authorHelper;
            }

            public Task<List<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
            {
                List<AuthorDto> resultList = new List<AuthorDto>();
                var tempAuthors = context.Authors.Include(x => x.Books)
                                               .Include(x => x.Rates)
                                               .Skip(request.page * request.count)
                                               .Take(request.count)
                                               .ToList();


                foreach (var author in tempAuthors)
                {
                    var books = _authorHelper.GetBooksOfAuthor(author);

                    resultList.Add(new AuthorDto
                    {
                        Id = author.Id,
                        AverageRate = _authorHelper.CountAuthorRateAverage(author.Rates),
                        RatesCount = _authorHelper.CountAuthorRates(author),
                        Books = books.Result,
                        FirstName = author.FirstName,
                        SecondName = author.SecondName
                    });
                }

                return Task.FromResult(resultList);
            }

        }
    }
}
