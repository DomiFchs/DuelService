using DtoLibrary.Dtos;
using DtoLibrary.Extensions;
using RegistrationModel.Entities;

namespace RegistrationService.Extensions; 

public static class UserExtensions {
    
    public static User ToEntity(this CreateUserDto dto) {
        return new User {
            Name = dto.Name
        };
    }

    public static User ToEntity(this DefaultUserDto dto) {
        return new User() {
            Id = dto.Id,
            Name = dto.Name,
            EloRating = dto.EloRating,
            State = dto.State
        };
    }
    
    public static DefaultUserDto ToDefaultDto(this User entity, Uri baseAddress) {
        return new DefaultUserDto {
            Id = entity.Id,
            Name = entity.Name,
            EloRating = entity.EloRating,
            State = entity.State,
            Actions = entity.State.GetActionsForPlayer(entity.Id, baseAddress.ToString())
        };
    }

    public static SmallUserDto ToSmallUserDto(this User user, string relativePath) {
        return new SmallUserDto {
            UserId = user.Id,
            UserName = user.Name,
            Url = $"{relativePath}/{user.Id}"
        };
    }
    
    public static SmallUserDto ToSmallUserDto(this DefaultUserDto user, string relativePath) {
        return new SmallUserDto {
            UserId = user.Id,
            UserName = user.Name,
            Url = $"{relativePath}/{user.Id}"
        };
    }
    
    public static UpdateUserDto ToUpdateDto(this User entity) {
        return new UpdateUserDto {
            Id = entity.Id,
            Name = entity.Name,
            EloRating = entity.EloRating,
            State = entity.State
        };
    }
    
    public static UpdateUserDto ToUpdateDto(this DefaultUserDto dto) {
        return new UpdateUserDto {
            Id = dto.Id,
            Name = dto.Name,
            EloRating = dto.EloRating,
            State = dto.State
        };
    }
    
    public static User ToEntity(this UpdateUserDto dto) {
        return new User {
            Id = dto.Id,
            Name = dto.Name,
            EloRating = dto.EloRating,
            State = dto.State
        };
    }
}