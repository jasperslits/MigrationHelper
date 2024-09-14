using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.Db;

namespace MigrationHelper.Pages;

public class PayGroupDetails : PageModel
{
    private readonly ILogger<PayGroupDetails> _logger;
    private readonly MigHelperCtx _context;


    public Dictionary<int, CalDay> Cal { get; set; }

    public List<PayPeriod> Results { get; set; }

    public string FormattedMonth { get; set; } = "";

    public GccNames Gcc { get; set; }

    public int Month { get; set; } = 1;
    public int Year { get; set; } = 1;


    public PayGroupDetails(ILogger<PayGroupDetails> logger, MigHelperCtx context)
    {
        _logger = logger;
        _context = context;
    }

    public string FormatCell(int day, PayPeriod p)
    {
        if (day == p.PayDate.Day) return "Paydate";
        if (day == p.CutOff.Day) return "CutOff";
        if (p.Open.Day >= day && day <= p.CutOff.Day) return "Open";
        if (day > p.Open.Day) return "Open";
        return "Closed";
    }

    public void OnGet(string gcc, int year, int month)
    {
        Gcc = _context.GccNames.Where(x => x.Gcc == gcc).First(); ;
        Month = month;
        Year = year;
        PGDetails Pd = new PGDetails(gcc, year, month);
        //  Helper h = new(gcc,month);
        Cal = new Calendar(year, month).Days;
        Results = Pd.GetDetails();
        FormattedMonth = Toolbox.MonthToName(month);
        var c = Pd.GetCalendar();
    }
}
