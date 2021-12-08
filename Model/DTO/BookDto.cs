using Model;
using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models.DTO
{
    [ElasticsearchType(IdProperty = nameof(Id))]
    public class BookDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("Title")]
        public string Title { get; set; }
        [JsonPropertyName("releasedate")]
        public DateTime ReleaseDate { get; set; }
        [JsonPropertyName("averagerate")]
        public string AverageRate { get; set; }
        [JsonPropertyName("ratescount")]
        public int RatesCount { get; set; }
        [JsonPropertyName("Description")]
        public string Description { get; set; }
       // [JsonPropertyName("authors")]
        public List<AuthorVM> Authors { get; set; }
    }

    public class AuthorVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string CV { get; set; }
    }
}
