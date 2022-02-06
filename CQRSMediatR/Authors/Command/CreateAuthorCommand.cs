using MediatR;
using Model;
using Models.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Authors.Command
{
    public class CreateAuthorCommand : IRequest<Author>
    {
        public AuthorDto newAuthor { get; set; }

        public class CreateNewAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Author>
        {
            private ApplicationDbContext context;

            public CreateNewAuthorCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public Task<Author> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
            {
                var newAuthor = new Author
                {
                    FirstName = request.newAuthor.FirstName,
                    SecondName = request.newAuthor.SecondName,
                    CV = request.newAuthor.CV,
                    
                };

                var result = context.Authors.Add(newAuthor);
                context.SaveChanges();

                return Task.FromResult(newAuthor);
            }
        }
    }
}
