using Model;
using System.Linq;

namespace CQRS.Books.Command
{
    public class DeleteBookCommand : ICommand
    {
        public int id { get; set; }

        public class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand>
        {
            private ApplicationDbContext context;

            public DeleteBookCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public void Handle(DeleteBookCommand command)
            {
                var bookToDelete = context.Books.Where(x => x.Id == command.id).FirstOrDefault();

                context.Books.Remove(bookToDelete);
                context.SaveChanges();
            }
        }
    }

}
