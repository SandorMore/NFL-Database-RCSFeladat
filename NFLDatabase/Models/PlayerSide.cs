using System.ComponentModel.DataAnnotations;

namespace NFLDatabase.Models
{
    public enum PlayerSide
    {
        Offense,
        Defense,

        [Display(Name = "Special Teams")]
        SpecialTeams
    }
}
