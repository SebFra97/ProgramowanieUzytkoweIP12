using MediatR;
using Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Books.Command
{
    public class DeleteBookCommand : IRequest<bool>
    {
        public int id { get; set; }

        public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
        {
            private ApplicationDbContext context;

            public DeleteBookCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
            {
                var bookToDelete = context.Books.Where(x => x.Id == request.id).FirstOrDefault();

                context.Books.Remove(bookToDelete);
                context.SaveChanges();

                return Task.FromResult(true);
            }
        }
    }
}
