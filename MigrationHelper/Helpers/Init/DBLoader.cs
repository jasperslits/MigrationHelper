namespace MigrationHelper.Helpers.Init;
using MigrationHelper.Db;

public class DBLoader
{
    public int Rc {get;set;} = 1;

    public MigHelperCtx context { get;set;}

    public DBLoader() {
        context = new MigHelperCtx();
    }

    public int GccLoader() {
        CSVMigHelper c = new CSVMigHelper();
      //  await context.GccNames.ExecuteDeleteAsync();
        var results = c.ReadGcc();
   
        context.AddRange(results);
        context.SaveChanges();
        return results.Count;
    }

    public int PGLoader() {
        CSVMigHelper c = new CSVMigHelper();
        

        string targetDirectory = "src/Data/Raw";
        string [] fileEntries = Directory.GetFiles(targetDirectory);
        int total = 0;
        foreach(string fileName in fileEntries) {
         
            var results = c.ReadPg(fileName);
            context.AddRange(results);
            total+=results.Count;
            context.SaveChanges();  
        }
        return total;
        
    }
}