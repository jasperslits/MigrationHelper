using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;

namespace MigrationHelper.Pages;

public class PayGroupDetails : PageModel
{
    private readonly ILogger<PayGroupDetails> _logger;


    public Dictionary<int,CalDay> Cal {get;set;}

   public List<PayPeriod> Results { get; set; }

    public string FormattedMonth { get; set; } = "";

    public string Gcc { get; set; } = "Dummy"; 

    public int Month { get;set; } = 1 ;

    

    public PayGroupDetails(ILogger<PayGroupDetails> logger)
    {
        _logger = logger;
    }

    public string FormatCell(int day,PayPeriod p)
    {
    
        if (day == p.PayDate.Day) return "Paydate";
        if (day == p.CutOff.Day) return "CutOff";
        if (p.Open.Day >= day && day <= p.CutOff.Day) return "Open";
        if (day > p.Open.Day) return "Open";
        return "Closed";
    }

    public void OnGet(string gcc, int month)
    {
           Gcc = gcc;
        Month = month;
       PGDetails Pd = new PGDetails(gcc,month);
      //  Helper h = new(gcc,month);
       Cal = new Calendar(2024,month).Days;
       Results = Pd.GetDetails();
       FormattedMonth = Toolbox.MonthToName(month);
       var c = Pd.GetCalendar();
        

    }
}
