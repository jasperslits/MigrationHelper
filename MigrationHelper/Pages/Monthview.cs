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
        return Toolbox.MonthToName(nr);
    }

    public MonthviewModel(ILogger<MonthviewModel> logger,MigHelperCtx context)
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet()
    {
        Res = _context.GccNames.Where(x => x.Migrated == false).GroupBy(x => x.Month);
    }
}
