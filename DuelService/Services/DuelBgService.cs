using System.Net;
using DtoLibrary.Dtos;

namespace DuelService.Services;

public class DuelBgService : BackgroundService {
    private readonly ILogger<DuelBgService> _logger;
    private readonly HttpClient _matchmakingHttpClient;
    private readonly HttpClient _playerStatHttpClient;
    private readonly HttpClient _registrationHttpClient;
    
    public DuelBgService(ILogger<DuelBgService> logger, IHttpClientFactory httpClientFactory) {
        _logger = logger;
        _matchmakingHttpClient = httpClientFactory.CreateClient("MatchmakingClient");
        _playerStatHttpClient = httpClientFactory.CreateClient("PlayerStatClient");
        _registrationHttpClient = httpClientFactory.CreateClient("RegistrationClient");
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            _logger.LogInformation("Starting Duels!");
            var matchmakingResponse = await _matchmakingHttpClient.GetAsync("", stoppingToken);
            var playerStatsResponse = await _playerStatHttpClient.GetAsync("", stoppingToken);
            var playerResponse = await _registrationHttpClient.GetAsync(_registrationHttpClient.BaseAddress + "/registeredPlayers", stoppingToken);
            _logger.LogInformation(_registrationHttpClient.BaseAddress + "/players");
            if(matchmakingResponse.StatusCode != HttpStatusCode.OK || playerStatsResponse.StatusCode != HttpStatusCode.OK || playerResponse.StatusCode != HttpStatusCode.OK) {
                _logger.LogError("Error while fetching data from other services");
                await Task.Delay(10_000, stoppingToken);
                continue;
            }
            var duels = await matchmakingResponse.Content.ReadFromJsonAsync<IEnumerable<DuelDataDto>>(cancellationToken: stoppingToken);
            var playerStats = await playerStatsResponse.Content.ReadFromJsonAsync<IEnumerable<DefaultUserStatDto>>(cancellationToken: stoppingToken);
            var players = await playerResponse.Content.ReadFromJsonAsync<IEnumerable<DefaultUserDto>>(cancellationToken: stoppingToken);

            if (duels == null || playerStats == null || players == null) {
                _logger.LogError("Error while deserializing data from other services");
                await Task.Delay(10_000, stoppingToken);
                continue;
            }
            var defaultUserStatDtos = playerStats as DefaultUserStatDto[] ?? playerStats.ToArray();
            var defaultUserDtos = players as DefaultUserDto[] ?? players.ToArray();
            
            foreach (var duel in duels) {
                var p1Stat = defaultUserStatDtos.FirstOrDefault(stat => stat.Id == duel.Player1Id);
                var p2Stat = defaultUserStatDtos.FirstOrDefault(stat => stat.Id == duel.Player2Id);
                
                var p1 = defaultUserDtos.FirstOrDefault(player => player.Id == duel.Player1Id);
                var p2 = defaultUserDtos.FirstOrDefault(player => player.Id == duel.Player2Id);
                
                if (p1Stat == null || p2Stat == null) continue;

                var delta = CalculateEloDelta(p1!.EloRating, p2!.EloRating);
                
                p1.EloRating += delta;
                p2.EloRating -= delta;
                
                switch (delta) {
                    case > 0:
                        p1Stat.DuelsWon++;
                        p2Stat.DuelsLost++;
                        break;
                    case < 0:
                        p1Stat.DuelsWon++;
                        p2Stat.DuelsLost++;
                        break;
                    default:
                        p1Stat.DuelsDrawn++;
                        p2Stat.DuelsDrawn++;
                        break;
                }
            }
            
            await _playerStatHttpClient.PostAsJsonAsync(_playerStatHttpClient.BaseAddress + "/list", defaultUserStatDtos, stoppingToken);
            await _registrationHttpClient.PostAsJsonAsync(_registrationHttpClient.BaseAddress + "/players", defaultUserDtos, stoppingToken);
            _logger.LogInformation("Finished Duels!");
            await Task.Delay(10_000, stoppingToken);
        }
    }

    private static double ExpectationToWin(float playerOneRating, float playerTwoRating) {
        return 1 / (1 + Math.Pow(10, (playerTwoRating - playerOneRating) / 400.0));
    }

    private static float CalculateEloDelta(float playerOneRating, float playerTwoRating) {
        const int eloK = 40;

        return (float)(eloK * (1 - ExpectationToWin(playerOneRating, playerTwoRating)));
    }
}