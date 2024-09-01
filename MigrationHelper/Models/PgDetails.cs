namespace MigrationHelper.Models;
using MigrationHelper.Db;
public class PGDetails
{

    private int Month { get; set; }

    private string Gcc { get; set; }
    public Dictionary<int, CalDay> c { get; set; }

    public PGDetails(string gcc, int month)
    {
        this.Month = month;
        this.Gcc = gcc;
        c = new Calendar(2024, month).Days;
    }

    public Dictionary<int, CalDay> GetCalendar() {
        return c;
    }

    public List<PayPeriod> GetDetails()
    {
        var context = new MigHelperCtx();

        DateTime pStart = new (2024, Month, 1, 0, 0, 0);
        DateTime pEnd = new (2024, Month, DateTime.DaysInMonth(2024, Month), 0, 0, 0);
        var pg = context.PayPeriods.Where(x => x.Gcc == Gcc && pStart >= x.Open).ToList();

        return pg;
/*
         int nrdays = c.Count();
        foreach (KeyValuePair<int, CalDay> a in c)
        {
            DateTime dt = new(2024, month, a.Key, 0, 0, 0);

            foreach (var p in pg) {
                if (p.Open.Day == dt.Day) {
                     c[a.Key].Color = "green";
                }
                if (p.Close.Day == dt.Day) {
                    c[a.Key].Color = "blue";
                }
                if (p.CutOff.Day == dt.Day) {
                    c[a.Key].Color = "orange";
                }
                if (p.PayDate.Day == dt.Day) {
                    c[a.Key].Color = "red";
                }
            }
        }

        return pg;
        */
    }


}