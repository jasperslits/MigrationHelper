using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;

namespace MigrationHelper.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;


    public Dictionary<string,string> Gccs { get; set; }
    public List<string> MonthNames { get; set; }
    public List<int> MonthNrs { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

 

    public void OnGet()
    {
        MigHelper h = new MigHelper();
        MonthNames = h.GetMonthNames();
        MonthNrs = h.GetMonthNrs();
        Gccs = h.GetGCCNames();
    }
}
