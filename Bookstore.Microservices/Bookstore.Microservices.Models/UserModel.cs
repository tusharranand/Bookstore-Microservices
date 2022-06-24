using Newtonsoft.Json;

namespace Bookstore.Microservices.Models
{
    public class UserModel
    {
        [JsonProperty("id")]
        public string UserID { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string mobileNumber { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}