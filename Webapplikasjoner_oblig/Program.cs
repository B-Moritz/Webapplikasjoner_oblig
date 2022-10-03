
using Microsoft.EntityFrameworkCore;
using Webapplikasjoner_oblig.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<TradingContext>(options => options.UseSqlite("Name=WebApiDatabase"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
