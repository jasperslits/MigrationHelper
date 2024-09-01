namespace MigrationHelper.Models;

public class ScoreConfiguration
{

    public ScoreConfiguration() {}

    public int Weekend { get; set; } = 10;
    public int CutOff { get; set; } = 2;
    public int PayDate { get; set; } = 2;

    public int Free {get;set; } = 4;
}