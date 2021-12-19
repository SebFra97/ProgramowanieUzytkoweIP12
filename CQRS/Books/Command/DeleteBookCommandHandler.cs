using Model;
using Models.DTO;
using System.Linq;

namespace CQRS.Books.Command
{
    public class DeleteBookCommand : ICommand
    {
        public int id { get; set; }

        public class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand>
        {
            private ApplicationDbContext context;
            private Repo _repo { get; }

            public DeleteBookCommandHandler(ApplicationDbContext context, Repo repo)
            {
                this.context = context;
                _repo = repo;
            }

            public void Handle(DeleteBookCommand command)
            {
                var bookToDelete = context.Books.Where(x => x.Id == command.id).FirstOrDefault();

                context.Books.Remove(bookToDelete);
                context.SaveChanges();

                _repo.elasticClient.Delete<BookDto>(command.id);
            }
        }
    }

}
