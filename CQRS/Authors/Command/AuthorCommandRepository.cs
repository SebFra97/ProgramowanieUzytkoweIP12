using Model;
using Models.DTO;
using System;
using System.Linq;

namespace CQRS
{
    public class AuthorCommandRepository : IAuthorCommandRepository
    {
        private ApplicationDbContext context;

        public AuthorCommandRepository(ApplicationDbContext context)
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


    }
}
