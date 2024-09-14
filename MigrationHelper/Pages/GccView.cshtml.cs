using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.Db;

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
        Helper h = new(gcc,year,month);
        
        keyValuePairs.Add("Weekend",-1*(int)ScoreConfiguration.Weekend);
        keyValuePairs.Add("Cut-off",-1*(int)ScoreConfiguration.CutOff);
        keyValuePairs.Add("Pay date",-1*(int)ScoreConfiguration.PayDate);
        keyValuePairs.Add("Pay date +1",-1*(int)ScoreConfiguration.NextPayDate);
        keyValuePairs.Add("Free slot before cut-off",(int)ScoreConfiguration.Free);
        keyValuePairs.Add("Free slot after cut-off",(int)ScoreConfiguration.FreeAfterClose);
        oh = new(h.c);
        Month = month;
        Year = year;
        MonthName = Toolbox.MonthToName(month);
        Gcc = _context.GccNames.Where(x => x.Gcc == gcc).First();
        
        
     //   Month = res[

    }
}
