using System.ComponentModel.DataAnnotations;

namespace MigrationHelper.Models;

public class ScoreBreakdownMessage
{

    public int Sc { get; set; }

    public string Message { get; set; }
}

public class ScoreBreakdown : ScoreBreakdownMessage
{

    public int ScoreBreakdownId { get; set; }
    [Required]
    public string Gcc { get; set; }
    [Required]
    public int Month { get; set; } = 0;

    public int Year { get; set; } = 2024;

    public int Day { get; set; } = 2024;

}