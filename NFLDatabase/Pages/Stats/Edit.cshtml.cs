using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Stats
{
    public class EditModel : PageModel
    {
        private readonly NFLDbContext _context;

        public EditModel(NFLDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PlayerSeasonStats Stats { get; set; } = default!;

        public string PlayerName { get; set; } = "";

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
            PlayerName = stats.Player!.FullName;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _context.PlayerSeasonStats.AnyAsync(s => s.PlayerId == Stats.PlayerId && s.Season == Stats.Season && s.Id != Stats.Id))
                ModelState.AddModelError("Stats.Season", "This player already has stats for that season.");

            if (!ModelState.IsValid)
            {
                PlayerName = (await _context.Players.FindAsync(Stats.PlayerId))?.FullName ?? "";
                return Page();
            }

            _context.Attach(Stats).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.PlayerSeasonStats.AnyAsync(s => s.Id == Stats.Id))
                    return NotFound();
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
