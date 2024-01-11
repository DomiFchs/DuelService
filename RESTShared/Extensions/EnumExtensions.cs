using DtoLibrary.Enums;
using RegistrationModel.Enums;

namespace DtoLibrary.Extensions; 

public static class EnumExtensions {

    public static string[] GetActionsForPlayer(this EPlayerState playerState,int userId, string baseUri) {
        return playerState switch {
            EPlayerState.Pending => new[] {$"{baseUri}/delete/{userId}", $"{baseUri}/register/{userId}"},
            EPlayerState.Registered => new[] {$"{baseUri}/delete/{userId}"},
            EPlayerState.Deleted => new[] {$"{baseUri}/register/{userId}"},
            _ => throw new ArgumentOutOfRangeException(nameof(playerState), playerState, null)
        };
    }

    public static string ToEnumString(this EService service) {
        return service switch {
            EService.Matchmaking => "Matchmaking",
            EService.PlayerStat => "PlayerStat",
            EService.Registration => "Registration",
            _ => throw new ArgumentOutOfRangeException(nameof(service), service, null)
        };
    }
}