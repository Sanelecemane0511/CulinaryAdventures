using System;
using System.ComponentModel.DataAnnotations;

namespace CulinaryAdventures.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        [Required, StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [StringLength(500)]
        public string Comment { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public Recipe Recipe { get; set; } = null!;
    }
}