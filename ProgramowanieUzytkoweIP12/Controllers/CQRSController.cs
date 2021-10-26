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
    public class CQRSController : ControllerBase { 
    

        private readonly CommandBus commandBus;

        public CQRSController(CommandBus commandBus)
        {
            this.commandBus = commandBus;
        }



        #region BOOK ENDPOINTS

        [HttpGet("/book/cqrs/get")]
        public List<BookDto> GetBooks()
        {
            return null;
        }

        [HttpGet("/book/cqrs/get/{id}")]
        public ActionResult<BookDto> GetBook([FromRoute] int id)
        {
            return null;
        }

        [HttpPost("/book/cqrs/create")]
        public ActionResult<Book> CreateBook(BookDto book)
        {
            return null;
        }

        [HttpPost("/book/cqrs/add/rate/{id}")]
        public ActionResult AddRateToBook([FromRoute] int id, int rate)
        {
            return null;
        }

        [HttpDelete("/book/cqrs/delete/{id}")]
        public ActionResult DeleteBook([FromRoute] int id)
        {
            return null; 
        }

        #endregion
        #region AUTHOR ENDPOINTS

        [HttpGet("/authors/cqrs/get")]
        public List<AuthorDto> GetAuthor()
        {
            return null;
        }

        [HttpPost("/author/cqrs/create")]
        public ActionResult<Author> CreateAuthor(AuthorDto author)
        {
            return null;
        }
        [HttpPost("/author/cqrs/add/rate/{id}")]
        public ActionResult AddRateToAuthor([FromRoute] int id, int rate)
        {
            return null;
        }
        [HttpDelete("/author/cqrs/delete/{id}")]
        public ActionResult DeleteAuthor([FromRoute] int id)
        {
            return null;
        }

        #endregion
    }
}
