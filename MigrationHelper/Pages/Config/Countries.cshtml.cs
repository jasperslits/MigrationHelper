using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Helpers.Init;

namespace MigrationHelper.Pages.Config;

public class CountriesModel : PageModel
{
    private readonly ILogger<CountriesModel> _logger;


    public int DbCount {get;set;}    

    public CountriesModel(ILogger<CountriesModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        DBLoader db = new();
        DbCount = db.CountryLoader();
      

    }
 
}
