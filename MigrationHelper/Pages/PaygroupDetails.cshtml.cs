using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.Db;
using MigrationHelper.Helpers;
using MigrationHelper.BL;

namespace MigrationHelper.Pages;



public class PayGroupDetails : PageModel
{
    private readonly ILogger<PayGroupDetails> _logger;
    private readonly MigHelperCtx _context;


    public Dictionary<int, CalDay> Cal { get; set; }


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
         if (day == p.CutOff.Day-1 || day == p.CutOff.Day-2) return "Blackout";
        if (day == p.PayDate.Day) return "Paydate";
        if (day == p.CutOff.Day) return "CutOff";
        if (day == p.PCStartDate.Day) return "PYstart";
        if (day == p.PCEndDate.Day) return "PYend";
        if (day == p.PayStartDate.Day) return "PreviewPayroll";
        if (day == p.PayEndDate.Day) return "PreviewCutOff";
        if (day >= p.CutOff.Day && day <= p.QueueOpen.Day) return "Closed";
        return "Open";
      
      //  if (day > p.Open.Day) return "Open";
      //  return "Closed";
    }

    public List<PGD> pgd = new();

    public void OnGet(string gcc, int year, int month)
    {
        Gcc = _context.GccNames.Where(x => x.Gcc == gcc).First(); ;
        Month = month;
        Year = year;
        PGDetails Pd = new(gcc, year, month);
        //  Helper h = new(gcc,month);
        Cal = new Calendar(year, month).Days;
        var Results = Pd.GetDetails();
        foreach(var x in Results) {
            var res = pgd.Where( y => y.Lcc == x.Lcc && y.PayGroup == x.PayGroup);
            if (res.Count() == 0) {
                var copiedCal = new Calendar(year, month).Days;
                foreach(var day in copiedCal) {
                    day.Value.Color = FormatCell(day.Key,x);
                    Console.WriteLine($"Format {x.Lcc} and pg = {x.PayGroup}, day {day.Key} and color = {day.Value.Color}");
                }    

                pgd.Add( new PGD { Lcc = x.Lcc, PayGroup = x.PayGroup, calDays = copiedCal});
            } else {
                var first = res.First();
                foreach(var day in first.calDays) {
                    Console.WriteLine($"Before:: Format {x.Lcc} and pg = {x.PayGroup}, day {day.Key} and color = {day.Value.Color}");
                    if (day.Value.Color == "Open" || day.Value.Color == "Closed") {
                        day.Value.Color = FormatCell(day.Key,x);
                    }
                    Console.WriteLine($"After:: Format {x.Lcc} and pg = {x.PayGroup}, day {day.Key} and color = {day.Value.Color}");
                }     

                Console.WriteLine($"Found {x.Lcc} and pg = {x.PayGroup}");
            }
        };

        FormattedMonth = Toolbox.MonthToName(year,month);
        var c = Pd.GetCalendar();
    }
}
