using CQRSMediatR.Authors.Command;
using CQRSMediatR.Authors.Query;
using CQRSMediatR.Books.Command;
using CQRSMediatR.Books.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Model;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProgramowanieUzytkoweIP12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CQRSMediatRController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CQRSMediatRController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region BOOKS REGION

        [HttpGet("/book/mediator/get")]
        public ActionResult<List<BookDto>> GetBooks([FromQuery] GetAllBooksQuery query)
        {
            var books = _mediator.Send(query);

            if (books.Result != null) return Ok(books.Result);
            else return BadRequest();

        }

        [HttpGet("/book/mediator/get/{id}")]
        public ActionResult<BookDto> GetBook([FromQuery] GetBookQuery query)
        {
            var book = _mediator.Send(query);

            if (book != null) return Ok(book);
            else return BadRequest();
        }

        [HttpPost("/book/mediator/create")]
        public ActionResult<Book> CreateBook(BookDto book)
        {
            try
            {
                _mediator.Send(new CreateBookCommand() { newBook = book });
                return Ok(book);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/book/mediator/add/rate/{id}")]
        public ActionResult AddRateToBook([FromRoute] int id, int rate)
        {
            try
            {
                _mediator.Send(new AddRateToBookCommand() { id = id, rate = rate });
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("/book/mediator/delete/{id}")]
        public ActionResult DeleteBook([FromRoute] int id)
        {
            try
            {
                _mediator.Send(new DeleteBookCommand() {id = id });
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion

        #region AUTHOR REGION

        [HttpGet("/authors/mediator/get")]
        public ActionResult<List<AuthorDto>> GetAuthorsMediator([FromQuery] GetAllAuthorsQuery query)
        {
            var authors = _mediator.Send(query);
            if (authors.Result != null) return Ok(authors.Result);
            else return BadRequest();
        }

        [HttpPost("/author/mediator/create")]
        public ActionResult<AuthorDto> CreateAuthor(AuthorDto author)
        {
            try
            {
                _mediator.Send(new CreateAuthorCommand() { newAuthor = author });
                return Ok(author);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("/author/mediator/add/rate/{id}")]
        public ActionResult AddRateToAuthor([FromRoute] int id, int rate)
        {
            try
            {
                _mediator.Send(new AddRateToAuthorCommand() { id = id, rate = rate });
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("/author/mediator/delete/{id}")]
        public ActionResult DeleteAuthor([FromRoute] int id)
        {
            try
            {
                var result = _mediator.Send(new DeleteAuthorCommand() { Id = id });

                return (result.Result ? Ok() : BadRequest());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion
    }
}
