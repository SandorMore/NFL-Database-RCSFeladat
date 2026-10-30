using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Stats
{
    public class DeleteModel : PageModel
    {
        private readonly NFLDbContext _context;

        public DeleteModel(NFLDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var stats = await _context.PlayerSeasonStats.FindAsync(id);
            if (stats != null)
            {
                _context.PlayerSeasonStats.Remove(stats);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
