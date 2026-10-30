using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Players
{
    public class EditModel : PageModel
    {
        private readonly NFLDbContext _context;

        public EditModel(NFLDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Player Player { get; set; } = default!;

        public SelectList TeamOptions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == id);
            if (player == null)
                return NotFound();

            Player = player;
            LoadTeams();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadTeams();
                return Page();
            }

            _context.Attach(Player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Players.AnyAsync(p => p.Id == Player.Id))
                    return NotFound();
                throw;
            }

            return RedirectToPage("./Index");
        }

        private void LoadTeams()
        {
            TeamOptions = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name");
        }
    }
}
