namespace MigrationHelper.Models;
public class OutputHelper(Dictionary<int, CalDay> res)
{

    public List<string> DayNames { get; set; } = res.Select(x => x.Value.Name).ToList();
    public List<int> DayScore { get; set; } = res.Select(x => x.Value.Score).ToList();
    public List<int> DayNumbers { get; set; } = res.Select(x => x.Value.Day).ToList();
    public List<int> DayPercentage { get; set; } = res.Select(x => x.Value.Percentage).ToList();
}