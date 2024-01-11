using DtoLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MatchmakingService.Controllers; 

[ApiController]
[Route("matchmaking")]
public class MatchmakingController : ControllerBase {
    
    private readonly HashSet<int> _matchedIds = new();

    private readonly ILogger<MatchmakingController> _logger;
    private readonly HttpClient _playerStatHttpClient;
    private readonly HttpClient _registrationHttpClient;
    public MatchmakingController(ILogger<MatchmakingController> logger, IHttpClientFactory httpClientFactory) {
        _logger = logger;
        _playerStatHttpClient = httpClientFactory.CreateClient("PlayerStatClient");
        _registrationHttpClient = httpClientFactory.CreateClient("RegistrationClient");
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DuelDataDto>>> GetMatches(CancellationToken ct) {
        _matchedIds.Clear();
        var playerResponse = await _registrationHttpClient.GetAsync(_registrationHttpClient.BaseAddress + "/registeredPlayers",ct);
        var players = await playerResponse.Content.ReadFromJsonAsync<IEnumerable<DefaultUserDto>>(cancellationToken: ct);
        
        var statsResponse = await _playerStatHttpClient.GetAsync(_playerStatHttpClient.BaseAddress + "/registeredPlayerStats", ct);
        var userStats = await statsResponse.Content.ReadFromJsonAsync<IEnumerable<DefaultUserStatDto>>(cancellationToken: ct);
        
        if (players == null) return NotFound("No players found!");
        if (userStats == null) return NotFound("No user stats found!");
        
        var sortedUsers = players.OrderBy(u => u.EloRating).ToList();
        var sortedUserStats = userStats.OrderBy(stat => stat.LastPlayedAt).ToList();
        
        var duels = new List<DuelDataDto>();
        foreach (var user in sortedUserStats)
        {
            var eligibleOpponents = sortedUsers.Where(u => u.Id != user.Id && !IsAlreadyMatched(user.Id, u.Id)).ToList();

            if (eligibleOpponents.Count <= 0) continue;

            var nearestOpponent = eligibleOpponents.OrderBy(opponent => Math.Abs(opponent.EloRating - sortedUsers.FirstOrDefault(u => u.Id == user.Id)!.EloRating)).First();

            var duel = new DuelDataDto
            {
                Player1Id = user.Id,
                Player2Id = nearestOpponent.Id
            };

            duels.Add(duel);
            _matchedIds.Add(user.Id);
            _matchedIds.Add(nearestOpponent.Id);
        }
        _logger.LogInformation($"Found {duels.Count} pairs to duel!");
        return duels;
    }
    
    private bool IsAlreadyMatched(int playerId1, int playerId2)
    {
        return _matchedIds.Contains(playerId1) || _matchedIds.Contains(playerId2);
    }
}