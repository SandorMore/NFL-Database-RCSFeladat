using System.ComponentModel.DataAnnotations;

namespace NFLDatabase.Models
{
    public class Player
    {
        public int Id { get; set; }

        // ESPN athlete id
        [MaxLength(20)]
        public string? ExternalId { get; set; }

        [Required, MaxLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = "";

        [Required, MaxLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = "";

        [Range(0, 99)]
        [Display(Name = "Jersey #")]
        public int? JerseyNumber { get; set; }

        [Required, MaxLength(5)]
        public string Position { get; set; } = ""; 

        public PlayerSide Side { get; set; }

        [Range(60, 90)]
        [Display(Name = "Height (in)")]
        public int? HeightInches { get; set; }

        [Range(150, 400)]
        [Display(Name = "Weight (lbs)")]
        public int? WeightLbs { get; set; }

        [Range(18, 50)]
        public int? Age { get; set; }

        [MaxLength(100)]
        public string? College { get; set; }

        [Url, MaxLength(300)]
        [Display(Name = "Headshot")]
        public string? HeadshotUrl { get; set; }

        [Display(Name = "Team")]
        public int? TeamId { get; set; }
        public Team? Team { get; set; }

        public List<PlayerSeasonStats> SeasonStats { get; set; } = new();

        [Display(Name = "Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
