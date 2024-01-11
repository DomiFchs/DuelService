using DtoLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using RegistrationDomain.Repositories.Interfaces;
using RegistrationModel.Enums;
using RegistrationService.Extensions;

namespace RegistrationService.Controllers; 

[ApiController]
[Route("users")]
public class RegistrationController : ControllerBase{

    private readonly ILogger<RegistrationController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly HttpClient _playerStatsClient;


    public RegistrationController(ILogger<RegistrationController> logger, IUserRepository userRepository, IHttpClientFactory httpClientFactory) {
        _logger = logger;
        _userRepository = userRepository;
        _playerStatsClient = httpClientFactory.CreateClient("PlayerStatsClient");
    }
    
    [HttpGet("players")]
    public async Task<IEnumerable<SmallUserDto?>> ListPlayers(CancellationToken ct) {
        var users = await _userRepository.ReadAsync(ct);

        var userDtos = users.Select(u => u.ToSmallUserDto(_playerStatsClient.BaseAddress!.ToString()));
        var smallUserDtos = userDtos as SmallUserDto[] ?? userDtos.ToArray();
        _logger.LogInformation($"Found {smallUserDtos.Count()} players!");
        return smallUserDtos;
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DefaultUserDto?>> GetPlayer(int id, CancellationToken ct) {
        var user = await _userRepository.ReadAsync(id, ct);
        if (user is null) return NotFound();

        var userDto = user.ToDefaultDto(_playerStatsClient.BaseAddress!);
        _logger.LogInformation($"Found player with id {userDto.Id}!");
        return userDto;
    }
    
    [HttpPost("registration")]
    public async Task<ActionResult<DefaultUserDto>> CreatePlayer(CreateUserDto userDto, CancellationToken ct) {
        var user = userDto.ToEntity();
        user.EloRating = 1500f;
        
        var created = await _userRepository.CreateAsync(user, ct);
        var stat = new DefaultUserStatDto() {
            Id = created.Id,
            DuelsWon = 0,
            DuelsLost = 0,
            DuelsDrawn = 0,
            DuelsPlayed = 0,
            LastPlayedAt = DateTime.Now
        };
        await _playerStatsClient.PostAsJsonAsync("", stat, ct);
        _logger.LogInformation($"Created player with id {created.Id}!");
        return created.ToDefaultDto(_playerStatsClient.BaseAddress!);
    }
    
    [HttpPost("players")]
    public async Task<ActionResult> UpdatePlayer([FromBody] List<UpdateUserDto>? updatedStats, CancellationToken ct)
    {
        if (updatedStats == null)
            return BadRequest("Invalid data in the request body");
        
        
        await _userRepository.UpdateAsync(updatedStats.Select(s => s.ToEntity()), ct);
        _logger.LogInformation($"Updated {updatedStats.Count} players!");
        return Ok();
    }
        
    [HttpGet("registeredPlayers")]
    public async Task<IEnumerable<DefaultUserDto>> ListRegisteredPlayers(CancellationToken ct) {
        var users = await _userRepository.GetRegisteredUsersAsync(ct);
        var userDtos = users.Select(u => u.ToDefaultDto(_playerStatsClient.BaseAddress!));
        var listRegisteredPlayers = userDtos as DefaultUserDto[] ?? userDtos.ToArray();
        _logger.LogInformation($"Found {listRegisteredPlayers.Count()} registered players!");
        return listRegisteredPlayers;
    }
    
    [HttpPost("register/{id:int}")]
    public async Task<ActionResult> RegisterPlayer(int id, CancellationToken ct) {
        var user = await _userRepository.ReadAsync(id, ct);
        if (user is null) return NotFound();
        user.State = EPlayerState.Registered;
        await _userRepository.UpdateAsync(user, ct);
        _logger.LogInformation($"Registered player with id {user.Id}!");
        return Ok();
    }
    
    [HttpPost("delete/{id:int}")]
    public async Task<ActionResult> DeletePlayer(int id, CancellationToken ct) {
        var user = await _userRepository.ReadAsync(id, ct);
        if (user is null) return NotFound();
        user.State = EPlayerState.Deleted;
        await _userRepository.UpdateAsync(user, ct);
        _logger.LogInformation($"Deleted player with id {user.Id}!");
        return Ok();
    }
}