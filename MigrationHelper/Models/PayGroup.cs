namespace MigrationHelper.Models;

public class PayPeriodGcc {

    public int PayPeriodGccId { get;set;}

    public string Gcc { get; set; }

    public string PayGroup { get; set; }

    public DateTime CutOff {get;set;}
    public DateTime PayDate {get;set;}
    public DateTime Open {get;set;}
    public DateTime Close {get;set;}
}