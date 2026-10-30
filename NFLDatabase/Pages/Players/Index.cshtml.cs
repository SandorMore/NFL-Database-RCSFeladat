using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Players
{
    public class IndexModel : PageModel
    {
        private readonly NFLDbContext _context;

        public IndexModel(NFLDbContext context)
        {
            _context = context;
        }

        public IList<Player> Players { get; set; } = new List<Player>();

        public async Task OnGetAsync()
        {
            Players = await _context.Players
                .Include(p => p.Team)
                .OrderBy(p => p.LastName).ThenBy(p => p.FirstName)
                .ToListAsync();
        }
    }
}
