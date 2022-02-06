using MediatR;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Authors.Command
{
    public class EditAuthorCommand : IRequest<bool>
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string CV { get; set; }

        public int BookId { get; set; }

        public class EditAuthorCommandHandler : IRequestHandler<EditAuthorCommand, bool>
        {
            private ApplicationDbContext context;

            public EditAuthorCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public Task<bool> Handle(EditAuthorCommand request, CancellationToken cancellationToken)
            {
                var findAuthor = context.Authors.FirstOrDefault(x => x.Id == request.AuthorId);
                

                if (findAuthor == null) return Task.FromResult(false);

                if (findAuthor.FirstName != request.Name)
                {
                    findAuthor.FirstName = request.Name;
                }

                if (findAuthor.SecondName != request.SurName)
                {
                    findAuthor.SecondName = request.SurName;
                }

                if (findAuthor.CV != request.CV)
                {
                    findAuthor.CV = request.CV;
                }

                List<Book> bookAuthor = new List<Book>();
                var findBook = context.Books.FirstOrDefault(x => x.Id == request.BookId);


                bookAuthor.Add(findBook);
                findAuthor.Books = bookAuthor;

                context.SaveChanges();

                return Task.FromResult(true);

            }
        }
    }
}
