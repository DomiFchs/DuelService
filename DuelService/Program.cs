using DtoLibrary.Enums;
using DtoLibrary.Extensions;
using DuelService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();





builder.Services.AddHttpClient("RegistrationClient", client => {
    client.BaseAddress = builder.Configuration.BuildUri(EService.Registration);
});

builder.Services.AddHttpClient("PlayerStatClient",client => 
    client.BaseAddress = builder.Configuration.BuildUri(EService.PlayerStat)
);

builder.Services.AddHttpClient("MatchmakingClient",client => 
    client.BaseAddress = builder.Configuration.BuildUri(EService.Matchmaking)
);


builder.Services.AddHostedService<DuelBgService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();