using System.Text.Json.Serialization;

namespace SmallProjectSandeepGupta.Models
{
    public class Like
    {
        public int LikeID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public DateTime LikedAt { get; set; }

        // Use JsonIgnore to prevent cyclic references during serialization
        [JsonIgnore]
        public Post? Post { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
