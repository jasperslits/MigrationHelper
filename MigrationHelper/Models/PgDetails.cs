namespace MigrationHelper.Models;
using MigrationHelper.Db;
using MigrationHelper.BL;


public class PGDetails
{

    private int Month { get; set; }
    private int Year { get; set; }

    private string Gcc { get; set; }
    public Dictionary<int, CalDay> c { get; set; }

    public PGDetails(string gcc, int year, int month)
    {
        this.Month = month;
        this.Year = year;
        this.Gcc = gcc;
        c = new Calendar(year, month).Days;
    }

    public Dictionary<int, CalDay> GetCalendar() {
        return c;
    }

    public List<PayPeriod> GetDetails()
    {
        var mh = new MigHelper();
        mh.LoadData(this.Gcc, this.Year,this.Month);
        return mh.pp;

     /*   DateTime pStart = new (Year, Month, 1, 0, 0, 0);
        DateTime pEnd = new (Year, Month, DateTime.DaysInMonth(Year, Month), 0, 0, 0);
        var pg = context.PayPeriods.Where(x => x.Gcc == Gcc && x.CutOff >= pStart 
                           &&  x.CutOff <= pEnd).OrderBy(x => x.Lcc).ToList();
*/
    

    }


}