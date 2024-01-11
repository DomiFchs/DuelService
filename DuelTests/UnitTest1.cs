using System.Net.Http.Json;
using DtoLibrary.Dtos;
using Newtonsoft.Json;
using RegistrationModel.Enums;
using Xunit.Abstractions;

namespace Help;

public class UnitTest1 {
    private readonly ITestOutputHelper _testOutputHelper;

    private const string BaseUrl = "https://localhost:7137";
    private readonly HttpClient _httpClient = new();
    public UnitTest1(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public async Task ListPlayers()
    {
        var response = await _httpClient.GetAsync($"https://localhost:7167/users/players");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var players = JsonConvert.DeserializeObject<List<DefaultUserDto>>(content);

        _testOutputHelper.WriteLine(players.Count.ToString());
        
        //write test 
        
    }

    [Fact]
    public async Task GetPlayer()
    {
        const int playerId = 5;
        var response = await _httpClient.GetAsync($"https://localhost:7167/users/{playerId}");
        response.EnsureSuccessStatusCode();

        
        var content = await response.Content.ReadAsStringAsync();
        var player = JsonConvert.DeserializeObject<DefaultUserDto>(content);
        
        _testOutputHelper.WriteLine(player.Name);
    }

    [Fact]
    public async Task CreatePlayer()
    {
        var createUserDto = new CreateUserDto
        {
            Name = "TestUser"
        };

        var response = await _httpClient.PostAsJsonAsync($"https://localhost:7167/users/registration", createUserDto);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        var player = JsonConvert.DeserializeObject<DefaultUserDto>(content);
        
        _testOutputHelper.WriteLine(player.Name);
    }
}