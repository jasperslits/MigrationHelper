namespace MigrationHelper.BL;

using MigrationHelper.Models;

public class Calendar
{

    public Dictionary<int, CalDay> Days { get; set; } = [];

    public Calendar(int year, int month)
    {
        int days = DateTime.DaysInMonth(year, month);
        for (int i = 1; i <= days; i++)
        {
            Days[i] = new CalDay { Day = i, Score = 0, Name = new DateTime(year,month,i).ToString("ddd"), Percentage = 0 };
        }
    }

}