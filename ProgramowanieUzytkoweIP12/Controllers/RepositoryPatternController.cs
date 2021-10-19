using Microsoft.AspNetCore.Mvc;
using Model;
using Models.DTO;
using RepositoryPattern;
using System;
using System.Collections.Generic;

namespace ProgramowanieUzytkoweIP12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoryPatternController : ControllerBase
    {
        private IBookRepository _bookRepository;
        private IAuthorRepository _authorRepository;

        public RepositoryPatternController(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        #region BOOK ENDPOINTS

        [HttpGet("/book/get")]
        public List<BookDto> GetBooks()
        {
            return _bookRepository.GetAllBooks();
        }

        [HttpGet("/book/get/{id}")]
        public ActionResult<BookDto> GetBook([FromRoute] int id)
        {
            var book = _bookRepository.GetBookById(id);

            if (book != null) return Ok(book);
            else return NotFound();
        }

        [HttpPost("/book/create")]
        public ActionResult<Book> CreateBook(BookDto book)
        {
            try
            {
                _bookRepository.CreateNewBook(book);
                return Ok(book);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/book/add/rate/{id}")]
        public ActionResult AddRateToBook([FromRoute] int id, int rate)
        {
            try
            {
                _bookRepository.AddRateToBook(id, rate);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("/book/delete/{id}")]
        public ActionResult DeleteBook([FromRoute] int id)
        {
            try
            {
                _bookRepository.DeleteBook(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion
        #region AUTHOR ENDPOINTS

        [HttpGet("/authors/get")]
        public List<AuthorDto> GetAuthor()
        {
            return _authorRepository.GetAllAuthors();
        }

        [HttpPost("/author/create")]
        public ActionResult<Author> CreateAuthor(AuthorDto author)
        {
            try
            {
                _authorRepository.CreateNewAuthor(author);
                return Ok(author);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/author/add/rate/{id}")]
        public ActionResult AddRateToAuthor([FromRoute] int id, int rate)
        {
            try
            {
                _authorRepository.AddRateToAuthor(id, rate);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("/author/delete/{id}")]
        public ActionResult DeleteAuthor([FromRoute] int id)
        {
            try
            {
                bool result = _authorRepository.DeleteAuthor(id);

                return (result ? Ok() : BadRequest());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion

    }
}
