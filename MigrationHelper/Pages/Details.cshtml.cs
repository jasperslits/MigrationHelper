using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.BL;
using MigrationHelper.Db;

namespace MigrationHelper.Pages;

public class DetailModel : PageModel
{
    private readonly ILogger<DetailModel> _logger;
    private readonly MigHelperCtx _context;

    public List<ScoreBreakdownMessage> Details { get; set;}

    [BindProperty]
    public ScoreConfig sc {get;set;}

    public GccNames Gcc { get; set; }

    public int Month { get;set; } = 1 ;

    public int Day { get; set; }

    public int Year { get; set; }

    public string FormattedDay { get; set; } = "";

    public DetailModel(ILogger<DetailModel> logger,MigHelperCtx context)
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet(string gcc, int year, int month, int day)
    {
        sc = _context.ScoreConfig.First();
        ScoreHelper h = new(sc,gcc,year,month);
        
        var res = h.CalendarDays;
        Gcc = _context.GccNames.Where(x => x.Gcc == gcc).Single();
        Month = month;
        Day = day;
        Year = year;
        FormattedDay = Helpers.Toolbox.DayToName(year,month,day);
        Details = _context.ScoreBreakdown.
        Where(x => x.Gcc == gcc && x.Year == Year && x.Month == Month && x.Day == Day).OrderBy( x=> x.Message).
        Select(x => new ScoreBreakdownMessage { Message = x.Message, Sc = x.Sc }).ToList();
   
    }
}
