using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace SocialMediaAPI.Entities
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_]*$", ErrorMessage = "Only letters, numbers, spaces, hyphens, and underscores are allowed.")]
        public string Title { get; set; }
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string? CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        [JsonIgnore]
        public IdentityUser? CreatedByUser { get; set; }


        [Required]
        public int GroupId { get; set; }

        [Required]
        [JsonIgnore]
        public Group Group { get; set; }

    }
}
