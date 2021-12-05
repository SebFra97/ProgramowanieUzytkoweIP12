using Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string AverageRate { get; set; }
        public int RatesCount { get; set; }
        public List<BookVM> Books { get; set; }
        [StringLength(1000)]
        public string CV { get; set; }
    }

    public class BookVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
