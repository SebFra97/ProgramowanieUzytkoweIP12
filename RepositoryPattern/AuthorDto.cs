using System.Collections.Generic;

namespace RepositoryPattern
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string AverageRate { get; set; }
        public int RatesCount { get; set; }
        public List<BookVM> Books { get; set; }
    }

    public class BookVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
