using MigrationHelper.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Diagnostics;

namespace MigrationHelper.Helpers.Init;

public class CSVMigHelper
{

    private string Path { get; set; }
    private async Task Fixdates()
    {

        string text = await File.ReadAllTextAsync(Path);
        text = text.Replace("%results/CUTOFFDATE%", "");
        await File.WriteAllTextAsync(Path, text);

    }

    public List<GccNames> ReadGcc()
    {
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


    public async Task<List<PayPeriod>> ReadPg(string path)
    {
        Path = path;

        await Fixdates();

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
           
            csv.Context.RegisterClassMap<ServiceProvidedMapPG>();
            records = csv.GetRecords<PayPeriod>().ToList();

        }
        Console.WriteLine($"Reading {path} with {records.Count} records");
        return records;
    }

public List<Countries> ReadCountries(string path) {
    
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            HeaderValidated = null,
            MissingFieldFound = null
        };
        var records = new List<Countries>();
        using (var reader = new StreamReader(path))
        using (var csv = new CsvReader(reader, config))
        {
            Console.WriteLine($"Reading {path}");
            csv.Context.RegisterClassMap<ServiceProvidedMapCountry>();
            records = csv.GetRecords<Countries>().ToList();

        }
        return records;
 }
}

public class ServiceProvidedMapCountry : ClassMap<Countries>
{

	public ServiceProvidedMapCountry()
	{
        DateTime ts = DateTime.Now;

		Map(m => m.Name).Name("name");
		Map(m => m.Code).Name("alpha-2");
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

public class ServiceProvidedMapPG : ClassMap<PayPeriod>
{
    public ServiceProvidedMapPG()
    {
        DateTime ts = new(1981, 01, 01);

        Map(m => m.Gcc).Name("GCC");
        Map(m => m.Lcc).Name("LCC");
        Map(m => m.PayGroup).Name("PAYGROUP");
        Map(m => m.Open).Name("OPEN_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.PayDate).Name("PAYDATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.CutOff).Name("CUTOFFDATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.Close).Name("CLOSE_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.Number).Name("NUMBER").Default(0);
        Map(m => m.Frequency).Name("PAY_FREQUENCY");
        Map(m => m.PayStartDate).Name("PAY_START_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.PayEndDate).Name("PAY_END_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.PCStartDate).Name("PC_START_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.PCEndDate).Name("PC_END_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.QueueOpen).Name("QUEUE_OPEN").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
        Map(m => m.Offcycle).Name("OFFCYCLE").Default(0);
        Map(m => m.Payslip).Name("PAYSLIP_DATE").TypeConverterOption.Format("yyyy-MM-dd").Default(ts);
    }
}