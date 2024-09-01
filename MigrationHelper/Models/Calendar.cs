
namespace MigrationHelper.Models;

public class CalDay
{
    public string Name { get; set; }
    public int Day { get; set; }
    public int Score { get; set; } = 0;
    public int Percentage { get; set; } = 0;

    public string Color { get; set; } = "green";

    public List<string> Details { get; set; } = new();
}

public class Calendar
{


    public int Id { get; set; }

    public Dictionary<int, CalDay> Days { get; set; } = new();


    public Calendar(int year, int month)
    {
        int days = DateTime.DaysInMonth(year, month);
        ScoreConfiguration sc = new ScoreConfiguration();

        for (int i = 1; i <= days; i++)
        {

            DateTime dt = new DateTime(2024, month, i);
            Days[i] = new CalDay { Day = i, Score = 0, Name = dt.ToString("ddd"), Percentage = 0 };
            if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
            {
                Days[i].Score -= sc.Weekend;
                Days[i].Details.Add($"Weekend");
            }

        }
    }

}