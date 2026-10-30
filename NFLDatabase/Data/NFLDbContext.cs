using Microsoft.EntityFrameworkCore;
using NFLDatabase.Models;

namespace NFLDatabase.Data
{
    public class NFLDbContext : DbContext
    {
        public NFLDbContext(DbContextOptions<NFLDbContext> options) : base(options)
        {

        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<PlayerSeasonStats> PlayerSeasonStats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Deleting a team turns its players into free agents instead of deleting them
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .OnDelete(DeleteBehavior.SetNull);

            // Deleting a player deletes their stats
            modelBuilder.Entity<PlayerSeasonStats>()
                .HasOne(s => s.Player)
                .WithMany(p => p.SeasonStats)
                .OnDelete(DeleteBehavior.Cascade);

            // A player can only have one stats row per season
            modelBuilder.Entity<PlayerSeasonStats>()
                .HasIndex(s => new { s.PlayerId, s.Season })
                .IsUnique();
        }
    }
}
