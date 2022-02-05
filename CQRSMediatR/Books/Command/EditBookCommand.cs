using MediatR;
using Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Books.Command
{
    public class EditBookCommand : IRequest<bool>
    {
        public int BookId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }


        public class EditBookCommandHandler : IRequestHandler<EditBookCommand, bool>
        {
            private ApplicationDbContext context;

            public EditBookCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public Task<bool> Handle(EditBookCommand request, CancellationToken cancellationToken)
            {
                var findBook = context.Books.FirstOrDefault(x => x.Id == request.BookId);

                if (findBook == null) return Task.FromResult(false);

                if(findBook.Description != request.Description)
                {
                    findBook.Description = request.Description;
                }

                if (findBook.Title != request.Title)
                {
                    findBook.Title = request.Title;
                }

                if (findBook.ReleaseDate != request.ReleaseDate)
                {
                    findBook.ReleaseDate = request.ReleaseDate;
                }

                context.SaveChanges();

                return Task.FromResult(true);

            }
        }
    }
}
