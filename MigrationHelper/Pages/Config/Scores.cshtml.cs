using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MigrationHelper.Db;
using MigrationHelper.Helpers.Init;
using MigrationHelper.Models;

using Microsoft.EntityFrameworkCore;

namespace MigrationHelper.Pages;

public class ScoresModel : PageModel
{
    private readonly ILogger<ScoresModel> _logger;
    private readonly MigHelperCtx _context;

    [BindProperty]
    public   ScoreConfig Sc { get; set; }
    

    public ScoresModel(ILogger<ScoresModel> logger,MigHelperCtx context)
    {
        _context = context;
        _logger = logger;
    }
    
     public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Sc != null) _context.Attach(Sc).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        await _context.ScoreCache.ExecuteDeleteAsync();
        await _context.ScoreBreakdown.ExecuteDeleteAsync();
        DBLoader db = new();
        await db.PGCacheLoader();
        return RedirectToPage("./Index");
    }

    public async Task OnGet()
    {
        Sc = await _context.ScoreConfig.FirstOrDefaultAsync();
    }
}
