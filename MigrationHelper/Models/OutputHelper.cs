namespace MigrationHelper.Models;
public class OutputHelper
{

    public List<string> DayNames { get; set; }
    public List<int> DayScore { get; set; }
    public List<int> DayNumbers { get; set; }
    public List<int> DayPercentage { get; set; }

    public OutputHelper(Dictionary<int, CalDay> res)
    {
        DayNames = res.Select(x => x.Value.Name).ToList();
        DayScore = res.Select(x => x.Value.Score).ToList();
        DayPercentage = res.Select(x => x.Value.Percentage).ToList();
        DayNumbers = res.Select(x => x.Value.Day).ToList();

    }

}