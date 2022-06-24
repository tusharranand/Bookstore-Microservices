using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Microservices.Models
{
    public class BookModel
    {

        [JsonProperty("id")]
        public string BookID { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("bookName")]
        public string BookName { get; set; }

        [JsonProperty("authorName")]
        public string AuthorName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("averageRating")]
        public float AverageRating { get; set; }

        [JsonProperty("totalReviews")]
        public int TotalReviews { get; set; }

        [JsonProperty("currentPrice")]
        public int CurrentPrice { get; set; }

        [JsonProperty("originalPrice")]
        public int OriginalPrice { get; set; }

        [JsonProperty("availableQuantity")]
        public int AvailableQuantity { get; set; }
    }
}
