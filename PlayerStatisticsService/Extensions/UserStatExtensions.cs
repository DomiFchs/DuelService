using DtoLibrary.Dtos;
using PlayerStatisticsModel.Entities;

namespace PlayerStatisticsService.Extensions; 

public static class UserStatExtensions {


    public static UserStat ToEntity(this DefaultUserStatDto dto) {
        return new UserStat {
            Id = dto.Id,
            DuelsWon = dto.DuelsWon,
            DuelsLost = dto.DuelsLost,
            DuelsDrawn = dto.DuelsDrawn,
            DuelsPlayed = dto.DuelsPlayed,
            LastPlayedAt = dto.LastPlayedAt
        };
    }

    public static DefaultUserStatDto ToDefaultDto(this UserStat userStat) {
        return new DefaultUserStatDto() {
            Id = userStat.Id,
            DuelsWon = userStat.DuelsWon,
            DuelsLost = userStat.DuelsLost,
            DuelsDrawn = userStat.DuelsDrawn,
            DuelsPlayed = userStat.DuelsPlayed,
            LastPlayedAt = userStat.LastPlayedAt
        };
    }
}