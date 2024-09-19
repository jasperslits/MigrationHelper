
using System.ComponentModel.DataAnnotations.Schema;

namespace MigrationHelper.Models;

public class CalDay
{
    public string Name { get; set; }
    public int Day { get; set; }
    public int Score { get; set; } = 0;
    public int Percentage { get; set; } = 0;

    public string Color { get; set; } = "green";

    [NotMapped]
    public List<ScoreBreakdownMessage> Details { get; set; } = new();
}

