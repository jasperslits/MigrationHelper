using System.ComponentModel.DataAnnotations;

namespace MigrationHelper.Models;

public class MigStats {

    [Key]
    public string Gcc { get;set;}

    public int Countrycount { get; set; }
    public int LCCCount { get; set; }

    public int PGCount { get; set; }
}