using CQRS;
using CQRS.Authors.Command;
using CQRS.Authors.Query;
using CQRS.Books.Command;
using CQRS.Books.Query;
using Helpers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model;
using Models.DTO;
using Nest;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using static CQRS.Authors.Command.AddRateToAuthorCommand;
using static CQRS.Authors.Command.CreateAuthorCommand;
using static CQRS.Authors.Command.DeleteAuthorCommand;
using static CQRS.Authors.Query.GetAllAuthorsQuery;
using static CQRS.Books.Command.AddBookCommand;
using static CQRS.Books.Command.AddRateToBookCommand;
using static CQRS.Books.Command.DeleteBookCommand;
using static CQRS.Books.Query.GetAllBooksQuery;
using static CQRS.Books.Query.GetBookQuery;

namespace ProgramowanieUzytkoweIP12
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddScoped<IElasticClient>(x => new ElasticClient(new ConnectionSettings(new Uri("http://localhost:9200"))));
            var assembly = AppDomain.CurrentDomain.Load("CQRSMediatR");
            services.AddMediatR(assembly);
            services.AddSwaggerGen();

            services.AddScoped<CommandBus>();
            services.AddScoped<QueryBus>();

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookHelpers,BookHelpers>();
            services.AddScoped<IAuthorHelpers, AuthorHelpers>();

            services.AddScoped<ICommandHandler<AddBookCommand>, AddBookCommandHandler>();
            services.AddScoped<ICommandHandler<AddRateToBookCommand>, AddRateToBookCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteBookCommand>, DeleteBookCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllBooksQuery, List<BookDto>>, GetAllBooksQueryHandler>();
            services.AddScoped<IQueryHandler<GetBookQuery, BookDto>, GetBookQueryHandler>();

            services.AddScoped<ICommandHandler<AddRateToAuthorCommand>, AddRateToAuthorCommandHandler>();
            services.AddScoped<ICommandHandler<CreateAuthorCommand>, CreateNewAuthorCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteAuthorCommand>, DeleteAuthorCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllAuthorsQuery, List<AuthorDto>>, GetAllAuthorsQueryHandler>();

            services.AddDbContext<ApplicationDbContext>(options =>
                                                        options.UseNpgsql("Server=localhost;Port=5432;Database=PU_Database;User Id=postgres;Password=sebfra1;"));

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SebFra API v1");
                c.RoutePrefix = "api";
            });

        }
    }
}
