using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;

namespace MigrationHelper.Pages.BL;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public OutputHelper oh {get;set;}

    [BindProperty]
    public ScoreConfiguration sc {get;set;}

    public Dictionary<int,CalDay> output {get;set;}

    public string Gcc { get; set; } = "Dummy"; 

    public int Month { get;set; } = 1 ;

    

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet(string gcc, int month)
    {
        Helper h = new(gcc,month);
       
         sc = new ScoreConfiguration();
        // var res = h.c;
        oh = new(h.c);
        Gcc = gcc;
        Month = month;
     //   Month = res[

    }
}
