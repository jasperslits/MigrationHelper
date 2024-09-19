using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.Helpers;
using MigrationHelper.Db;
using MigrationHelper.BL;

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

    public string CheckMonth(string gcc,int year,int month, int currentmonth) {

        if (month == currentmonth) { return "mig"; }

        if (year == 2024) return "nomig"; 

        var results = _cache.Where(x => x.Gcc == gcc && x.Month == month).Count();

        if (results == 0) { return "nodata"; } else { return "nomig"; };
    }

    public Dictionary<string,int> CountryCount { get; set; }

    public void OnGet()
    {
        Periods = new List<Periods>();
        var currentmonth = DateTime.Now;

        _cache = _context.PayPeriods.Select(a => new MonthCache { Gcc = a.Gcc, Month = a.CutOff.Month}).Distinct().ToList();

        var m = new DateTime(2025,3,1);
        for(int i = 0; i < 8;i++) {

            Periods.Add(new Periods { Year = currentmonth.Year, Month = currentmonth.Month, MonthName = currentmonth.ToString("MMMM yyyy") });
            if (currentmonth.Month == m.Month && currentmonth.Year == m.Year) {
                break;
            }
            currentmonth = currentmonth.AddMonths(1);

        }
        MigHelper h = new();
        CountryCount = new();
        var res = _context.MigStats.ToList();
        var x = res.Select(x => x.Gcc).ToList();
        foreach(var m2 in res) {
            CountryCount.Add(m2.Gcc,m2.Countrycount);
        }
        Gccs = h.GetGCCNames();
        Gccs = Gccs.Where( y => x.Contains(y.Gcc) ).ToList();
    }
}
