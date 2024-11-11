using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.Helpers;
using MigrationHelper.Db;
using MigrationHelper.BL;
using Microsoft.EntityFrameworkCore;

namespace MigrationHelper.Pages;



public class ProposalModel(ILogger<ProposalModel> logger, MigHelperCtx context) : PageModel
{
    private readonly ILogger<ProposalModel> _logger = logger;

    private readonly MigHelperCtx _context = context;

    private List<ScoreCache> _scores;
    public List<GccNames> Gccs { get; set; }
    public List<Periods> Periods { get; set; }

    public string Days(string gcc,int year,int month) 
    {
        Dictionary<int,int> dayx = [];
      
        var res2 = _scores.Where(x => x.Gcc == gcc && x.Month == month && x.Year == year).ToList();
        if (!res2.Any()) {
            return "No data";
        }
        var res = res2.Where(x => x.Percentage > 0).Select(t => new { t.Day, t.Percentage} ).ToDictionary( t => t.Day,t => t.Percentage);
        if (res.Count == 0) {
            return "No OK scores";
        } else {

            foreach(var q in res) {
                if (res.ContainsKey(q.Key+1)) {
                    dayx[q.Key] = q.Value + res[q.Key + 1];
                }
            }
                
        }
        if (dayx.Count == 0) {
            return "No OK dates";
        }
        int top = dayx.OrderByDescending(x => x.Value).First().Key;
        int dayOnePercentage = res[top];
        int dayTwoPercentage = res[top+1];
        return $"{top} ({dayOnePercentage}%)<br/>{top+1} ({dayTwoPercentage})%";
    }
      
    
 

    public async Task OnGet()
    {
        Periods = [];
        var currentmonth = DateTime.Now.AddDays(2);

        _scores = await _context.ScoreCache.Where(x => x.Score > 0).ToListAsync();
        var m = new DateTime(2025,6,1);
        for(int i = 0; i < 8;i++) {

            Periods.Add(new Periods(currentmonth.ToString("MMMM yyyy")) { Year = currentmonth.Year, Month = currentmonth.Month });
            if (currentmonth.Month == m.Month && currentmonth.Year == m.Year) {
                break;
            }
            currentmonth = currentmonth.AddMonths(1);

        }
   
        Gccs = await _context.GccNames.Where(x => x.Countrycount > 0).OrderBy(x => x.Gcc).ToListAsync();
    }
}
