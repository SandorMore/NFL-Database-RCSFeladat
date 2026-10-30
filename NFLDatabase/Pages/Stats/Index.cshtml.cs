using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Stats
{
    public class IndexModel : PageModel
    {
        private readonly NFLDbContext _context;

        public IndexModel(NFLDbContext context)
        {
            _context = context;
        }

        public IList<PlayerSeasonStats> Stats { get; set; } = new List<PlayerSeasonStats>();

        public async Task OnGetAsync()
        {
            Stats = await _context.PlayerSeasonStats
                .Include(s => s.Player)
                .OrderByDescending(s => s.Season)
                .ThenBy(s => s.Player!.LastName)
                .ToListAsync();
        }
    }
}
