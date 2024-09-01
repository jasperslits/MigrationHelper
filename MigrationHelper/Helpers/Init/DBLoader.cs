namespace MigrationHelper.Helpers.Init;
using MigrationHelper.Db;

public class DBLoader
{
    public int Rc {get;set;} = 1;

    public DBLoader() {
        CSVMigHelper c = new CSVMigHelper();
        string Gcc;


        var context = new MigHelperCtx();

        string targetDirectory = "src/Data/Raw";
        string [] fileEntries = Directory.GetFiles(targetDirectory);
        foreach(string fileName in fileEntries) {
         
            var results = c.ReadPg(fileName);
            context.AddRange(results);
            Console.WriteLine($"{fileName} - {results.Count}");
            context.SaveChanges();  
        }

        
    }
}