using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Helpers.Init;

namespace MigrationHelper.Pages.Config;

public class LoadPGModel : PageModel
{
    private readonly ILogger<LoadPGModel> _logger;


    public int DbCount {get;set;}    

    public LoadPGModel(ILogger<LoadPGModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        DBLoader db = new DBLoader();
        DbCount = db.PGLoader();
        

    }
 
}
