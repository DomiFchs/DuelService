namespace DtoLibrary.Dtos; 

public class DefaultUserStatDto {
    public int Id { get; set; }
    public int DuelsWon { get; set; }
    public int DuelsLost { get; set; }
    public int DuelsDrawn { get; set; }
    public int DuelsPlayed { get; set; }
    public DateTime LastPlayedAt { get; set; }
}