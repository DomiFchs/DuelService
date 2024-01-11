using System.ComponentModel.DataAnnotations.Schema;

namespace PlayerStatisticsModel.Entities; 

[Table("USER_STATISTICS")]
public class UserStat {

    [Column("USER_ID")]
    public int Id { get; set; }

    [Column("DUELS_WON")]
    public int DuelsWon { get; set; }
    
    [Column("DUELS_LOST")]
    public int DuelsLost { get; set; }
    
    [Column("DUELS_DRAWN")]
    public int DuelsDrawn { get; set; }
    
    [Column("DUELS_PLAYED")]
    public int DuelsPlayed { get; set; }
    
    [Column("LAST_PLAYED_AT")]
    public DateTime LastPlayedAt { get; set; }
}