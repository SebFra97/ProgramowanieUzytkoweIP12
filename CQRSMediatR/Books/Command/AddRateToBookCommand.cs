using MediatR;
using Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Books.Command
{
    public class AddRateToBookCommand : IRequest<bool>
    {
        public int id { get; set; }
        public int rate { get; set; }

        public class AddRateToBookCommandHandler : IRequestHandler<AddRateToBookCommand, bool>
        {
            private ApplicationDbContext context;

            public AddRateToBookCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public Task<bool> Handle(AddRateToBookCommand request, CancellationToken cancellationToken)
            {
                var book = context.Books.Where(x => x.Id == request.id).FirstOrDefault();

                if (book == null) return Task.FromResult(false);

                context.BookRate.Add(new BookRate
                {
                    Type = RateType.BookRate,
                    Book = book,
                    FkBook = book.Id,
                    Date = DateTime.Now,
                    Value = (short)request.rate
                });

                context.SaveChanges();
                return Task.FromResult(true);

            }
        }
    }
}
