using Helpers;
using Microsoft.AspNetCore.Mvc;
using Model;
using Models.DTO;
using RepositoryPattern;
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

        public ElasticSearchSeedController(Repo repo, IBookRepository bookRepository, IAuthorRepository authorRepository, ApplicationDbContext dbContext, IMockDataHelper mockDataHelper)
        {
            _repo = repo;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            this.dbContext = dbContext;
            this.mockDataHelper = mockDataHelper;
        }

        [HttpGet("/seed/data")]
        public Task<bool> SeedData()
        {
            // STEP 1 - Seeding Index into Elasticsearch

            if (!_repo.elasticClient.Indices.Exists("authors.index").Exists)
            {
                _repo.elasticClient.Indices.Create("authors_index", index => index.Map<AuthorDto>(x => x.AutoMap()));
            };

            if (!_repo.elasticClient.Indices.Exists("books.index").Exists)
            {
                _repo.elasticClient.Indices.Create("books.index", index => index.Map<BookDto>(x => x.AutoMap()));
            };

            // STEP 2 - Truncate Database

            dbContext.Books.RemoveRange(dbContext.Books);
            dbContext.Authors.RemoveRange(dbContext.Authors);
            dbContext.AuthorsRates.RemoveRange(dbContext.AuthorsRates);
            dbContext.BookRate.RemoveRange(dbContext.BookRate);
            dbContext.SaveChanges();

            // STEP 3 - Seed Book & Author Data

            List<BookDto> mockBooks = new List<BookDto>();
            List<AuthorDto> mockAuthors = new List<AuthorDto>();

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

            for (int b = 1; b < 11; b++)
            {
                List<AuthorVM> tempAuthorList = new List<AuthorVM>();
                tempAuthorList.Add(new AuthorVM { FirstName = mockAuthors[b - 1].FirstName, SecondName = mockAuthors[b - 1].SecondName, CV = mockAuthors[b - 1].CV });

                var tempBook = new BookDto
                {
                    Title = "Książka nr " + b,
                    Description = mockDataHelper.RandomString(20),
                    ReleaseDate = System.DateTime.Now,
                    Authors = tempAuthorList,
                };
                mockBooks.Add(tempBook);
            }

            foreach (var book in mockBooks)
            {
                _bookRepository.CreateNewBook(book);
            }

            return Task.FromResult(true);
        }
    }
}