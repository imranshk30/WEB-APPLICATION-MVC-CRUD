using System.Text.Json.Serialization;

namespace WebApplication_MVC_CRUD.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
    }
}
