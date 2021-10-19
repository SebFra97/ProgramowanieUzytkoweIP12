using MediatR;
using Model;
using Models.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Authors.Command
{
    public class CreateNewAuthorCommand : IRequest<Author>
    {
        public AuthorDto newAuthor { get; set; }

        public class CreateNewAuthorCommandHandler : IRequestHandler<CreateNewAuthorCommand, Author>
        {
            private ApplicationDbContext context;

            public CreateNewAuthorCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public Task<Author> Handle(CreateNewAuthorCommand request, CancellationToken cancellationToken)
            {
                var newAuthor = new Author
                {
                    FirstName = request.newAuthor.FirstName,
                    SecondName = request.newAuthor.SecondName,
                };

                var result = context.Authors.Add(newAuthor);
                context.SaveChanges();

                return Task.FromResult(newAuthor);
            }
        }
    }
}
