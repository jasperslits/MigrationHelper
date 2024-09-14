namespace MigrationHelper.Models;
public static class Toolbox 
{

    public static string MonthToName(int month) {
        return new DateTime(2024, month, 1).ToString("MMMM yyyy");
    }

     public static string DayToName(int year,int month,int day) {
        return new DateTime(year, month, day).ToString("dd MMMM yyyy");
    }
}