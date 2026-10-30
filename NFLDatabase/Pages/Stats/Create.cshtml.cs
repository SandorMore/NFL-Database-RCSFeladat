using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Stats
{
    public class CreateModel : PageModel
    {
        private readonly NFLDbContext _context;

        public CreateModel(NFLDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PlayerSeasonStats Stats { get; set; } = default!;

        public SelectList PlayerOptions { get; set; } = default!;

        public IActionResult OnGet()
        {
            LoadPlayers();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _context.PlayerSeasonStats.AnyAsync(s => s.PlayerId == Stats.PlayerId && s.Season == Stats.Season))
                ModelState.AddModelError("Stats.Season", "This player already has stats for that season.");

            if (!ModelState.IsValid)
            {
                LoadPlayers();
                return Page();
            }

            _context.PlayerSeasonStats.Add(Stats);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void LoadPlayers()
        {
            var players = _context.Players
                .OrderBy(p => p.LastName).ThenBy(p => p.FirstName)
                .Select(p => new { p.Id, Name = p.LastName + ", " + p.FirstName + " (" + p.Position + ")" });

            PlayerOptions = new SelectList(players, "Id", "Name");
        }
    }
}
