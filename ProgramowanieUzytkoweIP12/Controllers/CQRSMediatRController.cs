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
        public Task<List<BookDto>> GetBooks()
        {
            return _mediator.Send(new GetAllBooksQuery());
        }

        [HttpGet("/book/mediator/get/{id}")]
        public ActionResult<BookDto> GetBook([FromRoute] int id)
        {
            var book = _mediator.Send(new GetBookQuery() { id = id });

            if (book != null) return Ok();
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
        public Task<List<AuthorDto>> GetAuthorsMediator()
        {
            return _mediator.Send(new GetAllAuthorsQuery());
        }

        [HttpPost("/author/mediator/create")]
        public ActionResult<AuthorDto> CreateAuthor(AuthorDto author)
        {
            try
            {
                _mediator.Send(new CreateNewAuthorCommand() { newAuthor = author });
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
