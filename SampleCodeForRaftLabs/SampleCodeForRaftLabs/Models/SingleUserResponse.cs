using System.Text.Json.Serialization;

namespace SampleCodeForRaftLabs.Models
{
    public class SingleUserResponse
    {
        [JsonPropertyName("data")]
        public User? Data { get; set; }
    }
}
