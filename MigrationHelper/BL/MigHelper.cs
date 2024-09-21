namespace MigrationHelper.BL;

using MigrationHelper.Models;

using MigrationHelper.Db;

public class MigHelper
{
    public List<PayPeriodGcc> pg = [];
    public List<PayPeriod> pp = [];

    private MigHelperCtx _context;

    public MigHelper()
    {
        _context = new MigHelperCtx();
    }

    public List<GccNames> GetGCCNames()
    {
        return _context.GccNames.OrderBy(x => x.Gcc).ToList();
    }

    public void PeriodsToGcc() {
          List<PayPeriodGcc> x = pp.DistinctBy(x => x.PayGroup).Select(o => new PayPeriodGcc
        {
            Gcc = o.Gcc,
            PayGroup = o.PayGroup,
            Close = o.Close,
            PayDate = o.PayDate,
            Open = o.Open,
            CutOff = o.CutOff,
            PayEndDate = o.PayEndDate,
            PayStartDate = o.PayStartDate,
            PCEndDate = o.PCEndDate,
            PCStartDate = o.PCStartDate,
            Payslip = o.Payslip,
            Number = o.Number,
            Frequency = o.Frequency,
            QueueOpen = o.QueueOpen,
            Offcycle = o.Offcycle
                
        }).ToList();
        pg = x;
    }

    public void LoadData(string Gcc, int year, int month)
    {
        DateTime pStart = new (year, month, 1, 0, 0, 0);
        DateTime pEnd = new (year, month, DateTime.DaysInMonth(year, month), 0, 0, 0);
        pp = _context.PayPeriods.Where(x => x.Gcc == Gcc && x.CutOff >= pStart && x.CutOff <= pEnd).ToList();

// Console.WriteLine($"Found PayPeriodGcc {x.Count} records");
    }
}