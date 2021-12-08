using Model;
using Models.DTO;
using System.Collections.Generic;

namespace CQRS.Books.Command
{
    public class AddBookCommand : ICommand
    {
        public BookDto newBook { get; set; }

        public class AddBookCommandHandler : ICommandHandler<AddBookCommand>
        {
            private ApplicationDbContext context;

            public AddBookCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public void Handle(AddBookCommand command)
            {
                List<Author> bookAuthors = new List<Author>();

                foreach (var author in command.newBook.Authors)
                {
                    bookAuthors.Add(new Author
                    {
                        FirstName = author.FirstName,
                        SecondName = author.SecondName,
                        CV = author.CV
                    });
                }

                var book = new Book
                {
                    Title = command.newBook.Title,
                    Authors = bookAuthors,
                    ReleaseDate = command.newBook.ReleaseDate
                };

                context.Books.Add(book);
                context.SaveChanges();
            }
        }
    }
}
