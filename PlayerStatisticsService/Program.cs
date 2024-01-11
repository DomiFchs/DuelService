using DtoLibrary.Enums;
using DtoLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using PlayerStatisticsModel.Configurations;
using PlayerStatisticsDomain.Repositories.Implementations;
using PlayerStatisticsDomain.Repositories.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextFactory<PlayerStatsDbContext>(
    options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(8, 0, 31))
        ).UseLoggerFactory(new NullLoggerFactory()),
    ServiceLifetime.Transient
);

builder.Services.AddScoped<IPlayerStatsRepository, PlayerStatsRepository>();

builder.Services.AddHttpClient("RegistrationClient", client => {
    client.BaseAddress = builder.Configuration.BuildUri(EService.Registration);
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();