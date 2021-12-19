using Helpers;
using Model;
using Models.DTO;
using System;
using System.Collections.Generic;
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
            private IBookHelpers _bookHelper;
            private Repo _repo { get; }

            public AddRateToBookCommandHandler(ApplicationDbContext context, IBookHelpers bookHelper, Repo repo)
            {
                this.context = context;
                _bookHelper = bookHelper;
                _repo = repo;
            }

            public void Handle(AddRateToBookCommand request)
            {
                List<BookRate> rates = new List<BookRate>();
                var authorList = new List<AuthorVM>();

                var book = context.Books.Where(x => x.Id == request.id).FirstOrDefault();

                if (book != null)
                {
                    var rate = new BookRate
                    {
                        Type = RateType.BookRate,
                        Book = book,
                        FkBook = book.Id,
                        Date = DateTime.Now,
                        Value = (short)request.rate
                    };

                   context.BookRate.Add(rate);
                   rates.Add(rate);
                   context.SaveChanges();
                    
                    // ElasticSearch

                    foreach(var author in book.Authors)
                    {
                        var tempAuthor = new AuthorVM()
                        {
                            Id = author.Id,
                            FirstName = author.FirstName,
                            SecondName = author.SecondName,
                            CV = author.CV    
                        };

                        authorList.Add(tempAuthor);
                    }

                    var bookDto = new BookDto
                    {
                        Id = book.Id,
                        Authors = authorList,
                        AverageRate = _bookHelper.CountBookAverageRate(rates),
                        Description = book.Description,
                        RatesCount = _bookHelper.CountBookRates(book),
                        ReleaseDate = book.ReleaseDate,
                        Title = book.Title

                    };

                    _repo.elasticClient.Update<BookDto>(request.id, s => s.Index("books.index").Doc(bookDto));
                }
            }
        }
    }
}
