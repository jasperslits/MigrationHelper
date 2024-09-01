using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Helpers.Init;

namespace MigrationHelper.Pages.BL;

public class LoadModel : PageModel
{
    private readonly ILogger<LoadModel> _logger;


    public int DbCount {get;set;}    

    public LoadModel(ILogger<LoadModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        DBLoader db = new DBLoader();
        DbCount = db.Rc;

    }
 
}
