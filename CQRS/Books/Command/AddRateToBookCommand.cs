using Model;
using System;
using System.Linq;

namespace CQRS.Books.Command
{
    public class AddRateToBookCommand : ICommand
    {
        public int id { get; set; }
        public int rate { get; set; }

        public class AddRateToBookCommandHandler : ICommandHandler<AddRateToBookCommand>
        {
            private ApplicationDbContext context;

            public AddRateToBookCommandHandler(ApplicationDbContext context)
            {
                this.context = context;
            }

            public void Handle(AddRateToBookCommand request)
            {
                var book = context.Books.Where(x => x.Id == request.id).FirstOrDefault();

                if (book != null)
                {
                    context.BookRate.Add(new BookRate
                    {
                        Type = RateType.BookRate,
                        Book = book,
                        FkBook = book.Id,
                        Date = DateTime.Now,
                        Value = (short)request.rate
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
