using MigrationHelper.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Diagnostics;

namespace MigrationHelper.Helpers.Init;

public class CSVMigHelper {
    
    private string Path {get;set;}


    private  void Fixdates() {

        string text = File.ReadAllText(Path);
        text = text.Replace("%results/CUTOFFDATE%","");
        File.WriteAllText(Path, text);
         
    }

    public List<GccNames> ReadGcc() {
    var path = "src/Data/Input/gcc.csv";
        
   

     var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";",
            HeaderValidated = null,
            MissingFieldFound = null
        };
        var records = new List<GccNames>();
        Console.WriteLine($"Reading {path}");
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, config))
        {
      
            csv.Context.RegisterClassMap<ServiceProvidedMapGcc>();
            records = csv.GetRecords<GccNames>().ToList();

        }
        return records;
        }


    public List<PayPeriod> ReadPg(string path) {
        Path = path;
        
        Fixdates();

     var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            HeaderValidated = null,
            MissingFieldFound = null
        };
        var records = new List<PayPeriod>();
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, config))
        {
            Console.WriteLine($"Reading {path}");
            csv.Context.RegisterClassMap<ServiceProvidedMapPG>();
            records = csv.GetRecords<PayPeriod>().ToList();

        }
        return records;
        }
}

public class ServiceProvidedMapPG : ClassMap<PayPeriod>
{


	public ServiceProvidedMapPG()
	{
        DateTime ts = new DateTime(1981,01,01);

		Map(m => m.Gcc).Name("GCC");
		Map(m => m.Lcc).Name("LCC");
		Map(m => m.PayGroup).Name("PAYGROUP");
		Map(m => m.Open).Name("OPEN_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
		Map(m => m.PayDate).Name("PAYDATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.CutOff).Name("CUTOFFDATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
		Map(m => m.Close).Name("CLOSE_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.Number).Name("NUMBER").Default(0);
         Map(m => m.Frequency).Name("FREQUENCY");
        Map(m => m.PayStartDate).Name("PAY_START_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
         Map(m => m.PayEndDate).Name("PAY_END_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.PCStartDate).Name("PC_START_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
		Map(m => m.PCEndDate).Name("PC_END_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.QueueOpen).Name("QUEUE_OPEN").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
	}
}

public class ServiceProvidedMapGcc : ClassMap<GccNames>
{


	public ServiceProvidedMapGcc()
	{
        DateTime ts = DateTime.Now;

		Map(m => m.Gcc).Name("Gcc");
		Map(m => m.Name).Name("Name");
	}
}

// GCC,LCC,PAYGROUP,OPEN_DATE,CLOSE_DATE,PAYSCHEDULE,PAYDATE,CUTOFFDATE
// Gcc;Name