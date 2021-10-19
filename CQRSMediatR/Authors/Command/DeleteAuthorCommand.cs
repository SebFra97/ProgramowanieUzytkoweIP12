using MediatR;
using Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Authors.Command
{
    public class DeleteAuthorCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
        {
            private ApplicationDbContext context;

            public DeleteAuthorCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
            {
                var authorToDelete = context.Authors.Where(x => x.Id == request.Id).FirstOrDefault();

                if (!authorToDelete.Books.Any())
                {
                    context.Authors.Remove(authorToDelete);
                    context.SaveChanges();
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
        }
    }
}
