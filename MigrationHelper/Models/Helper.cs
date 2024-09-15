
namespace MigrationHelper.Models;

public class Helper
{

    public Dictionary<int, CalDay> c { get; set; } = new();
    private int Month { get; set; }
    private int Year { get; set; }

    private string Gcc { get; set; }

    public Helper(string Gcc, int year, int Month)
    {
        this.Gcc = Gcc;
        this.Month = Month;
        this.Year = year;
        ScoreCacheHelper sch = new(Gcc,year,Month);
        var results = sch.GetCache();
        if (results.Count == 0) {
            Console.WriteLine($"No cache for {Gcc} and {Month}");
            MigHelper mh = new();
            mh.LoadData(Gcc, year,Month);
            List<PayPeriodGcc> pg = mh.pg;

            FillCalendar(pg);
            sch.AddCache(c);
        } else {
            foreach(var x in results) {
                c.Add(x.Day,x);
            }
            Console.WriteLine($"Cache for {Gcc} and {Month}");
        }
        
    }




    public void FillCalendar(List<PayPeriodGcc> pg)
    {


        c = new Calendar(Year, this.Month).Days;
        int nrdays = c.Count();
        bool notClosed = false;
        foreach (KeyValuePair<int, CalDay> a in c)
        {
            DateTime dt = new(Year, this.Month, a.Key, 0, 0, 0);

            foreach (var p in pg)
            {

                if (p.CutOff.Day == dt.Day)
                {
                    c[a.Key].Score += (int)ScoreConfiguration.CutOff;
                    c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Cut off date for {p.PayGroup}", Sc = ScoreConfiguration.CutOff});
                    if (a.Key != 1) {
               //     c[a.Key - 1].Score -= sc.CutOff;
               //     c[a.Key - 1].Details.Add($"Cut off -1 date for {p.PayGroup}");
                    }
                }
                if (p.PayDate.Day == dt.Day)
                {
                    c[a.Key].Score += (int)ScoreConfiguration.PayDate;
                    c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Pay date for {p.PayGroup}", Sc = ScoreConfiguration.PayDate});
                  
                    if (a.Key != nrdays) {
                        c[a.Key + 1].Score += (int)ScoreConfiguration.NextPayDate;
                        c[a.Key + 1].Details.Add(new ScoreBreakdownMessage { Message = $"Pay date +1 for {p.PayGroup}", Sc = ScoreConfiguration.NextPayDate});
                    }
                }

                notClosed = dt.Day <= p.CutOff.Day || dt.Day >= p.CutOff.Day;

                if (c[a.Key].Score >= 0 && notClosed)
                {
                    c[a.Key].Score += (int)ScoreConfiguration.Free;
                    c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Free slot when {p.PayGroup} is not closed", Sc = ScoreConfiguration.Free});
          
                }
                
                if (c[a.Key].Score >= 0 && ! notClosed)
                {
                    c[a.Key].Score += (int)ScoreConfiguration.FreeAfterClose;
                    c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Free slot when {p.PayGroup} is closed", Sc = ScoreConfiguration.FreeAfterClose});
                }

            }
        }

        int maxScore = pg.DistinctBy(x => x.PayGroup).Count() * 4;
        foreach (KeyValuePair<int, CalDay> a in c)
        {
            if (c[a.Key].Score > 0)
            {
                double res = (c[a.Key].Score *1.0 / maxScore*1.0)*100;
                c[a.Key].Percentage = (int)Math.Ceiling(res);

            }
        
            if (a.Key != 1 && a.Key != nrdays && c[a.Key-1].Score <= 0 && c[a.Key].Score > 0 && c[a.Key+1].Score <= 0) {
            {
                c[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"No free slot on the next day ({a.Key+1})", Sc = ScoreConfiguration.FreeAfterClose});
                c[a.Key].Percentage = 50;
            }
            }

        }

    }

}
