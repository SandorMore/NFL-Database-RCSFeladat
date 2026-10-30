using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NFLDatabase.Data;
using NFLDatabase.Models;

namespace NFLDatabase.Pages.Teams
{
    public class IndexModel : PageModel
    {
        private readonly NFLDbContext _context;

        public IndexModel(NFLDbContext context)
        {
            _context = context;
        }

        public IList<Team> Teams { get; set; } = new List<Team>();

        public async Task OnGetAsync()
        {
            Teams = await _context.Teams.OrderBy(t => t.Name).ToListAsync();
        }
    }
}
