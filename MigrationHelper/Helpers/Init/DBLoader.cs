namespace MigrationHelper.Helpers.Init;
using MigrationHelper.Db;
using MigrationHelper.Models;
using MigrationHelper.BL;
using Microsoft.EntityFrameworkCore;

public class DBLoader
{

    public MigHelperCtx Context { get; set; }
    private CSVMigHelper Cmh { get; set; }

    public DBLoader()
    {
        Context = new MigHelperCtx();
        Cmh = new();
    }

    public async Task<int> GccLoader()
    {
        
        Context.GccNames.RemoveRange(Context.GccNames);
        Context.SaveChanges();

        var results = Cmh.ReadGcc();

        await Context.AddRangeAsync(results);
        await Context.SaveChangesAsync();
        return results.Count;
    }

    public async Task<int> CountryLoader()
    {
        Context.Countries.RemoveRange(Context.Countries);
        await Context.SaveChangesAsync();

        var results = Cmh.ReadCountries("src/Data/countries.csv");

        await Context.AddRangeAsync(results);
        await Context.SaveChangesAsync();
        return results.Count;
    }

    public async Task<int> PGCacheLoader()
    {
        var currentmonth = DateTime.Now;
        var sc = Context.ScoreConfig.First();
        var m = new DateTime(2025, 3, 1);
        List<String> g = await Context.GccNames.Select(x => x.Gcc).ToListAsync();
        for (int i = 0; i < 8; i++)
        {
            if (currentmonth.Month == m.Month && currentmonth.Year == m.Year)
            {
                break;
            }

            int year = currentmonth.Year;
            int month = currentmonth.Month;
            foreach (string gcc in g)
            {
                ScoreHelper sh = new(sc, gcc, year, month);
                MigHelper mh = new();
                await mh.LoadData(gcc, year, month);
                mh.PeriodsToGcc();
                List<PayPeriodGcc> pg = mh.pg;
                sh.FillCalendar(pg);
                sh.Sch.AddCache(sh.CalendarDays);
            }
            currentmonth = currentmonth.AddMonths(1);
        }

        return 0;
    }

    public async Task<int> PGLoader()
    {
        CSVMigHelper c = new();
        string targetDirectory = "src/Data/Raw";
        string[] fileEntries = Directory.GetFiles(targetDirectory, "*.csv");
        int total = 0;
        DateTime ts = new(1981, 01, 01);
        foreach (string fileName in fileEntries)
        {

            var results = await c.ReadPg(fileName);
            results = results.Where(x => x.Close > ts && x.CutOff > ts && x.Open > ts && x.PayDate > ts).ToList();
            await Context.AddRangeAsync(results);
            total += results.Count;

        }
        await Context.SaveChangesAsync();

        var cccs = Context.GccNames;
        foreach (var gcc in cccs)
        {

            gcc.LCCCount = Context.PayPeriods.Where(x => x.Gcc == gcc.Gcc).Select(x => x.Lcc).Distinct().Count();
            gcc.PGCount = Context.PayPeriods.Where(x => x.Gcc == gcc.Gcc).Select(x => x.PayGroup).Distinct().Count();
            gcc.Countrycount = Context.PayPeriods.Where(x => x.Gcc == gcc.Gcc).Select(x => x.Lcc.Substring(0, 2)).Distinct().Count();
        }
    
        await Context.SaveChangesAsync();
        await PGCacheLoader();
        return total;

    }
}