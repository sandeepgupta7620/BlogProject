using System.Text.Json.Serialization;

namespace SmallProjectSandeepGupta.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        // Use JsonIgnore to prevent cyclic references during serialization
        [JsonIgnore]
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        [JsonIgnore]
        public ICollection<Like> Likes { get; set; }= new List<Like>();
    }
}

