namespace MigrationHelper.Models;

using Microsoft.EntityFrameworkCore;
using MigrationHelper.Db;

public class ScoreCache : CalDay
{
    public int ScoreCacheId { get; set; }
    public string Gcc { get; set; } = "";
    public int Month { get; set; } = 0;

     public int Year { get; set; } = 0;

}

public class ScoreCacheHelper(string gcc, int year, int month)
{
    private readonly MigHelperCtx _Ctx = new();
    private readonly string Gcc = gcc;
    private readonly int Month = month;
     private readonly int Year = year;

    public List<CalDay> GetCache() {
        List<CalDay> cd = [];
        var res = _Ctx.ScoreCache.Where(x => x.Gcc == Gcc && x.Month == Month && x.Year == Year).ToList();
        foreach(ScoreCache x in res) {
            cd.Add( new CalDay { Day = x.Day, Percentage = x.Percentage, Score = x.Score, Name = x.Name});
        }

        return cd;
    }

    public async void AddCache(Dictionary<int, CalDay> cd) {
       
       List<ScoreCache> sc = [];
       List<ScoreBreakdown> sb = [];
       foreach(var y in cd) {
        CalDay x = y.Value;
        sc.Add ( new ScoreCache { Gcc = Gcc, Month = Month, Year = Year, Day = x.Day, Name = x.Name, Percentage = x.Percentage, Score = x.Score });
        foreach(var d in x.Details) {
            sb.Add( new ScoreBreakdown { Gcc = Gcc, Month = Month,Day = x.Day, Message = d.Message, Year = Year, Sc = d.Sc });    
        }
       }

        await _Ctx.ScoreCache.AddRangeAsync(sc);
        await _Ctx.ScoreBreakdown.AddRangeAsync(sb);
        await _Ctx.SaveChangesAsync();
    }
}