using System.ComponentModel.DataAnnotations;

namespace NFLDatabase.Models
{
    // One row per player per regular season
    public class PlayerSeasonStats
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        [Range(1920, 2100)]
        public int Season { get; set; }

        [Display(Name = "Games")]
        public int GamesPlayed { get; set; }

        // Passing
        [Display(Name = "Pass Yds")]
        public int PassingYards { get; set; }

        [Display(Name = "Pass TD")]
        public int PassingTouchdowns { get; set; }

        [Display(Name = "INT Thrown")]
        public int InterceptionsThrown { get; set; }

        // Rushing
        [Display(Name = "Rush Att")]
        public int RushingAttempts { get; set; }

        [Display(Name = "Rush Yds")]
        public int RushingYards { get; set; }

        [Display(Name = "Rush TD")]
        public int RushingTouchdowns { get; set; }

        // Receiving
        [Display(Name = "Rec")]
        public int Receptions { get; set; }

        [Display(Name = "Rec Yds")]
        public int ReceivingYards { get; set; }

        [Display(Name = "Rec TD")]
        public int ReceivingTouchdowns { get; set; }

        // Defense
        [Display(Name = "Tackles")]
        public int TotalTackles { get; set; }

        [Display(Name = "TFL")]
        public int TacklesForLoss { get; set; }

        public double Sacks { get; set; }                // half sacks exist, e.g. 7.5

        [Display(Name = "INT")]
        public int Interceptions { get; set; }

        [Display(Name = "PD")]
        public int PassesDefended { get; set; }

        [Display(Name = "FF")]
        public int ForcedFumbles { get; set; }

        // Kicking
        [Display(Name = "FGM")]
        public int FieldGoalsMade { get; set; }

        [Display(Name = "FGA")]
        public int FieldGoalAttempts { get; set; }

        // Touchdowns the player scored (passing TDs are scored by the receiver)
        [Display(Name = "Total TD")]
        public int TotalTouchdowns => RushingTouchdowns + ReceivingTouchdowns;
    }
}
