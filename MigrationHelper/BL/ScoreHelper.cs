
namespace MigrationHelper.BL;

using MigrationHelper.Models;

public class ScoreHelper
{

    public Dictionary<int, CalDay> c { get; set; } = new();
    private int Month { get; set; }
    private int Year { get; set; }

    private string Gcc { get; set; }

    public ScoreHelper(string Gcc, int year, int Month)
    {
        this.Gcc = Gcc;
        this.Month = Month;
        this.Year = year;
        ScoreCacheHelper sch = new(Gcc, year, Month);
        var results = sch.GetCache();
        if (results.Count == 0)
        {
            Console.WriteLine($"No cache for {Gcc} and {Month}");
            MigHelper mh = new();
            mh.LoadData(Gcc, year, Month);
            List<PayPeriodGcc> pg = mh.pg;

            FillCalendar(pg);
            sch.AddCache(c);
        }
        else
        {
            foreach (var x in results)
            {
                c.Add(x.Day, x);
            }
            Console.WriteLine($"Cache for {Gcc} and {Month}");
        }

    }




    public void FillCalendar(List<PayPeriodGcc> pg)
    {


        c = new Calendar(Year, this.Month).Days;
        int nrdays = c.Count();
        bool Closed = false;
        foreach (KeyValuePair<int, CalDay> a in c)
        {
            DateTime dt = new(Year, this.Month, a.Key, 0, 0, 0);

            foreach (var p in pg)
            {


                //     Console.WriteLine
                if (p.CutOff.Day == dt.Day)
                {
                    c[a.Key].Score += (int)ScoreConfiguration.CutOff;
                    c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Cut off date for {p.PayGroup}", Sc = ScoreConfiguration.CutOff });
                    continue;
                }

                if (dt.Day + 1 == p.CutOff.Day)
                {

                    c[a.Key].Score += (int)ScoreConfiguration.CutOffBlackout;
                    c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Cut off -1 date for {p.PayGroup}", Sc = ScoreConfiguration.CutOffBlackout });
                    continue;
                }
                if (dt.Day + 2 == p.CutOff.Day)
                {
                    c[a.Key].Score += (int)ScoreConfiguration.CutOffBlackout;
                    c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Cut off -2 date for {p.PayGroup}", Sc = ScoreConfiguration.CutOffBlackout });
                    continue;
                }
                if (p.PayDate.Day == dt.Day)
                {
                    c[a.Key].Score += (int)ScoreConfiguration.PayDate;
                    c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Pay date for {p.PayGroup}", Sc = ScoreConfiguration.PayDate });


                    if (a.Key != nrdays)
                    {
                        c[a.Key + 1].Score += (int)ScoreConfiguration.NextPayDate;
                        c[a.Key + 1].Details.Add(new ScoreBreakdownMessage { Message = $"Pay date +1 for {p.PayGroup}", Sc = ScoreConfiguration.NextPayDate });
                    }
                }

                Closed = dt.Day >= p.CutOff.Day && dt.Day <= p.QueueOpen.Day;
                if (Closed)
                //   if (c[a.Key].Score >= 0 && notClosed)
                {
                    c[a.Key].Score += (int)ScoreConfiguration.BlockedAfterClose;
                    c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Pay group {p.PayGroup} is closed", Sc = ScoreConfiguration.BlockedAfterClose });

                }

                if (!Closed)
                //   if (c[a.Key].Score >= 0 && ! notClosed)
                {
                    c[a.Key].Score += (int)ScoreConfiguration.FreeAfterClose;
                    c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Free slot for {p.PayGroup}", Sc = ScoreConfiguration.FreeAfterClose });
                }

            }
        }

        int maxScore = pg.DistinctBy(x => x.PayGroup).Count() * 4;
        foreach (KeyValuePair<int, CalDay> a in c)
        {
            if (c[a.Key].Score > 0)
            {
                double res = (c[a.Key].Score * 1.0 / maxScore * 1.0) * 100;
                c[a.Key].Percentage = (int)Math.Ceiling(res);

            }

            if (a.Key != 1 && a.Key != nrdays && c[a.Key - 1].Score <= 0 && c[a.Key].Score > 0 && c[a.Key + 1].Score <= 0)
            {
                {
                    c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"No free slot on the next day ({a.Key + 1})", Sc = ScoreConfiguration.FreeAfterClose });
                    c[a.Key].Percentage = 50;
                }
            }

        }

    }

}
