using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CulinaryAdventures.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(30)]
        public string Category { get; set; } = string.Empty; // e.g. "Cape Malay"

        [Required, StringLength(500)]
        public string Ingredients { get; set; } = string.Empty;

        [Required, StringLength(2000)]
        public string Instructions { get; set; } = string.Empty;

        [Range(1, 120)]
        public int PrepTimeMin { get; set; }

        [Range(1, 20)]
        public int Servings { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        // navigation
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public double AverageRating => Reviews.Count == 0 
                               ? 0 
                               : Math.Round(Reviews.Average(r => r.Rating), 1);

        [StringLength(50)]
        public string OwnerName { get; set; } = string.Empty;   
    }
}