using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Players
{
    public class DetailsModel : PageModel
    {
        private readonly NFLDbContext _context;

        public DetailsModel(NFLDbContext context)
        {
            _context = context;
        }

        public Player Player { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var player = await _context.Players
                .Include(p => p.Team)
                .Include(p => p.SeasonStats)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (player == null)
                return NotFound();

            Player = player;
            return Page();
        }
    }
}
