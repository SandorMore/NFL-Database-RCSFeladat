using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // Filters come from the query string, e.g. /Players?Side=Offense&Position=RB
        [BindProperty(SupportsGet = true)]
        public PlayerSide? Side { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Position { get; set; }

        public SelectList PositionOptions { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var query = _context.Players.Include(p => p.Team).AsQueryable();

            if (Side.HasValue)
                query = query.Where(p => p.Side == Side.Value);

            if (!string.IsNullOrEmpty(Position))
                query = query.Where(p => p.Position == Position);

            Players = await query
                .OrderBy(p => p.LastName).ThenBy(p => p.FirstName)
                .ToListAsync();

            var positions = await _context.Players
                .Select(p => p.Position)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();

            PositionOptions = new SelectList(positions, Position);
        }
    }
}
