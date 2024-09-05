using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Helpers.Init;

namespace MigrationHelper.Pages.Config;

public class LoadGCCModel : PageModel
{
    private readonly ILogger<LoadGCCModel> _logger;


    public int DbCount {get;set;}    

    public LoadGCCModel(ILogger<LoadGCCModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        DBLoader db = new();
        DbCount = db.GccLoader();
      

    }
 
}
