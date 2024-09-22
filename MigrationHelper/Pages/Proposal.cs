using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.Helpers;
using MigrationHelper.Db;
using MigrationHelper.BL;
using Microsoft.EntityFrameworkCore;

namespace MigrationHelper.Pages;



public class ProposalModel : PageModel
{
    private readonly ILogger<ProposalModel> _logger;

    private readonly MigHelperCtx _context;

    private List<MonthCache> _cache;

    private List<ScoreCache> _scores;
    public List<GccNames> Gccs { get; set; }
    public List<Periods> Periods { get; set; }

    private string _DayTwo {get; set;}

    public ProposalModel(ILogger<ProposalModel> logger,MigHelperCtx context)
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

    public string Days(string gcc,int year,int month) 
    {
        string daytwo = "";
        Dictionary<int,int> dayx = new();
      
        var res = _scores.Where(x => x.Gcc == gcc && x.Month == month && x.Year == year && x.Percentage > 0).Select(t => new { t.Day, t.Percentage} ).ToDictionary( t => t.Day,t => t);
        if (res.Count == 0) {
            return "No data";
        } else {

            foreach(var q in res) {
                if (res.ContainsKey(q.Key+1)) {
                    dayx[q.Key] = q.Value.Percentage + res[q.Key + 1].Percentage;
                }
            }
                
        }
        var top = dayx.OrderByDescending(x => x.Value).First().Key;
 
        int dayOnePercentage = res[top].Percentage;
        int dayTwoPercentage = res[top+1].Percentage;
        return $"{top} ({dayOnePercentage}%)<br/>{top+1} ({dayTwoPercentage})%";


       //     var nextday = _scores.Where(x => x.Gcc == gcc && x.Month == month && x.Year == year && x.Day== res.Day+1).OrderByDescending(x => x.Score).FirstOrDefault();
       //     if (nextday == null) { daytwo = "No data";} else { 
       //         daytwo = $"{nextday.Day} ({nextday.Percentage}%)";
       //     }
       //     return $"{res.Day} ({res.Percentage}%)<br/>{daytwo}";
      
        }
      
    
 

    public async Task OnGet()
    {
        Periods = new();
        var currentmonth = DateTime.Now;

        _cache = _context.PayPeriods.Select(a => new MonthCache { Gcc = a.Gcc, Month = a.CutOff.Month}).Distinct().ToList();
        _scores = await _context.ScoreCache.ToListAsync();
        var m = new DateTime(2025,3,1);
        for(int i = 0; i < 8;i++) {

            Periods.Add(new Periods { Year = currentmonth.Year, Month = currentmonth.Month, MonthName = currentmonth.ToString("MMMM yyyy") });
            if (currentmonth.Month == m.Month && currentmonth.Year == m.Year) {
                break;
            }
            currentmonth = currentmonth.AddMonths(1);

        }
     
     
   
        Gccs = _context.GccNames.Where(x => x.Countrycount > 0).OrderBy(x => x.Gcc).ToList();
    }
}
