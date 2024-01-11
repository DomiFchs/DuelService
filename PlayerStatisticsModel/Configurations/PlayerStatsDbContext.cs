using Microsoft.EntityFrameworkCore;
using PlayerStatisticsModel.Entities;

namespace PlayerStatisticsModel.Configurations; 

public class PlayerStatsDbContext : DbContext{
    
    public PlayerStatsDbContext(DbContextOptions<PlayerStatsDbContext> options) : base(options) {
    }

    public DbSet<UserStat> UserStats { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
    }
}