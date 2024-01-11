using DtoLibrary.Enums;
using DtoLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using RegistrationDomain.Repositories.Implementations;
using RegistrationDomain.Repositories.Interfaces;
using RegistrationModel.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<RegistrationDbContext>(
    options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(8, 0, 31))
        ).UseLoggerFactory(new NullLoggerFactory()),
    ServiceLifetime.Transient
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddHttpClient("PlayerStatsClient",client => 
    client.BaseAddress = builder.Configuration.BuildUri(EService.PlayerStat)
);


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