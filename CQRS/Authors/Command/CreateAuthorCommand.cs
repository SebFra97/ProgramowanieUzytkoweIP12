using Model;
using Models.DTO;

namespace CQRS.Authors.Command
{
    public class CreateAuthorCommand : ICommand
    {
        public AuthorDto newAuthor { get; set; }

        public class CreateNewAuthorCommandHandler : ICommandHandler<CreateAuthorCommand>
        {
            private ApplicationDbContext context;

            public CreateNewAuthorCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public void Handle(CreateAuthorCommand request)
            {
                var newAuthor = new Author
                {
                    FirstName = request.newAuthor.FirstName,
                    SecondName = request.newAuthor.SecondName,
                };

                var result = context.Authors.Add(newAuthor);
                context.SaveChanges();
            }
        }
    }
}
