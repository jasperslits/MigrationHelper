
namespace MigrationHelper.Models;

using MigrationHelper.Db;


public class CalDay
{
    public string Name { get; set; }
    public int Day { get; set; }
    public int Score { get; set; } = 0;
    public int Percentage { get; set; } = 0;

    public string Color { get; set; } = "green";

    public List<string> Details { get; set; } = new();
}

public class MigHelper
{
    public List<PayPeriodGcc> pg = new List<PayPeriodGcc>();

    public MigHelper()
    {

    }

    public List<string> GetGCCs()
    {
        var context = new MigHelperCtx();

        var results = context.PayPeriods.Select(x => x.Gcc).Distinct().ToList();
        results.Sort();
        return results;
    }


    public void LoadData(string Gcc, int month)
    {

        var context = new MigHelperCtx();

        DateTime pStart = new DateTime(2024, month, 1, 0, 0, 0);
        DateTime pEnd = new DateTime(2024, month, DateTime.DaysInMonth(2024, month), 0, 0, 0);
        var b = context.PayPeriods.Where(x => x.Gcc == Gcc && pStart >= x.Open).ToList();
        Console.WriteLine($"Found PayPeriods {b.Count} records");

        List<PayPeriodGcc> x = b.DistinctBy(x => x.PayGroup).Select(o => new PayPeriodGcc
        {
            Gcc = o.Gcc,
            PayGroup = o.PayGroup,
            Close = o.Close,
            PayDate = o.PayDate,
            Open = o.Open,
            CutOff = o.CutOff
        }).ToList();
        pg = x;
        Console.WriteLine($"Found PayPeriodGcc {x.Count} records");
    }
}

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

public class PGDetails
{

    private int Month { get; set; }

    private string Gcc { get; set; }
    public Dictionary<int, CalDay> c { get; set; }

    public PGDetails(string gcc, int month)
    {
        this.Month = month;
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

public class Helper
{

    public Dictionary<int, CalDay> c { get; set; }
    private int Month { get; set; }

    private string Gcc { get; set; }



    public Helper(string Gcc, int Month)
    {
        this.Gcc = Gcc;
        this.Month = Month;
        MigHelper mh = new();
        mh.LoadData(Gcc, Month);
        List<PayPeriodGcc> pg = mh.pg;

        FillCalendar(pg);

    }




    public void FillCalendar(List<PayPeriodGcc> pg)
    {

        ScoreConfiguration sc = new ScoreConfiguration();
        c = new Calendar(2024, this.Month).Days;
        int nrdays = c.Count();
        foreach (KeyValuePair<int, CalDay> a in c)
        {
            DateTime dt = new(2024, this.Month, a.Key, 0, 0, 0);

            foreach (var p in pg)
            {

                if (p.CutOff.Day == dt.Day)
                {
                    c[a.Key].Score -= sc.CutOff;
                    c[a.Key].Details.Add($"Cut off date for {p.PayGroup}");
                    if (a.Key != 1) {
                    c[a.Key - 1].Score -= sc.CutOff;
                    c[a.Key - 1].Details.Add($"Cut off -1 date for {p.PayGroup}");
                    }
                }
                if (p.PayDate.Day == dt.Day)
                {
                    c[a.Key].Score -= sc.PayDate;
                    c[a.Key].Details.Add($"Pay date for {p.PayGroup}");
                    if (a.Key != 1) {
                    c[a.Key - 1].Score -= sc.PayDate;
                    c[a.Key - 1].Details.Add($"Pay date -1 for {p.PayGroup}");
                    }
                }

                if (c[a.Key].Score >= 0)
                {
                    if (a.Key + 1 < nrdays && c[a.Key + 1].Score >= 0)
                        c[a.Key].Score += sc.Free;
                    c[a.Key].Details.Add($"Free slot for {p.PayGroup}");
                }

            }
        }

        int maxScore = pg.DistinctBy(x => x.PayGroup).Count() * 4;
        foreach (KeyValuePair<int, CalDay> a in c)
        {
            if (c[a.Key].Score > 0)
            {
                c[a.Key].Percentage = (int)Math.Ceiling(c[a.Key].Score / maxScore * 1.0) * 100;

                if (a.Key + 1 < nrdays && c[a.Key + 1].Score <= 0)
                {
                    c[a.Key].Percentage = 20;
                    c[a.Key].Score = 0;
                    c[a.Key].Details.Add("Next day is not free");
                }
            }

        }

    }

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