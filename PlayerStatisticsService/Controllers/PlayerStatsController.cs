using System.Net;
using DtoLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using PlayerStatisticsDomain.Repositories.Interfaces;
using PlayerStatisticsService.Extensions;

namespace PlayerStatisticsService.Controllers; 

[ApiController]
[Route("stats")]
public class PlayerStatsController : ControllerBase {

    private readonly IPlayerStatsRepository _playerStatsRepository;
    private readonly ILogger<PlayerStatsController> _logger;
    private readonly HttpClient _registrationHttpClient;

    public PlayerStatsController(IPlayerStatsRepository playerStatsRepository, ILogger<PlayerStatsController> logger, IHttpClientFactory httpClientFactory){
        _playerStatsRepository = playerStatsRepository;
        _registrationHttpClient = httpClientFactory.CreateClient("RegistrationClient");
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<DefaultUserStatDto>>> GetStats(CancellationToken ct) {
        var stats = await _playerStatsRepository.ReadAsync(ct);
        _logger.LogInformation($"Found {stats.Count} stats!");
        return stats.Select(s => s.ToDefaultDto()).ToList();
    }
    
    [HttpPost]
    public async Task<ActionResult<DefaultUserStatDto>> CreateStat(DefaultUserStatDto statDto, CancellationToken ct) {
        var stat = statDto.ToEntity();
        
        var created = await _playerStatsRepository.CreateAsync(stat, ct);
        _logger.LogInformation($"Created stat for player with id {created.Id}!");
        return created.ToDefaultDto();
    }
    
    [HttpPost("list")]
    public async Task<ActionResult> UpdateStats([FromBody] List<DefaultUserStatDto>? updatedStats, CancellationToken ct)
    {
        if (updatedStats == null)
            return BadRequest("Invalid data in the request body");
        
        await _playerStatsRepository.UpdateAsync(updatedStats.Select(s => s.ToEntity()), ct);
        _logger.LogInformation($"Updated {updatedStats.Count} stats!");
        return Ok();
    }
    
    [HttpGet("registeredPlayerStats")]
    public async Task<ActionResult<IEnumerable<DefaultUserStatDto>>> GetRegisteredStats(CancellationToken ct) {
        var registrationResponse = await _registrationHttpClient.GetAsync(_registrationHttpClient.BaseAddress +"/registeredPlayers", ct);

        var players = await registrationResponse.Content.ReadFromJsonAsync<IEnumerable<DefaultUserDto>>(cancellationToken: ct);
        if (players == null) return NotFound("No players found!");
        
        var stats = await _playerStatsRepository.ReadAsync(ct);
        var playerIdList = players.Select(p => p.Id).ToList();
        _logger.LogInformation($"Found {playerIdList.Count} registered players!");
        return stats.Where(s => playerIdList.Contains(s.Id)).Select(s => s.ToDefaultDto()).ToList();
    }
}