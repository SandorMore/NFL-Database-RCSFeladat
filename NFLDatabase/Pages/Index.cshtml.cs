using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;

namespace NFLDatabase.Pages
{
    public class IndexModel : PageModel
    {
        private readonly NFLDbContext _context;

        public IndexModel(NFLDbContext context)
        {
            _context = context;
        }

        public int TeamCount { get; set; }
        public int PlayerCount { get; set; }
        public int StatsCount { get; set; }

        public async Task OnGetAsync()
        {
            TeamCount = await _context.Teams.CountAsync();
            PlayerCount = await _context.Players.CountAsync();
            StatsCount = await _context.PlayerSeasonStats.CountAsync();
        }
    }
}
