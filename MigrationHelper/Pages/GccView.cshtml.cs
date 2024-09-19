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

     public Dictionary<string,int> keyValuePairs= new Dictionary<string,int>();

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
        ScoreHelper h = new(gcc,year,month);
        
        keyValuePairs.Add("Weekend",(int)ScoreConfiguration.Weekend);
        keyValuePairs.Add("Cut-off",(int)ScoreConfiguration.CutOff);
        keyValuePairs.Add("Cut-off -1",(int)ScoreConfiguration.CutOffBlackout);
        keyValuePairs.Add("Cut-off -2",(int)ScoreConfiguration.CutOffBlackout);
        keyValuePairs.Add("Pay date",(int)ScoreConfiguration.PayDate);
        keyValuePairs.Add("Pay date +1",(int)ScoreConfiguration.NextPayDate);
        keyValuePairs.Add("Between cutoff and queue open",(int)ScoreConfiguration.BlockedAfterClose);
        keyValuePairs.Add("Free slot before cut-off",(int)ScoreConfiguration.Free);
        keyValuePairs.Add("Free slot after cut-off",(int)ScoreConfiguration.FreeAfterClose);

        oh = new(h.c);
  
        MonthName = Helpers.Toolbox.MonthToName(year,month);
        Gcc = _context.GccNames.Where(x => x.Gcc == gcc).First();
        
        
     //   Month = res[

    }
}
