using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Stats
{
    public class DetailsModel : PageModel
    {
        private readonly NFLDbContext _context;

        public DetailsModel(NFLDbContext context)
        {
            _context = context;
        }

        public PlayerSeasonStats Stats { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var stats = await _context.PlayerSeasonStats
                .Include(s => s.Player)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (stats == null)
                return NotFound();

            Stats = stats;
            return Page();
        }
    }
}
