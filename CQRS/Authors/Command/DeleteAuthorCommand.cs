using Model;
using System.Linq;

namespace CQRS.Authors.Command
{
    public class DeleteAuthorCommand : ICommand
    {
        public int Id { get; set; }

        public class DeleteAuthorCommandHandler : ICommandHandler<DeleteAuthorCommand>
        {
            private ApplicationDbContext context;

            public DeleteAuthorCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public void Handle(DeleteAuthorCommand request)
            {
                var authorToDelete = context.Authors.Where(x => x.Id == request.Id).FirstOrDefault();

                if (!authorToDelete.Books.Any())
                {
                    context.Authors.Remove(authorToDelete);
                    context.SaveChanges();
                }
            }
        }
    }
}
