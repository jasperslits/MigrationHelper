namespace MigrationHelper.Models;

using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;

public class PayPeriod
{
    public int PayPeriodId { get;set;}
    public string Gcc { get; set; }
    public string Lcc { get; set; }
    public string PayGroup { get; set; }
    public DateTime Open { get; set; }
    public DateTime Close { get; set; }
    public DateTime CutOff { get; set; }
    public DateTime PayDate { get; set; }
    public DateTime QueueOpen { get; set; }
    public int Number {get;set;}
    public string Frequency {get;set;}
    public DateTime Payslip { get; set; }
    public DateTime PayStartDate { get; set; }
    public DateTime PayEndDate { get; set; }
    public DateTime PCStartDate { get; set; }
    public DateTime PCEndDate { get; set; }

    public int Offcycle { get; set; }
}
