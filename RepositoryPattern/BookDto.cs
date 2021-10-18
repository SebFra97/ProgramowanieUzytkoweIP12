using System;
using System.Collections.Generic;

namespace RepositoryPattern
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string AverageRate { get; set; }
        public int RatesCount { get; set; }
        public List<AuthorVM> Authors { get; set; }
    }

    public class AuthorVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }
}
