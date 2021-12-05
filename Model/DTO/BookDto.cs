using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.DTO
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string AverageRate { get; set; }
        public int RatesCount { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public List<AuthorVM> Authors { get; set; }
    }

    public class AuthorVM
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }
}
