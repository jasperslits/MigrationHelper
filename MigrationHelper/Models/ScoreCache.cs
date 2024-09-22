namespace MigrationHelper.Models;

using MigrationHelper.Db;

public class ScoreCache : CalDay
{
    public int ScoreCacheId { get; set; }
    public string Gcc { get; set; } = "";
    public int Month { get; set; } = 0;

     public int Year { get; set; } = 0;

}

public class ScoreCacheHelper 
{
    private MigHelperCtx _Ctx;
    private string Gcc;
    private int Month;

     private int Year;
    public ScoreCacheHelper(string gcc, int year, int month) {
         _Ctx = new MigHelperCtx();
         Gcc = gcc;
         Month = month;
         Year = year;

    }

    public List<CalDay> GetCache() {
        List<CalDay> cd = new();
        var res = _Ctx.ScoreCache.Where(x => x.Gcc == Gcc && x.Month == Month && x.Year == Year).ToList();
        foreach(ScoreCache x in res) {
            cd.Add( new CalDay { Day = x.Day, Percentage = x.Percentage, Score = x.Score, Name = x.Name});
        }

        return cd;
    }

    public async void AddCache(Dictionary<int, CalDay> cd) {
       
       List<ScoreCache> sc = new();
       List<ScoreBreakdown> sb = new();
       foreach(var y in cd) {
        CalDay x = y.Value;
        sc.Add ( new ScoreCache { Gcc = Gcc, Month = Month, Year = Year, Day = x.Day, Name = x.Name, Percentage = x.Percentage, Score = x.Score });
        foreach(var d in x.Details) {
            sb.Add( new ScoreBreakdown { Gcc = Gcc, Month = Month,Day = x.Day, Message = d.Message, Year = Year, Sc = d.Sc });    
        }
       }

        _Ctx.ScoreCache.AddRange(sc);
        _Ctx.ScoreBreakdown.AddRange(sb);
        await _Ctx.SaveChangesAsync();
    }
}