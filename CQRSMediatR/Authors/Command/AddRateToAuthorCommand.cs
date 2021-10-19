using MediatR;
using Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSMediatR.Authors.Command
{
    public class AddRateToAuthorCommand : IRequest<bool>
    {
        public int id { get; set; }
        public int rate { get; set; }

        public class AddRateToAuthorCommandHandler : IRequestHandler<AddRateToAuthorCommand, bool>
        {
            private ApplicationDbContext context;

            public AddRateToAuthorCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public Task<bool> Handle(AddRateToAuthorCommand request, CancellationToken cancellationToken)
            {
                var author = context.Authors.Where(x => x.Id == request.id).FirstOrDefault();

                if (author == null) return Task.FromResult(false);

                context.AuthorsRates.Add(new AuthorRate
                {
                    Type = RateType.AuthorRate,
                    Author = author,
                    FkAuthor = author.Id,
                    Date = DateTime.Now,
                    Value = (short)request.rate
                });

                context.SaveChanges();
                return Task.FromResult(true);
            }
        }
    }
}
