using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RegistrationModel.Enums;

namespace RegistrationModel.Entities; 

[Table("USERS")]
public class User {

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("USER_ID")]
    public int Id { get; set; }

    [Column("USERNAME")]
    public string Name { get; set; } = null!;
    
    [Column("ELO_RATING")]
    public float EloRating { get; set; }
    
    [Column("STATE")]
    public EPlayerState State { get; set; }
}