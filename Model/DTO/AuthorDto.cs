using Model;
using Nest;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models.DTO
{
    [ElasticsearchType(IdProperty = nameof(Id))]
    public class AuthorDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }
        [JsonPropertyName("secondname")]
        public string SecondName { get; set; }
        [JsonPropertyName("averagerate")]
        public string AverageRate { get; set; }
        [JsonPropertyName("ratescount")]
        public int RatesCount { get; set; }
        [JsonPropertyName("id")]
        public List<BookVM> Books { get; set; }
        [JsonPropertyName("id")]
        [StringLength(1000)]
        public string CV { get; set; }
    }

    public class BookVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
