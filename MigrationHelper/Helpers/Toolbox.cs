namespace MigrationHelper.Helpers;
public static class Toolbox 
{

    public static string MonthToName(int year,int month) {
        return new DateTime(year, month, 1).ToString("MMMM yyyy");
    }

     public static string DayToName(int year,int month,int day) {
        return new DateTime(year, month, day).ToString("dd MMMM yyyy");
    }
}