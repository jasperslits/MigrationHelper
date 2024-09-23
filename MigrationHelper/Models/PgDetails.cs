namespace MigrationHelper.Models;
using MigrationHelper.Db;
using MigrationHelper.BL;


public class PGDetails(string gcc, int year, int month)
{

    private int Month { get; set; } = month;
    private int Year { get; set; } = year;

    private string Gcc { get; set; } = gcc;
    private Dictionary<int, CalDay> C { get; set; } = new Calendar(year, month).Days;

    public Dictionary<int, CalDay> GetCalendar() {
        return C;
    }

    public async Task<List<PayPeriod>> GetDetails()
    {
        MigHelper mh = new();
        await mh.LoadData(Gcc, Year,Month);
        return mh.pp;
    }


}