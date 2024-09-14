using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MigrationHelper.Db;
using MigrationHelper.Models;

namespace MigrationHelper.Pages;

public class GccDetailModel : PageModel
{
    private readonly ILogger<GccDetailModel> _logger;


    [BindProperty]
    public   GccNames Gcc { get; set; }
    private readonly MigHelperCtx _context;
      public MigStats Stats { get; set; }

    public GccDetailModel(ILogger<GccDetailModel> logger,MigHelperCtx context)
    {
        _context = context;
        _logger = logger;
    }

   public List<SelectListItem> YearsSL { get; set; }
public List<SelectListItem> MonthsSL { get; set; }
    
     public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Gcc != null) _context.Attach(Gcc).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    public void OnGet(string gcc)
    {
     
       Gcc = _context.GccNames.FirstOrDefault(g => g.Gcc == gcc);
       Stats = new MigHelper().GetStats(gcc);

       MonthsSL = new List<SelectListItem>() { new SelectListItem { Text = "Select month", Value = "0"} };
       string[] names = new System.Globalization.DateTimeFormatInfo().MonthNames;

       foreach(var name in names) {
        int monthnr = Array.IndexOf(names,name)+1;
        MonthsSL.Add(new SelectListItem { Text = name, Value = monthnr.ToString(), Selected = monthnr == Gcc.Month });
       }

    
     YearsSL = new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = "2024",
                Value = "2024",
                Selected = 2024 == Gcc.Year
            },
            new SelectListItem
            {
                Text = "2025",
                Value = "2025",
                Selected = 2025 == Gcc.Year
            }
        };
       

    }
}
