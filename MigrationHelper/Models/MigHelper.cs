namespace MigrationHelper.Models;

using MigrationHelper.Db;

public class MigHelper
{
    public List<PayPeriodGcc> pg = new List<PayPeriodGcc>();

    public MigHelper()
    {

    }

    public List<string> GetGCCs()
    {
        var context = new MigHelperCtx();

        var results = context.PayPeriods.Select(x => x.Gcc).Distinct().ToList();
        results.Sort();
        return results;
    }


    public void LoadData(string Gcc, int month)
    {

        var context = new MigHelperCtx();

        DateTime pStart = new DateTime(2024, month, 1, 0, 0, 0);
        DateTime pEnd = new DateTime(2024, month, DateTime.DaysInMonth(2024, month), 0, 0, 0);
        var b = context.PayPeriods.Where(x => x.Gcc == Gcc && pStart >= x.Open).ToList();
       // Console.WriteLine($"Found PayPeriods {b.Count} records");

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