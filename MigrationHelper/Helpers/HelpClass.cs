namespace MigrationHelper.Helpers;

using MigrationHelper.Models;

public class MonthCache {
    public string Gcc { get;set;}
    public int Month { get; set; }
}

public class PGD {
    public string Lcc { get; set; }
    public string PayGroup { get; set; }

    public  Dictionary<int, CalDay> calDays{ get; set; }
}