namespace MigrationHelper.BL;

using MigrationHelper.Models;

public class ScoreHelper
{

    public Dictionary<int, CalDay> CalendarDays { get; set; } = [];
    private  int  Month { get; set; }
    private int Year { get; set; }

    public readonly ScoreCacheHelper Sch;

    private readonly ScoreConfig Sc;

    public ScoreHelper(ScoreConfig sc,string Gcc, int year, int Month)
    {
        Sc = sc;
        this.Month = Month;
        this.Year = year;
        Sch = new(Gcc, year, Month);

        foreach (var x in Sch.GetCache())
        {
            CalendarDays.Add(x.Day, x);
        }
    }

    public void FillCalendar(List<PayPeriodGcc> pg)
    {


        CalendarDays = new Calendar(Year, this.Month).Days;
        int nrdays = CalendarDays.Count;
        bool Closed = false;
        foreach (KeyValuePair<int, CalDay> a in CalendarDays)
        {
            DateTime dt = new(Year, this.Month, a.Key, 0, 0, 0);

            // Saturday and Sunday only once
            
            if (dt.DayOfWeek == DayOfWeek.Saturday)
            {
                CalendarDays[a.Key].Score += Sc.Saturday;
                CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Saturday", Sc = Sc.Saturday});
                
            }
            if (dt.DayOfWeek == DayOfWeek.Sunday)
            {
                CalendarDays[a.Key].Score += Sc.Sunday;
                CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Sunday", Sc = Sc.Sunday});
                
            }
            foreach (var p in pg) {

                if (p.CutOff.Day == dt.Day)
                {
                    CalendarDays[a.Key].Score += Sc.CutOff;
                    CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Cut off date for {p.PayGroup}", Sc = Sc.CutOff });
                    continue;
                }

                if (Month == 12 && Year == 2024 && dt.Day > 23) {
                    CalendarDays[a.Key].Score += Sc.CutOff;
                    CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Christmas blackout for {p.PayGroup}", Sc = Sc.CutOff });
                    continue;   
                }

                if (p.Frequency == "monthly")
                {
                    if (dt.Day + 1 == p.CutOff.Day)
                    {

                        CalendarDays[a.Key].Score += Sc.CutOffBlackout;
                        CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Cut off -1 date for {p.PayGroup}", Sc = Sc.CutOffBlackout });
                        continue;
                    }
                    if (dt.Day + 2 == p.CutOff.Day)
                    {
                        CalendarDays[a.Key].Score += Sc.CutOffBlackout;
                        CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Cut off -2 date for pay group {p.PayGroup}", Sc = Sc.CutOffBlackout });
                        continue;
                    }
                }
                if (p.PayDate.Day == dt.Day)
                {
                    CalendarDays[a.Key].Score += Sc.PayDate;
                    CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Pay date for pay group {p.PayGroup}", Sc = Sc.PayDate });
                    continue;
                }
                if (p.PayDate.Day + 1 == dt.Day)
                {
                    CalendarDays[a.Key].Score += Sc.NextPayDate;
                    CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Pay date +1 for pay group {p.PayGroup}", Sc = Sc.NextPayDate });
                    continue;
                }

                if (dt.Day <= p.QueueOpen.Day && p.QueueOpen > p.PCEndDate && p.Frequency == "monthly")
                {
                    CalendarDays[a.Key].Score += Sc.BlockedAfterClose;
                    CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Pay group {p.PayGroup} is closed", Sc = Sc.BlockedAfterClose });
                    continue;
                }

                Closed = dt.Day >= p.CutOff.Day && dt.Day <= p.QueueOpen.Day;
                if (Closed)
                {
                    CalendarDays[a.Key].Score += Sc.BlockedAfterClose;
                    CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Pay group {p.PayGroup} is closed", Sc = Sc.BlockedAfterClose });
                    continue;
                }

                if (!Closed)
                //   if (c[a.Key].Score >= 0 && ! notClosed)
                {
                    CalendarDays[a.Key].Score += Sc.FreeAfterClose;
                    CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"Free slot for pay group {p.PayGroup}", Sc = Sc.FreeAfterClose });
                    continue;
                }

            }
        }

        int maxScore = pg.DistinctBy(x => x.PayGroup).Count() * 4;
        foreach (KeyValuePair<int, CalDay> a in CalendarDays)
        {
            if (CalendarDays[a.Key].Score > 0)
            {
                double res = (CalendarDays[a.Key].Score * 1.0 / maxScore * 1.0) * 100;
                CalendarDays[a.Key].Percentage = (int)Math.Ceiling(res);

            }

            if (a.Key != 1 && a.Key != nrdays && CalendarDays[a.Key - 1].Score <= 0 && CalendarDays[a.Key].Score > 0 && CalendarDays[a.Key + 1].Score <= 0)
            {
                {
                    CalendarDays[a.Key].Details.Add(new ScoreBreakdownMessage { Message = $"No free slot on the next day ({a.Key + 1})", Sc = Sc.FreeAfterClose });
                    CalendarDays[a.Key].Percentage = 50;
                }
            }

        }

    }

}
