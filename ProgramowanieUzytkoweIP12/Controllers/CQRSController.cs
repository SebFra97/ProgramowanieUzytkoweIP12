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
        private readonly IElasticClient _elasticClient;

        public CQRSController(CommandBus commandBus, QueryBus queryBus, IElasticClient elasticClient)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
            _elasticClient = elasticClient;
        }

        public class ElasticModel
        {
            public string Currency { get; set; }
            public string Customer_first_name{ get; set; }
            public string Customer_full_name{ get; set; }
            public string Customer_gender{ get; set; }
            public int Customer_id{ get; set; }
        }

        [HttpGet("/book/cqrs/es/get")]
        public IEnumerable<BookDto> GetElasticBooks([FromQuery] PaginationDto pagination, [FromQuery] string filter)
        {
            //return ElasticClient.Search<ElasticModel>(x => x.Index("kibana_sample_data_ecommerce").Query(q => q.Match(q => q.Field(f => f.Customer_first_name).Query("Eddie")))).Documents;
            //IndexResponse res = _elasticClient.IndexDocument<BookDto>(person);


            var x = _elasticClient.Search<BookDto>(x => x.Size(10).Skip(0)
            .Query(q => q
                .QueryString(qs =>
                    qs.Fields(x => x
                        .Field(f => f.Title)
                        .Field(f => f.Description))
                        .Query("*" + filter + "*")))).Documents;

            return x;

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
