using Helpers;
using Model;
using Models.DTO;
using System;
using System.Collections.Generic;
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
            private IAuthorHelpers _authorHelper;
            private Repo _repo { get; }

            public AddRateToAuthorCommandHandler(ApplicationDbContext context, IAuthorHelpers authorHelper, Repo repo)
            {
                this.context = context;
                _authorHelper = authorHelper;
                _repo = repo;
            }

            public void Handle(AddRateToAuthorCommand request)
            {
                List<AuthorRate> rates = new List<AuthorRate>();
                var bookList = new List<BookVM>();

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


                    // ElasticSearch

                    foreach (var book in author.Books)
                    {
                        var tempBook = new BookVM()
                        {
                            Id = book.Id,
                            Title = book.Title
                        };

                        bookList.Add(tempBook);
                    }


                    var authorDto = new AuthorDto
                    {
                        Id = author.Id,
                        FirstName = author.FirstName,
                        SecondName = author.SecondName,
                        Books = bookList,
                        CV = author.CV,
                        AverageRate = _authorHelper.CountAuthorRateAverage(rates),
                        RatesCount = _authorHelper.CountAuthorRates(author)
                    };

                    _repo.elasticClient.Update<AuthorDto>(request.id, s => s.Index("authors.index").Doc(authorDto));

                }
            }
        }
    }
}
