using CQRS;
using Microsoft.AspNetCore.Mvc;
using Model;
using Models.DTO;
using System;
using System.Collections.Generic;

namespace ProgramowanieUzytkoweIP12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CQRSController : ControllerBase
    {
        private IAuthorQueryRepository _authorQueryRepository;
        private IAuthorCommandRepository _authorCommandRepository;

        private IBookQueryRepository _bookQueryRepository;
        private IBookCommandRepository _bookCommandRepository;

        public CQRSController(IAuthorQueryRepository authorQueryRepository, IAuthorCommandRepository authorCommandRepository, IBookQueryRepository bookQueryRepository, IBookCommandRepository bookCommandRepository)
        {
            _authorQueryRepository = authorQueryRepository;
            _authorCommandRepository = authorCommandRepository;
            _bookQueryRepository = bookQueryRepository;
            _bookCommandRepository = bookCommandRepository;
        }


        #region BOOK ENDPOINTS

        [HttpGet("/book/cqrs/get")]
        public List<BookDto> GetBooks()
        {
            return _bookQueryRepository.GetAllBooks();
        }

        [HttpGet("/book/cqrs/get/{id}")]
        public ActionResult<BookDto> GetBook([FromRoute] int id)
        {
            var book = _bookQueryRepository.GetBookById(id);

            if (book != null) return Ok(book);
            else return NotFound();
        }

        [HttpPost("/book/cqrs/create")]
        public ActionResult<Book> CreateBook(BookDto book)
        {
            try
            {
                _bookCommandRepository.CreateNewBook(book);
                return Ok(book);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/book/cqrs/add/rate/{id}")]
        public ActionResult AddRateToBook([FromRoute] int id, int rate)
        {
            try
            {
                _bookCommandRepository.AddRateToBook(id, rate);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("/book/cqrs/delete/{id}")]
        public ActionResult DeleteBook([FromRoute] int id)
        {
            try
            {
                _bookCommandRepository.DeleteBook(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion
        #region AUTHOR ENDPOINTS

        [HttpGet("/authors/cqrs/get")]
        public List<AuthorDto> GetAuthor()
        {
            return _authorQueryRepository.GetAllAuthors();
        }

        [HttpPost("/author/cqrs/create")]
        public ActionResult<Author> CreateAuthor(AuthorDto author)
        {
            try
            {
                _authorCommandRepository.CreateNewAuthor(author);
                return Ok(author);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("/author/cqrs/add/rate/{id}")]
        public ActionResult AddRateToAuthor([FromRoute] int id, int rate)
        {
            try
            {
                _authorCommandRepository.AddRateToAuthor(id, rate);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("/author/cqrs/delete/{id}")]
        public ActionResult DeleteAuthor([FromRoute] int id)
        {
            try
            {
                bool result = _authorCommandRepository.DeleteAuthor(id);

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
