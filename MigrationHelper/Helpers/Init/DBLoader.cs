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

    public int PGLoader() {
        CSVMigHelper c = new();
        

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