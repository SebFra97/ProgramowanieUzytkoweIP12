using Helpers;
using Microsoft.EntityFrameworkCore;
using Model;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryPattern
{
    public class AuthorRepository : IAuthorRepository
    {
        private ApplicationDbContext context;
        private IAuthorHelpers _authorHelpers;

        public AuthorRepository(ApplicationDbContext context, IAuthorHelpers authorHelpers)
        {
            this.context = context;
            _authorHelpers = authorHelpers;
        }

        public void AddRateToAuthor(int id, int rate)
        {
            var author = context.Authors.Where(x => x.Id == id).FirstOrDefault();

            if (author != null)
            {
                context.AuthorsRates.Add(new AuthorRate
                {
                    Type = RateType.AuthorRate,
                    Author = author,
                    FkAuthor = author.Id,
                    Date = DateTime.Now,
                    Value = (short)rate
                });

                context.SaveChanges();
            }
        }
        public void CreateNewAuthor(AuthorDto newAuthor)
        {
            context.Authors.Add(new Author
            {
                FirstName = newAuthor.FirstName,
                SecondName = newAuthor.SecondName,
            });
            context.SaveChanges();
        }
        public bool DeleteAuthor(int id)
        {
            var authorToDelete = context.Authors.Where(x => x.Id == id).FirstOrDefault();

            if (!authorToDelete.Books.Any())
            {
                context.Authors.Remove(authorToDelete);
                context.SaveChanges();
                return true;
            }

            return false;
        }
        public List<AuthorDto> GetAllAuthors(PaginationDto pagination)
        {
            List<AuthorDto> resultList = new List<AuthorDto>();
            var tempAuthors = context.Authors.Include(x => x.Books)
                                           .Include(x => x.Rates)
                                           .Skip(pagination.Page * pagination.Count)
                                           .Take(pagination.Count)
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
