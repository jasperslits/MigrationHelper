using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MigrationHelper.Db;

namespace MigrationHelper.Pages.Config;

public class FlushCacheModel : PageModel
{
    private readonly ILogger<FlushCacheModel> _logger;

    private readonly MigHelperCtx _context;


    public int DbCountSC {get;set;}    
    public int DbCountBD {get;set;}    

    public FlushCacheModel(ILogger<FlushCacheModel> logger,MigHelperCtx context)
    {
        _context = context;
        _logger = logger;
    }

    public async void OnGet()
    {
        DbCountSC = await _context.ScoreCache.ExecuteDeleteAsync();
        DbCountBD = await _context.ScoreBreakdown.ExecuteDeleteAsync();
        
    }
 
}
