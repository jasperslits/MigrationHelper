using MigrationHelper.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Diagnostics;

namespace MigrationHelper.Helpers.Init;

public class CSVCountries {
    
    private string Path {get;set;}


    public List<Countries> ReadCountries(string path) {
        Path = path;

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
