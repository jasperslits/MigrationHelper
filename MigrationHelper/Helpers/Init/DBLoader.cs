namespace MigrationHelper.Helpers.Init;
using MigrationHelper.Db;
using MigrationHelper.Models;

public class DBLoader
{
    public int Rc {get;set;} = 1;

    public MigHelperCtx Context { get;set;}

    public DBLoader() {
        Context = new MigHelperCtx();
    }

    public int GccLoader() {
        CSVMigHelper c = new CSVMigHelper();
       
        Context.GccNames.RemoveRange(Context.GccNames);
        Context.SaveChanges();
      //  await context.GccNames.ExecuteDeleteAsync();
        var results = c.ReadGcc();
   
        Context.AddRange(results);
        Context.SaveChanges();
        return results.Count;
    }

    public int PGLoader() {
        CSVMigHelper c = new CSVMigHelper();
        

        string targetDirectory = "src/Data/Raw";
        string [] fileEntries = Directory.GetFiles(targetDirectory);
        int total = 0;
        foreach(string fileName in fileEntries) {
         
            var results = c.ReadPg(fileName);
            Context.AddRange(results);
            total+=results.Count;
            Context.SaveChanges();  
        }
        return total;
        
    }
}