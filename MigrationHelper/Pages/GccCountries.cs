using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MigrationHelper.Db;
using MigrationHelper.Models;

namespace MigrationHelper.Pages;

public class GccCountriesModel : PageModel
{
    private readonly ILogger<GccCountriesModel> _logger;
    private readonly MigHelperCtx _context;

    public GccNames Gcc { get; set; }
    
    public List<Countries> Countries { get; set; }


    public GccCountriesModel(ILogger<GccCountriesModel> logger,MigHelperCtx context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task OnGet(string gcc)
    {
     
        Gcc = _context.GccNames.FirstOrDefault(g => g.Gcc == gcc);
        List<string> cts = await _context.PayPeriods.Where(g => g.Gcc == gcc).Select(x => x.Lcc.Substring(0,2)).Distinct().ToListAsync();
        Countries = await _context.Countries.Where(x => cts.Contains(x.Code)).ToListAsync();
      
    }
}
