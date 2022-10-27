
using Microsoft.EntityFrameworkCore;
using TradingSchemaSp;
using Webapplikasjoner_oblig.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddDbContext<TradingContext>();
builder.Services.AddScoped<ITradingRepository, TradingRepository>();
builder.Services.AddScoped<ISearchResultRepositry, SearchResultRepositry>();
// Adding timed background service to clean the database once per day
builder.Services.AddHostedService<TradingSchemaWorker>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
//AddNewtonsoftJson
