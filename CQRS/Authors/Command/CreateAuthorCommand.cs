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
            private Repo _repo { get;  }

            public CreateNewAuthorCommandHandler(ApplicationDbContext context, Repo repo)
            {
                this.context = context;
                _repo = repo;
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

                _repo.elasticClient.Index(newAuthor, i => i.Index("authors.index"));
            }
        }
    }
}
