using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.Helpers;
using MigrationHelper.Db;
using MigrationHelper.BL;
using Microsoft.EntityFrameworkCore;

namespace MigrationHelper.Pages;



public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly MigHelperCtx _context;

    private List<MonthCache> _cache;
    public List<GccNames> Gccs { get; set; }
    public List<Periods> Periods { get; set; }

    public IndexModel(ILogger<IndexModel> logger,MigHelperCtx context)
    {
        _logger = logger;
        _context = context;
    }

    public string HasDataInMonth(string gcc,int year,int migmonth, int currentmonth) {

        if (migmonth == currentmonth) return "mig"; 

        if (year == 2024) return "nomig";

        if (_cache.Where(x => x.Gcc == gcc && x.Month == currentmonth).Any()) {
            return "nomig"; 
        } else {
            return "nodata";
        }
    }

 

    public async Task OnGet()
    {
        Periods = [];
        var currentmonth = DateTime.Now.AddDays(2);

        _cache =  await _context.PayPeriods.Select(a => new MonthCache { Gcc = a.Gcc, Month = a.CutOff.Month}).Distinct().ToListAsync();

        var m = new DateTime(2025,6,1);
        for(int i = 0; i < 8;i++) {

            Periods.Add(new Periods(currentmonth.ToString("MMMM yyyy")) { Year = currentmonth.Year, Month = currentmonth.Month });
            if (currentmonth.Month == m.Month && currentmonth.Year == m.Year) {
                break;
            }
            currentmonth = currentmonth.AddMonths(1);

        }
     
     
   
        Gccs = await _context.GccNames.OrderBy(x => x.Gcc).Where(x => x.Countrycount > 0).ToListAsync();
    }
}
