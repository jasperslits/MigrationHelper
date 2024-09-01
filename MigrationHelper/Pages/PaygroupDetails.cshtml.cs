using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;

namespace MigrationHelper.Pages;

public class PayGroupDetails : PageModel
{
    private readonly ILogger<PayGroupDetails> _logger;


    public Dictionary<int,CalDay> Cal {get;set;}

   public List<PayPeriod> Results { get; set; }

   

    public string Gcc { get; set; } = "Dummy"; 

    public int Month { get;set; } = 1 ;

    

    public PayGroupDetails(ILogger<PayGroupDetails> logger)
    {
        _logger = logger;
    }

    public void OnGet(string gcc, int month)
    {
           Gcc = gcc;
        Month = month;
       PGDetails Pd = new PGDetails(gcc,month);
       Results = Pd.GetDetails();
       var c = Pd.GetCalendar();
        

    }
}
