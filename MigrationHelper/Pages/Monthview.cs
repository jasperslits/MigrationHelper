using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.Db;

namespace MigrationHelper.Pages;

public class MonthviewModel : PageModel
{
    private readonly ILogger<MonthviewModel> _logger;

    public IQueryable<IGrouping<int,GccNames>> Res{ get; set; }

    private readonly MigHelperCtx _context;

    public string FormatMonth(int nr) 
    { 
        if (nr == 0) {
            return "No month";
        }
  
        int year = (nr >=1 && nr < 9) ? 2025: 2024;
        return Helpers.Toolbox.MonthToName(year,nr);
    }

    public MonthviewModel(ILogger<MonthviewModel> logger,MigHelperCtx context)
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet()
    {
        var res2 = _context.GccNames.Where(x => x.Migrated == false).OrderBy(x => x.Gcc);
        Res = res2.GroupBy(x => x.Month);


    
    }
}
