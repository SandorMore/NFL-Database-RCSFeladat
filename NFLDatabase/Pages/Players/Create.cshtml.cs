using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Players
{
    public class CreateModel : PageModel
    {
        private readonly NFLDbContext _context;

        public CreateModel(NFLDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Player Player { get; set; } = default!;

        public SelectList TeamOptions { get; set; } = default!;

        public IActionResult OnGet()
        {
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

            _context.Players.Add(Player);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private void LoadTeams()
        {
            TeamOptions = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name");
        }
    }
}
