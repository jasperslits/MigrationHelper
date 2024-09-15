using System.ComponentModel.DataAnnotations;

namespace MigrationHelper.Models;

[System.ComponentModel.DataAnnotations.Schema.Table("GCCNames")]
public class GccNames {

    public int GccNamesId { get; set;}
    [Required]
    public string Gcc { get; set; }
    [Required]
    public string Name { get; set; }

    public int Month { get; set; } = 0;

    public int? Year { get; set; } = 2024;

    public bool Migrated { get; set; } = false;
}