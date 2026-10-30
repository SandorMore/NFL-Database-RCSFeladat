using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Teams
{
    public class DetailsModel : PageModel
    {
        private readonly NFLDbContext _context;

        public DetailsModel(NFLDbContext context)
        {
            _context = context;
        }

        public Team Team { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var team = await _context.Teams
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
                return NotFound();

            Team = team;
            return Page();
        }
    }
}
