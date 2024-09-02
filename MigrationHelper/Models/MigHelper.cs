namespace MigrationHelper.Models;

using MigrationHelper.Db;

public class MigHelper
{
    public List<PayPeriodGcc> pg = new List<PayPeriodGcc>();

    public MigHelper()
    {

    }

    public List<int> GetMonthNrs() {
        return new List<int> { 9,10,11,12 }; 
    }

    public List<string> GetMonthNames() {
        var rv = new List<string>();
        foreach(var y in GetMonthNrs()) {
            rv.Add(MigrationHelper.Models.Toolbox.MonthToName(y));
        } 

        return rv;
    }

    public List<string> GetGCCs2()
    {
        var context = new MigHelperCtx();

        var results = context.PayPeriods.Select(x => x.Gcc).Distinct().ToList();
        results.Sort();
        return results;
    }

       public Dictionary<string,string> GetGCCNames()
    {
        var context = new MigHelperCtx();
        Dictionary<string,string> rv = new();

        var results = context.GccNames.OrderBy(x => x.Gcc).ToList();
        // results.Sort();
        foreach(var r in results) {
            rv.Add(r.Gcc,r.Name);
        } 
        return rv;
    }


    public void LoadData(string Gcc, int month)
    {

        var context = new MigHelperCtx();

        DateTime pStart = new DateTime(2024, month, 1, 0, 0, 0);
        DateTime pEnd = new DateTime(2024, month, DateTime.DaysInMonth(2024, month), 0, 0, 0);
        var b = context.PayPeriods.Where(x => x.Gcc == Gcc && x.CutOff >= pStart &&  x.CutOff <= pEnd).ToList();


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