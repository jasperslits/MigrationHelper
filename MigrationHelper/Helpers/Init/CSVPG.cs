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
            csv.Context.RegisterClassMap<ServiceProvidedMap>();
            records = csv.GetRecords<PayPeriod>().ToList();

        }
        return records;
        }
}


public class ServiceProvidedMap : ClassMap<PayPeriod>
{


	public ServiceProvidedMap()
	{
        DateTime ts = DateTime.Now;

		Map(m => m.Gcc).Name("GCC");
		Map(m => m.Lcc).Name("LCC");
		Map(m => m.PayGroup).Name("PAYGROUP");
		Map(m => m.Open).Name("OPEN_DATE").Name("OPEN_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);;
		Map(m => m.PayDate).Name("PAYDATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);;
        Map(m => m.CutOff).Name("CUTOFFDATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);;
		Map(m => m.Close).Name("CLOSE_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
	}
}

// GCC,LCC,PAYGROUP,OPEN_DATE,CLOSE_DATE,PAYSCHEDULE,PAYDATE,CUTOFFDATE
