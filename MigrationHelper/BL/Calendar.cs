namespace MigrationHelper.BL;

using MigrationHelper.Models;

public class Calendar
{


//    public int Id { get; set; }

    public Dictionary<int, CalDay> Days { get; set; } = new();


    public Calendar(int year, int month)
    {
        int days = DateTime.DaysInMonth(year, month);


        for (int i = 1; i <= days; i++)
        {

            DateTime dt = new DateTime(year, month, i);
            Days[i] = new CalDay { Day = i, Score = 0, Name = dt.ToString("ddd"), Percentage = 0 };
            if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
            {
                Days[i].Score += (int)ScoreConfiguration.Weekend;
                 Days[i].Details.Add(new ScoreBreakdownMessage { Message = $"Weekend", Sc = ScoreConfiguration.Weekend});
                
            }

        }
    }

}