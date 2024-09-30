namespace MigrationHelper.Models;

public record Periods(string MonthName)
{
    public int Year { get; set; } = 2024;
    public int Month { get; set; } = 9;
}