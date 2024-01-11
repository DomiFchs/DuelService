using DtoLibrary.Enums;
using DtoLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpClient("PlayerStatClient",client => 
    client.BaseAddress = builder.Configuration.BuildUri(EService.PlayerStat)
);

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