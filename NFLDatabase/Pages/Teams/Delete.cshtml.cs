using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Teams
{
    public class DeleteModel : PageModel
    {
        private readonly NFLDbContext _context;

        public DeleteModel(NFLDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Team Team { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
                return NotFound();

            Team = team;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            // The team's players become free agents (TeamId is set to null by the database)
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
