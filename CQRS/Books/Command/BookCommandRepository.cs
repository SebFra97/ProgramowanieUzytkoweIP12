using Model;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRS
{
    public class BookCommandRepository : IBookCommandRepository
    {
        private ApplicationDbContext context;

        public BookCommandRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddRateToBook(int Id, int rate)
        {
            var book = context.Books.Where(x => x.Id == Id).FirstOrDefault();

            if (book != null)
            {
                context.BookRate.Add(new BookRate
                {
                    Type = RateType.BookRate,
                    Book = book,
                    FkBook = book.Id,
                    Date = DateTime.Now,
                    Value = (short)rate
                });

                context.SaveChanges();
            }
        }
        public void CreateNewBook(BookDto newBook)
        {
            List<Author> bookAuthors = new List<Author>();

            foreach (var author in newBook.Authors)
            {
                bookAuthors.Add(new Author
                {
                    FirstName = author.FirstName,
                    SecondName = author.SecondName,
                });
            }

            context.Books.Add(new Book
            {
                Title = newBook.Title,
                Authors = bookAuthors,
                ReleaseDate = newBook.ReleaseDate
            });
            context.SaveChanges();
        }
        public void DeleteBook(int Id)
        {
            var bookToDelete = context.Books.Where(x => x.Id == Id).FirstOrDefault();

            context.Books.Remove(bookToDelete);
            context.SaveChanges();
        }
    }
}
