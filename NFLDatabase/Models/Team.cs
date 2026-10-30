using System.ComponentModel.DataAnnotations;

namespace NFLDatabase.Models
{
    public class Team
    {
        public int Id { get; set; }

        // ESPN team id
        [MaxLength(20)]
        public string? ExternalId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = "";          

        [Required, MaxLength(5)]
        public string Abbreviation { get; set; } = ""; 

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(10)]
        public string? Color { get; set; }

        [Url, MaxLength(300)]
        [Display(Name = "Logo")]
        public string? LogoUrl { get; set; }

        public List<Player> Players { get; set; } = new();
    }
}
