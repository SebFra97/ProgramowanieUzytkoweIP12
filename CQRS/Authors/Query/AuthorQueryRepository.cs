using Helpers;
using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace CQRS
{
    public class AuthorQueryRepository : IAuthorQueryRepository
    {
        private ApplicationDbContext context;
        private IAuthorHelpers _authorHelper;

        public AuthorQueryRepository(ApplicationDbContext context, IAuthorHelpers authorHelper)
        {
            this.context = context;
            _authorHelper = authorHelper;
        }

        public List<AuthorDto> GetAllAuthors()
        {
            List<AuthorDto> resultList = new List<AuthorDto>();
            var tempAuthors = context.Authors.Include(x => x.Books)
                                           .Include(x => x.Rates)
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

            return resultList;
        }
    }
}
