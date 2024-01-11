using RegistrationModel.Enums;

namespace DtoLibrary.Dtos; 

public class UpdateUserDto {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public float EloRating { get; set; }
    public EPlayerState State { get; set; }
}