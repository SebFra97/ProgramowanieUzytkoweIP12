using CQRS;
using CQRS.Authors.Command;
using CQRS.Authors.Query;
using CQRS.Books.Command;
using CQRS.Books.Query;
using Microsoft.AspNetCore.Mvc;
using Model;
using Models.DTO;
using Nest;
using System;
using System.Collections.Generic;

namespace ProgramowanieUzytkoweIP12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CQRSController : ControllerBase 
    { 
        private readonly CommandBus commandBus;
        private readonly QueryBus queryBus;

        public CQRSController(CommandBus commandBus, QueryBus queryBus)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
        }

        #region BOOK ENDPOINTS

        [HttpGet("/book/cqrs/get")]
        public List<BookDto> GetBooks([FromQuery] GetAllBooksQuery query)
        {
            return queryBus.Handle<GetAllBooksQuery, List<BookDto>>(query);
        }

        [HttpGet("/book/cqrs/get/{id}")]
        public ActionResult<BookDto> GetBook([FromQuery] GetBookQuery query)
        {
            return queryBus.Handle<GetBookQuery, BookDto>(query);
        }

        [HttpPost("/book/cqrs/create")]
        public void CreateBook([FromBody] AddBookCommand command)
        {
            commandBus.Handle(command);
        }

        [HttpPost("/book/cqrs/add/rate/{id}")]
        public void AddRateToBook([FromBody] AddRateToBookCommand command)
        {
            commandBus.Handle(command);
        }

        [HttpDelete("/book/cqrs/delete/{id}")]
        public void DeleteBook([FromBody] DeleteBookCommand command)
        {
            commandBus.Handle(command);
        }

        #endregion
        #region AUTHOR ENDPOINTS

        [HttpGet("/authors/cqrs/get")]
        public List<AuthorDto> GetAuthors([FromQuery] GetAllAuthorsQuery query)
        {
            return queryBus.Handle<GetAllAuthorsQuery, List<AuthorDto>>(query);
        }

        [HttpPost("/author/cqrs/create")]
        public void CreateAuthor([FromBody] CreateAuthorCommand command)
        {
            commandBus.Handle(command);
        }
        [HttpPost("/author/cqrs/add/rate/{id}")]
        public void AddRateToAuthor([FromBody] AddRateToAuthorCommand command)
        {
            commandBus.Handle(command);
        }
        [HttpDelete("/author/cqrs/delete/{id}")]
        public void DeleteAuthor([FromBody] DeleteBookCommand command)
        {
            commandBus.Handle(command);
        }

        #endregion
    }
}
