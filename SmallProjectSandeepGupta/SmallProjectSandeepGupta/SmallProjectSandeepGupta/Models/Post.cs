using System.Text.Json.Serialization;

namespace SmallProjectSandeepGupta.Models
{
    public class Post
    {
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public int LikesCount { get; set; }

        // Use JsonIgnore to prevent cyclic references during serialization
        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
