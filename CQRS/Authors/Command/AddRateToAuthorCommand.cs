using Model;
using System;
using System.Linq;

namespace CQRS.Authors.Command
{
    public class AddRateToAuthorCommand : ICommand
    {
        public int id { get; set; }
        public int rate { get; set; }

        public class AddRateToAuthorCommandHandler : ICommandHandler<AddRateToAuthorCommand>
        {
            private ApplicationDbContext context;

            public AddRateToAuthorCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public void Handle(AddRateToAuthorCommand request)
            {
                var author = context.Authors.Where(x => x.Id == request.id).FirstOrDefault();

                if (author != null)
                {
                    context.AuthorsRates.Add(new AuthorRate
                    {
                        Type = RateType.AuthorRate,
                        Author = author,
                        FkAuthor = author.Id,
                        Date = DateTime.Now,
                        Value = (short)request.rate
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
