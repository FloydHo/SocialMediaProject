using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace SocialMediaAPI.Entities
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_]*$", ErrorMessage = "Only letters, numbers, spaces, hyphens, and underscores are allowed.")]
        public string Name { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        [JsonIgnore]
        public IdentityUser? CreatedByUser { get; set; }

        [JsonIgnore]
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
