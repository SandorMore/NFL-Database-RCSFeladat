using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NFLDatabase.Models;

namespace NFLDatabase.Data
{
    public class NFLDbContext : DbContext
    {
        public NFLDbContext(DbContextOptions<NFLDbContext> options) : base(options)
        {
            
        }

        DbSet<Player> Players { get; set; }
        DbSet<Team> Teams { get; set; }
    }
}
