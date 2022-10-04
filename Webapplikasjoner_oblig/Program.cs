
using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<TradingContext>();
builder.Services.AddScoped<ITradingRepository, TradingRepository>();

var app = builder.Build();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
