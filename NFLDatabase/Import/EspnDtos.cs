namespace NFLDatabase.Import
{
    public class EspnTeamsResponse
    {
        public List<EspnSport> Sports { get; set; } = new();
    }

    public class EspnSport
    {
        public List<EspnLeague> Leagues { get; set; } = new();
    }

    public class EspnLeague
    {
        public List<EspnTeamWrapper> Teams { get; set; } = new();
    }

    public class EspnTeamWrapper
    {
        public EspnTeam Team { get; set; } = null!;
    }

    public class EspnTeam
    {
        public string Id { get; set; } = "";
        public string Abbreviation { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Location { get; set; } = "";
        public string? Color { get; set; }
        public List<EspnImage> Logos { get; set; } = new();
    }

    public class EspnRosterResponse
    {
        public List<EspnRosterGroup> Athletes { get; set; } = new();
    }

    public class EspnRosterGroup
    {
        public List<EspnAthlete> Items { get; set; } = new();
    }

    public class EspnAthlete
    {
        public string Id { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string? Jersey { get; set; } 
        public double? Height { get; set; }   
        public double? Weight { get; set; }   
        public int? Age { get; set; }
        public EspnPosition? Position { get; set; }
        public EspnCollege? College { get; set; }
        public EspnImage? Headshot { get; set; }
    }

    public class EspnPosition
    {
        public string Abbreviation { get; set; } = "";
    }

    public class EspnCollege
    {
        public string Name { get; set; } = "";
    }

    public class EspnImage
    {
        public string Href { get; set; } = "";
    }

    public class EspnStatsResponse
    {
        public EspnStatsSplits? Splits { get; set; }

        public double Get(string category, string stat) =>
            Splits?.Categories.FirstOrDefault(c => c.Name == category)?
                   .Stats.FirstOrDefault(s => s.Name == stat)?.Value ?? 0;
    }

    public class EspnStatsSplits
    {
        public List<EspnStatCategory> Categories { get; set; } = new();
    }

    public class EspnStatCategory
    {
        public string Name { get; set; } = "";
        public List<EspnStat> Stats { get; set; } = new();
    }

    public class EspnStat
    {
        public string Name { get; set; } = "";
        public double Value { get; set; }
    }
}
