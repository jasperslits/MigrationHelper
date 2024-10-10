using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.Db;
using MigrationHelper.BL;

namespace MigrationHelper.Pages;

public class GccViewModel : PageModel
{
    private readonly ILogger<GccViewModel> _logger;

    public OutputHelper oh {get;set;}

    public GccNames Gcc { get; set; }

    public int Month { get;set; } = 1 ;
     public int Year { get;set; } = 1 ;

    public string MonthName { get; set;}

     private readonly MigHelperCtx _context;

     public Dictionary<string,int> keyValuePairs= [];

    public GccViewModel(ILogger<GccViewModel> logger,MigHelperCtx context)
    {
        _logger = logger;
        _context  = context;

    }

    public string ScoreColor(int score) {
        if (score == 0) return "red";
        if (score == 100) return "green";
        if (score > 80) {
            return "lightgreen";
        }  else {
            return "amber";
        }
    }




    public void OnGet(string gcc, int year, int month)
    {
        Month = month;
        Year = year;
        ScoreConfig scoreConfiguration = _context.ScoreConfig.First();
        ScoreHelper h = new(scoreConfiguration,gcc,year,month);
        
        keyValuePairs.Add("Saturday",scoreConfiguration.Saturday);
        keyValuePairs.Add("Sunday",scoreConfiguration.Sunday);
        keyValuePairs.Add("Cut-off",scoreConfiguration.CutOff);
        keyValuePairs.Add("Cut-off -1",scoreConfiguration.CutOffBlackout);
        keyValuePairs.Add("Cut-off -2",scoreConfiguration.CutOffBlackout);
        keyValuePairs.Add("Pay date",scoreConfiguration.PayDate);
        keyValuePairs.Add("Pay date +1",scoreConfiguration.NextPayDate);
        keyValuePairs.Add("Between cutoff and queue open",scoreConfiguration.BlockedAfterClose);
        keyValuePairs.Add("Free slot before cut-off",scoreConfiguration.Free);
        keyValuePairs.Add("Free slot after cut-off",scoreConfiguration.FreeAfterClose);

        oh = new(h.CalendarDays);
  
        MonthName = Helpers.Toolbox.MonthToName(year,month);
        Gcc = _context.GccNames.Where(x => x.Gcc == gcc).First();
        
        
     //   Month = res[

    }
}
