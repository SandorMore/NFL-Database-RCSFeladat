using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Import
{
    // Fills the empty database with real teams, players and season stats from ESPN.
    // Runs once: if there are already teams in the database it does nothing.
    public class DataSeeder
    {
        // Last full season + the current one
        private static readonly int[] Seasons = { 2025, 2026 };

        // How many stats requests run at the same time
        private const int MaxParallelRequests = 8;

        private static readonly HashSet<string> DefensePositions = new()
        {
            "DE", "DT", "NT", "DL", "LB", "OLB", "ILB", "MLB", "CB", "S", "FS", "SS", "DB", "EDGE"
        };

        private static readonly HashSet<string> SpecialTeamsPositions = new()
        {
            "PK", "K", "P", "LS"
        };

        private readonly NFLDbContext _db;
        private readonly EspnClient _espn;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(NFLDbContext db, EspnClient espn, ILogger<DataSeeder> logger)
        {
            _db = db;
            _espn = espn;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            if (await _db.Teams.AnyAsync())
            {
                _logger.LogInformation("Database already has data, skipping ESPN import.");
                return;
            }

            _logger.LogInformation("Importing teams and rosters from ESPN...");

            var players = new List<Player>();
            var seenPlayerIds = new HashSet<string>();

            foreach (var espnTeam in await _espn.GetTeamsAsync())
            {
                var team = MapTeam(espnTeam);
                _db.Teams.Add(team);

                foreach (var athlete in await _espn.GetRosterAsync(espnTeam.Id))
                {
                    if (!seenPlayerIds.Add(athlete.Id))
                        continue;

                    var player = MapPlayer(athlete);
                    player.Team = team;
                    players.Add(player);
                }
            }

            _db.Players.AddRange(players);
            _logger.LogInformation("Found {Teams} teams and {Players} players.", _db.Teams.Local.Count, players.Count);

            await ImportStatsAsync(players);

            await _db.SaveChangesAsync();
            _logger.LogInformation("ESPN import finished.");
        }

        private async Task ImportStatsAsync(List<Player> players)
        {
            _logger.LogInformation("Importing season stats for seasons {Seasons} (this takes a few minutes)...",
                string.Join(", ", Seasons));

            using var throttle = new SemaphoreSlim(MaxParallelRequests);

            // Only the HTTP calls run in parallel; the DbContext is touched afterwards on one thread
            var tasks = players
                .SelectMany(p => Seasons.Select(season => (Player: p, Season: season)))
                .Select(async job =>
                {
                    await throttle.WaitAsync();
                    try
                    {
                        var stats = await _espn.GetSeasonStatsAsync(job.Player.ExternalId!, job.Season);
                        return (job.Player, job.Season, Stats: stats);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not load {Season} stats for {Player}.", job.Season, job.Player.FullName);
                        return (job.Player, job.Season, Stats: (EspnStatsResponse?)null);
                    }
                    finally
                    {
                        throttle.Release();
                    }
                });

            var results = await Task.WhenAll(tasks);

            foreach (var (player, season, stats) in results)
            {
                // null = rookie / did not play that season
                if (stats?.Splits == null)
                    continue;

                player.SeasonStats.Add(MapStats(stats, season));
            }

            _logger.LogInformation("Loaded {Count} season stat lines.", results.Count(r => r.Stats?.Splits != null));
        }

        private static Team MapTeam(EspnTeam t) => new()
        {
            ExternalId = t.Id,
            Name = t.DisplayName,
            Abbreviation = t.Abbreviation,
            City = t.Location,
            Color = t.Color,
            LogoUrl = t.Logos.FirstOrDefault()?.Href
        };

        private static Player MapPlayer(EspnAthlete a)
        {
            var position = a.Position?.Abbreviation ?? "";

            return new Player
            {
                ExternalId = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                JerseyNumber = int.TryParse(a.Jersey, out var jersey) ? jersey : null,
                Position = position,
                Side = GetSide(position),
                HeightInches = a.Height.HasValue ? (int)Math.Round(a.Height.Value) : null,
                WeightLbs = a.Weight.HasValue ? (int)Math.Round(a.Weight.Value) : null,
                Age = a.Age,
                College = a.College?.Name,
                HeadshotUrl = a.Headshot?.Href
            };
        }

        // Decided by position, because ESPN puts injured / practice squad players in separate groups
        private static PlayerSide GetSide(string position)
        {
            if (SpecialTeamsPositions.Contains(position)) return PlayerSide.SpecialTeams;
            if (DefensePositions.Contains(position)) return PlayerSide.Defense;
            return PlayerSide.Offense;
        }

        private static PlayerSeasonStats MapStats(EspnStatsResponse s, int season) => new()
        {
            Season = season,
            GamesPlayed = (int)s.Get("general", "gamesPlayed"),

            PassingYards = (int)s.Get("passing", "passingYards"),
            PassingTouchdowns = (int)s.Get("passing", "passingTouchdowns"),
            InterceptionsThrown = (int)s.Get("passing", "interceptions"),

            RushingAttempts = (int)s.Get("rushing", "rushingAttempts"),
            RushingYards = (int)s.Get("rushing", "rushingYards"),
            RushingTouchdowns = (int)s.Get("rushing", "rushingTouchdowns"),

            Receptions = (int)s.Get("receiving", "receptions"),
            ReceivingYards = (int)s.Get("receiving", "receivingYards"),
            ReceivingTouchdowns = (int)s.Get("receiving", "receivingTouchdowns"),

            TotalTackles = (int)s.Get("defensive", "totalTackles"),
            TacklesForLoss = (int)s.Get("defensive", "tacklesForLoss"),
            Sacks = s.Get("defensive", "sacks"),
            Interceptions = (int)s.Get("defensiveInterceptions", "interceptions"),
            PassesDefended = (int)s.Get("defensive", "passesDefended"),
            ForcedFumbles = (int)s.Get("general", "fumblesForced"),

            FieldGoalsMade = (int)s.Get("kicking", "fieldGoalsMade"),
            FieldGoalAttempts = (int)s.Get("kicking", "fieldGoalAttempts")
        };
    }
}
