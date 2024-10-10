using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigrationHelper.Models;
using MigrationHelper.Db;
using MigrationHelper.Helpers;
using MigrationHelper.BL;
using System.Diagnostics;

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
        if (day == p.CutOff.Day - 1 || day == p.CutOff.Day - 2) return "Blackout";
        if (day == p.PayDate.Day) return "Paydate";
        if (day == p.CutOff.Day) return "CutOff";
        // if (day == p.PCStartDate.Day) return "PYstart";
        //  if (day == p.PCEndDate.Day) return "PYend";
        //      if (day == p.PayStartDate.Day) return "PreviewPayroll";
        //     if (day == p.PayEndDate.Day) return "PreviewCutOff";
        if (day == p.Payslip.Day) return "Closed";
        if (day >= p.CutOff.Day && day <= p.PayDate.Day) return "Closed";
        if (day <= p.QueueOpen.Day && p.QueueOpen > p.PCEndDate && p.Frequency == "monthly")
        {
            return "Closed";
        }



        return "Open";

        //  if (day > p.Open.Day) return "Open";
        //  return "Closed";
    }

    public List<PGD> pgd = [];

    public async void OnGet(string gcc, int year, int month)
    {
        Gcc = _context.GccNames.Where(x => x.Gcc == gcc).First(); ;
        Month = month;
        Year = year;
        PGDetails pd = new(gcc, year, month);

        Cal = new Calendar(year, month).Days;
        var results = await pd.GetDetails();
        results = results.OrderBy(x => x.Lcc).ToList();
        foreach (var x in results)
        {
            var res = pgd.Where(y => y.Lcc == x.Lcc && y.PayGroup == x.PayGroup);
            if (!res.Any())
            {
                var copiedCal = new Calendar(year, month).Days;
                foreach (var day in copiedCal)
                {
                    day.Value.Color = FormatCell(day.Key, x);
                }

                pgd.Add(new PGD { Lcc = x.Lcc, PayGroup = x.PayGroup, Frequency = x.Frequency, calDays = copiedCal });
            }
            else
            {
                var first = res.First();
                foreach (var day in first.calDays)
                {
                    if (day.Value.Color == "Open" || day.Value.Color == "Closed")
                    {
                        day.Value.Color = FormatCell(day.Key, x);
                    }
                }
            }
        };
        Debug.Assert(pgd.Count >= Gcc.LCCCount, $"{pgd.Count}  != {Gcc.LCCCount}");
        FormattedMonth = Toolbox.MonthToName(year, month);
        var c = pd.GetCalendar();
    }
}
