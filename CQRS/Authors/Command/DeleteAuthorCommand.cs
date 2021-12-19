using Model;
using Models.DTO;
using System.Linq;

namespace CQRS.Authors.Command
{
    public class DeleteAuthorCommand : ICommand
    {
        public int Id { get; set; }

        public class DeleteAuthorCommandHandler : ICommandHandler<DeleteAuthorCommand>
        {
            private ApplicationDbContext context;
            private Repo _repo { get; }

            public DeleteAuthorCommandHandler(ApplicationDbContext context, Repo repo)
            {
                this.context = context;
                _repo = repo;
            }

            public void Handle(DeleteAuthorCommand request)
            {
                var authorToDelete = context.Authors.Where(x => x.Id == request.Id).FirstOrDefault();

                if (!authorToDelete.Books.Any())
                {
                    context.Authors.Remove(authorToDelete);
                    context.SaveChanges();
                }

                _repo.elasticClient.Delete<AuthorDto>(request.Id);
            }
        }
    }
}
