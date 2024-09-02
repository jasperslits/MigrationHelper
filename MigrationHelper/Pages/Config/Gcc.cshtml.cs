using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;

namespace MigrationHelper.Pages.Config;

public class GccModel : PageModel
{
    private readonly ILogger<GccModel> _logger;
    public GccModel(ILogger<GccModel> logger)
    {
        _logger = logger;
    }

 

    public void OnGet()
    {
       
    }
}
