using MediatR;
using Model;
using Models.DTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Books.Command
{
    public class CreateBookCommand : IRequest<Book>
    {
        public BookDto newBook { get; set; }

        public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Book>
        {
            private ApplicationDbContext context;

            public CreateBookCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken)
            {
                List<Author> bookAuthors = new List<Author>();

                foreach (var author in request.newBook.Authors)
                {
                    bookAuthors.Add(new Author
                    {
                        FirstName = author.FirstName,
                        SecondName = author.SecondName,
                    });
                }

                var book = new Book
                {
                    Title = request.newBook.Title,
                    Authors = bookAuthors,
                    ReleaseDate = request.newBook.ReleaseDate
                };

                context.Books.Add(book);
                context.SaveChanges();

                return Task.FromResult(book);
            }
        }
    }
}
