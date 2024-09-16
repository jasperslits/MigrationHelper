namespace MigrationHelper.Models;


public class Countries {
    public string Name { get; set; }
    [System.ComponentModel.DataAnnotations.Key]
    public string Code { get; set; }

}