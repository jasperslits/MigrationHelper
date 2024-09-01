using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;

namespace MigrationHelper.Pages;

public class DetailModel : PageModel
{
    private readonly ILogger<DetailModel> _logger;

    public List<string> Details { get; set;}

    [BindProperty]
    public ScoreConfiguration sc {get;set;}

    public string Gcc { get; set; } = "Dummy"; 

    public int Month { get;set; } = 1 ;

    public int Day { get; set; }

    public DetailModel(ILogger<DetailModel> logger)
    {
        _logger = logger;
    }

    public void OnGet(string gcc, int month, int day)
    {
        Helper h = new(gcc,month);
         sc = new ScoreConfiguration();
        var res = h.c;
        Gcc = gcc;
        Month = month;
        Day = day;
        var f = res.Where(x => x.Key == day).Select(x => x.Value.Details).ToList();
        f[0].Sort();
        Details = f[0];
   
    }
}
