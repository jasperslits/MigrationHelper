using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.Db;

namespace MigrationHelper.Pages;

public class MonthviewModel : PageModel
{
    private readonly ILogger<MonthviewModel> _logger;


    public List<GccNames> Gcc { get; set; }

    public IQueryable<IGrouping<int,GccNames>> Res{ get; set; }

    private readonly MigHelperCtx _context;

    public string FormatMonth(int nr) 
    { 
        if (nr == 0) {
            return "No month";
        }
        int year = 2024;
        if (nr < 9) year = 2025;
        return Helpers.Toolbox.MonthToName(year,nr);
    }

    public MonthviewModel(ILogger<MonthviewModel> logger,MigHelperCtx context)
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet()
    {
        var Res2 = _context.GccNames.Where(x => x.Migrated == false).OrderBy(x => x.Gcc);
        Res = Res2.GroupBy(x => x.Month);


    
    }
}
