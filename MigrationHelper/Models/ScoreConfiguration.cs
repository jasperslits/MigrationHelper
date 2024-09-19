namespace MigrationHelper.Models;

public enum ScoreConfiguration : int
{

    Weekend = 0,
   CutOff = -2,
   PayDate = -2,
    NextPayDate = -1,
    Free = 4,
    FreeAfterClose = 4,
    BlockedAfterClose = -4,

    CutOffBlackout = -2
}