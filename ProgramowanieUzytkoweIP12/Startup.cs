using Helpers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model;
using RepositoryPattern;
using System;
using static CQRS.Books.Command.AddBookCommand;
using static CQRS.Books.Command.AddRateToBookCommand;
using static CQRS.Books.Command.DeleteBookCommand;

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
            var assembly = AppDomain.CurrentDomain.Load("CQRSMediatR");
            services.AddMediatR(assembly);
            services.AddSwaggerGen();

            services.AddScoped<CommandBus>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookHelpers,BookHelpers>();
            services.AddScoped<ICommandHandler<AddBookCommand>, AddBookCommandHandler>();
            services.AddScoped<ICommandHandler<AddRateToBookCommand>, AddRateToBookCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteBookCommand>, DeleteBookCommandHandler>();


            services.AddDbContext<ApplicationDbContext>(options =>
                                                        options.UseNpgsql("Server=localhost;Port=5432;Database=PU_Database;User Id=sebfra;Password=zaq1@WSX;"));

            
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
