using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Helpers.Init;
using MigrationHelper.Db;
using Microsoft.EntityFrameworkCore;

namespace MigrationHelper.Pages.Config;

public class LoadPGModel : PageModel
{
    private readonly ILogger<LoadPGModel> _logger;

    private readonly MigHelperCtx _context;
    public int DbCount {get;set;}    

      public LoadPGModel(ILogger<LoadPGModel> logger,MigHelperCtx context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task OnGet()
    {
        await _context.PayPeriods.ExecuteDeleteAsync();
        await _context.ScoreCache.ExecuteDeleteAsync();
        await _context.ScoreBreakdown.ExecuteDeleteAsync();
        DBLoader db = new();
        DbCount = await db.PGLoader();
        

    }
 
}
