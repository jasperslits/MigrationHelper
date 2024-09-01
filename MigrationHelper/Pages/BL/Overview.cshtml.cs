using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;

namespace MigrationHelper.Pages.BL;

public class OverviewModel : PageModel
{
    private readonly ILogger<OverviewModel> _logger;


    public List<string> Gccs { get; set; }
    public List<string> Months { get; set; }

    public OverviewModel(ILogger<OverviewModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        MigHelper h = new MigHelper();
        Months = new List<string> { "8","9","10","11","12" }; 
        Gccs = h.GetGCCs();
    }
}
