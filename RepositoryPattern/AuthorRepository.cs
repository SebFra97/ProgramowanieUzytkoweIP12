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

        public AuthorRepository(ApplicationDbContext context)
        {
            this.context = context;
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
        public List<AuthorDto> GetAllAuthors()
        {
            List<AuthorDto> resultList = new List<AuthorDto>();
            var tempAuthors = context.Authors.Include(x => x.Books)
                                           .Include(x => x.Rates)
                                           .ToList();


            foreach (var author in tempAuthors)
            {
                var books = GetBooksOfAuthor(author);

                resultList.Add(new AuthorDto
                {
                    Id = author.Id,
                    AverageRate = CountAuthorRateAverage(author.Rates),
                    RatesCount = CountAuthorRates(author),
                    Books = books.Result,
                    FirstName = author.FirstName,
                    SecondName = author.SecondName
                });
            }

            return resultList;
        }

    }
}
