namespace MigrationHelper.Helpers.Init;
using MigrationHelper.Db;

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

    public int PGLoader() {
        CSVMigHelper c = new();
        

        string targetDirectory = "src/Data/Raw";
        string [] fileEntries = Directory.GetFiles(targetDirectory);
        int total = 0;
        DateTime ts = new DateTime(1981,01,01);
        foreach(string fileName in fileEntries) {
         
            var results = c.ReadPg(fileName);
            results = results.Where(x => x.Close > ts && x.CutOff > ts && x.Open > ts && x.PayDate > ts).ToList();
            Context.AddRange(results);
            total+=results.Count;
            Context.SaveChanges();  
        }
        return total;
        
    }
}