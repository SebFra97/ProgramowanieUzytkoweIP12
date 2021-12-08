using Microsoft.EntityFrameworkCore;
using System;

namespace Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorRate> AuthorsRates { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookRate> BookRate { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rate>()
                        .HasDiscriminator(x => x.Type)
                        .HasValue<AuthorRate>(RateType.AuthorRate)
                        .HasValue<BookRate>(RateType.BookRate);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite($"Data Source={Environment.CurrentDirectory}\\database.db");
    }
}
