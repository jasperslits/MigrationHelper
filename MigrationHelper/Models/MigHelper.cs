namespace MigrationHelper.Models;

using MigrationHelper.Db;

public class MigHelper
{
    public List<PayPeriodGcc> pg = [];

    private MigHelperCtx _context;

    public MigHelper()
    {
        _context = new MigHelperCtx();
    }

    public MigStats GetStats(string gcc)
    {
        MigStats migStats = new()
        {
            LCCCount = _context.PayPeriods.Where(x => x.Gcc == gcc).Select(x => x.Lcc).Distinct().Count(),
            PGCount = _context.PayPeriods.Where(x => x.Gcc == gcc).Select(x => x.PayGroup).Distinct().Count(),
            Countrycount = _context.PayPeriods.Where(x => x.Gcc == gcc).Select(x => x.Lcc.Substring(0, 2)).Distinct().Count()
        };
        return migStats;
    }

    public List<int> GetMonthNrs()
    {
        return [9, 10, 11, 12];
    }

    public List<string> GetMonthNames()
    {
        var rv = new List<string>();
        foreach (var y in GetMonthNrs())
        {
            rv.Add(Toolbox.MonthToName(y));
        }

        return rv;
    }

    public List<GccNames> GetGCCNames()
    {
        return _context.GccNames.OrderBy(x => x.Gcc).ToList();
    }


    public void LoadData(string Gcc, int year, int month)
    {
        DateTime pStart = new DateTime(year, month, 1, 0, 0, 0);
        DateTime pEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month), 0, 0, 0);
        var b = _context.PayPeriods.Where(x => x.Gcc == Gcc && x.CutOff >= pStart && x.CutOff <= pEnd).ToList();


        List<PayPeriodGcc> x = b.DistinctBy(x => x.PayGroup).Select(o => new PayPeriodGcc
        {
            Gcc = o.Gcc,
            PayGroup = o.PayGroup,
            Close = o.Close,
            PayDate = o.PayDate,
            Open = o.Open,
            CutOff = o.CutOff
        }).ToList();
        pg = x;
        // Console.WriteLine($"Found PayPeriodGcc {x.Count} records");
    }
}