using System.ComponentModel.DataAnnotations;

namespace MigrationHelper.Models;

public class ScoreConfig
{
    [Key]
    public int Id { get; set; }

    [Range(-50,4,  ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Saturday { get; set; } = -5;
    [Range(-50,4,  ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Sunday { get; set; } = -10;
    [Range(-50,4,  ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int CutOff  { get; set; } = -2;
    [Range(-50,4,  ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int PayDate { get; set; } = -2;
    [Range(-50,4,  ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int NextPayDate { get; set; } = -1;
    [Range(-50,4,  ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Free { get; set; } = 4;
    [Range(-50,4,  ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int FreeAfterClose { get; set; } = 4;
    [Range(-50,4,  ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int BlockedAfterClose { get; set; } = -4;
    [Range(-50,4,  ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int CutOffBlackout { get; set; } = -2;
}
