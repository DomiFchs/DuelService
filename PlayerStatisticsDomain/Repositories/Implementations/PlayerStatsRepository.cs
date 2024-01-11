using PlayerStatisticsDomain.Repositories.Interfaces;
using PlayerStatisticsModel.Configurations;
using PlayerStatisticsModel.Entities;

namespace PlayerStatisticsDomain.Repositories.Implementations; 

public class PlayerStatsRepository : ARepository<UserStat>, IPlayerStatsRepository {
    public PlayerStatsRepository(PlayerStatsDbContext context) : base(context) { }
}