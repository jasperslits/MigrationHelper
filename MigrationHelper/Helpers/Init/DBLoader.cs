namespace MigrationHelper.Helpers.Init;
using MigrationHelper.Db;
using MigrationHelper.Models;
using MigrationHelper.BL;

public class DBLoader
{

    public MigHelperCtx Context { get;set;}

    public DBLoader() {
        Context = new MigHelperCtx();
    }

    public int GccLoader() {
        CSVMigHelper c = new();
        Context.GccNames.RemoveRange(Context.GccNames);
        Context.SaveChanges();
 
        var results = c.ReadGcc();
   
        Context.AddRange(results);
        Context.SaveChanges();
        return results.Count;
    }

     public int CountryLoader() {
        CSVCountries c = new();
       
        Context.Countries.RemoveRange(Context.Countries);
        Context.SaveChanges();
 
        var results = c.ReadCountries("src/Data/countries.csv");
   
        Context.AddRange(results);
        Context.SaveChanges();
        return results.Count;
    }

    private async Task<int> PGCacheLoader() {
     //   DateTime dt = DateTime.Now;

        var currentmonth = DateTime.Now;
        var m = new DateTime(2025,3,1);
        List<GccNames> g = Context.GccNames.ToList();
        for(int i = 0; i < 8;i++) {
            if (currentmonth.Month == m.Month && currentmonth.Year == m.Year) {
                break;
            }
            
            int year = currentmonth.Year;
            int month = currentmonth.Month;
            foreach(GccNames gc in g) {
                string Gcc = gc.Gcc;
                ScoreHelper sh = new(Gcc,year,month);
                ScoreCacheHelper sch = new(Gcc, year, month);
                MigHelper mh = new();
                await mh.LoadData(Gcc, year, month);
                mh.PeriodsToGcc();
                List<PayPeriodGcc> pg = mh.pg;
                sh.FillCalendar(pg);
                sch.AddCache(sh.c);
            }
            currentmonth = currentmonth.AddMonths(1);
        }

        return 0;
    }

    public async Task<int> PGLoader() {
        CSVMigHelper c = new();
        

        string targetDirectory = "src/Data/Raw";
        string [] fileEntries = Directory.GetFiles(targetDirectory,"*.csv");
        int total = 0;
        DateTime ts = new DateTime(1981,01,01);
        foreach(string fileName in fileEntries) {
         
            var results = c.ReadPg(fileName);
            results = results.Where(x => x.Close > ts && x.CutOff > ts && x.Open > ts && x.PayDate > ts).ToList();
            Context.AddRange(results);
            total+=results.Count;
            
        }
        await Context.SaveChangesAsync();          

        var Gccs = Context.GccNames;
        foreach(var gcc in Gccs) {

            gcc.LCCCount = Context.PayPeriods.Where(x => x.Gcc == gcc.Gcc).Select(x => x.Lcc).Distinct().Count();
            gcc.PGCount = Context.PayPeriods.Where(x => x.Gcc == gcc.Gcc).Select(x => x.PayGroup).Distinct().Count();
            gcc.Countrycount = Context.PayPeriods.Where(x => x.Gcc == gcc.Gcc).Select(x => x.Lcc.Substring(0, 2)).Distinct().Count();
        }
       // Context.MigStats.AddRange(mlist);
        await Context.SaveChangesAsync(); 
        await PGCacheLoader();
        return total;
        
    }
}