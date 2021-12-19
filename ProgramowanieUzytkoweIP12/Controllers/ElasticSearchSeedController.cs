using Helpers;
using Microsoft.AspNetCore.Mvc;
using Model;
using Models.DTO;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProgramowanieUzytkoweIP12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticSearchSeedController : Controller
    {
        private Repo _repo { get; set; }
        private IBookRepository _bookRepository { get; set; }
        private IAuthorRepository _authorRepository { get; set; }
        private ApplicationDbContext dbContext { get; set; }
        private IMockDataHelper mockDataHelper { get; set; }
        private IAuthorHelpers authorHelpers { get; set; }
        private IBookHelpers bookHelpers { get; set; }

        public ElasticSearchSeedController(Repo repo, IBookRepository bookRepository, IAuthorRepository authorRepository, ApplicationDbContext dbContext, IMockDataHelper mockDataHelper, IAuthorHelpers authorHelpers, IBookHelpers bookHelpers)
        {
            _repo = repo;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            this.dbContext = dbContext;
            this.mockDataHelper = mockDataHelper;
            this.authorHelpers = authorHelpers;
            this.bookHelpers = bookHelpers;
        }

        [HttpGet("/seed/data")]
        public Task<bool> SeedData()
        {
            try
            {
                // STEP 1 - Seeding Index into Elasticsearch

                if (_repo.elasticClient.Indices.Exists("authors.index").Exists)
                {
                    _repo.elasticClient.Indices.Delete("authors.index");                 
                };

                if (_repo.elasticClient.Indices.Exists("books.index").Exists)
                {
                    _repo.elasticClient.Indices.Delete("books.index");        
                };

                _repo.elasticClient.Indices.Create("authors.index", index => index.Map<AuthorDto>(x => x.AutoMap()));
                _repo.elasticClient.Indices.Create("books.index", index => index.Map<BookDto>(x => x.AutoMap()));

                // STEP 2 - Truncate Database

                dbContext.Books.RemoveRange(dbContext.Books);
                dbContext.Authors.RemoveRange(dbContext.Authors);
                dbContext.AuthorsRates.RemoveRange(dbContext.AuthorsRates);
                dbContext.BookRate.RemoveRange(dbContext.BookRate);
                dbContext.SaveChanges();

                // STEP 3 - Seed Book & Author Data (Index ES Included)

                List<BookDto> mockBooks = new List<BookDto>();
                List<AuthorDto> mockAuthors = new List<AuthorDto>();

                // create author , add rates, add index

                for (int a = 1; a < 11; a++)
                {
                    var tempAuthor = new AuthorDto
                    {
                        FirstName = mockDataHelper.GenerateName(),
                        SecondName = mockDataHelper.GenerateSurname(),
                        CV = mockDataHelper.RandomString(200),
                    };
                    mockAuthors.Add(tempAuthor);
                }

                foreach (var author in mockAuthors)
                {
                    var addedId = _authorRepository.CreateNewAuthor(author);
                    var rate = mockDataHelper.GenerateRandomRate(10);
                    _authorRepository.AddRateToAuthor(addedId,rate);
                    author.Id = addedId;
                    author.RatesCount = 1;
                    author.AverageRate = rate.ToString();               
                    _repo.elasticClient.Index(author, i => i.Index("authors.index"));
                }

                // create book , add rates, add index

                for (int b = 1; b < 11; b++)
                {
                    List<AuthorVM> tempAuthorList = new List<AuthorVM>();
                    tempAuthorList.Add(new AuthorVM { Id = mockAuthors[b - 1].Id, FirstName = mockAuthors[b - 1].FirstName, SecondName = mockAuthors[b - 1].SecondName, CV = mockAuthors[b - 1].CV });
                    var tempBook = new BookDto
                    {
                        Title = "Książka nr " + b,
                        Description = mockDataHelper.RandomString(20),
                        ReleaseDate = System.DateTime.Now,
                        Authors = tempAuthorList
                    };
                    mockBooks.Add(tempBook);
                }

                foreach (var book in mockBooks)
                {
                    var addedId = _bookRepository.CreateNewBook(book);
                    var rate = mockDataHelper.GenerateRandomRate(10);
                    _bookRepository.AddRateToBook(addedId, rate);
                    book.Id = addedId;
                    book.RatesCount = 1;
                    book.AverageRate = rate.ToString();
                    _repo.elasticClient.Index(book, i => i.Index("books.index"));
                }

                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
    }
}