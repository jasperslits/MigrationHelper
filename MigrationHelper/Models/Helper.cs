
namespace MigrationHelper.Models;




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
        bool notClosed = false;
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
               //     c[a.Key - 1].Score -= sc.CutOff;
               //     c[a.Key - 1].Details.Add($"Cut off -1 date for {p.PayGroup}");
                    }
                }
                if (p.PayDate.Day == dt.Day)
                {
                    c[a.Key].Score -= sc.PayDate;
                    c[a.Key].Details.Add($"Pay date for {p.PayGroup}");
                    if (a.Key != nrdays) {
                    c[a.Key + 1].Score -= sc.NextPayDate;
                    c[a.Key + 1].Details.Add($"Pay date +1 for {p.PayGroup}");
                    }
                }

                notClosed = dt.Day <= p.CutOff.Day || dt.Day >= p.CutOff.Day;

                if (c[a.Key].Score >= 0 && notClosed)
                {
                        c[a.Key].Score += sc.Free;
                        c[a.Key].Details.Add($"Free slot when {p.PayGroup} is not closed ");
                }
                
                if (c[a.Key].Score >= 0 && ! notClosed)
                {
                        c[a.Key].Score += sc.FreeAfterClose;
                        c[a.Key].Details.Add($"Free slot when {p.PayGroup} is closed ");
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
                c[a.Key].Percentage = 50;
            }
            }

        }

    }

}
