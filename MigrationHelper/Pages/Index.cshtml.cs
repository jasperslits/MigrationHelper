using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;

namespace MigrationHelper.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;


    public List<string> Gccs { get; set; }
    public List<string> Months { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
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
