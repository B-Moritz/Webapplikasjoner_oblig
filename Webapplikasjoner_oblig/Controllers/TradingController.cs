
using Webapplikasjoner_oblig.DAL;
using Webapplikasjoner_oblig.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Webapplikasjoner_oblig.Controllers
{
    [Route("[controller]/[action]")]
    public class TradingController
    {
        private readonly ITradingRepository _db;

        public TradingController(ITradingRepository db)
        {
            _db = db;
        }
        public async Task<bool> SaveTrade(Trade innTrading)
        {
            return await _db.SaveTradeAsync(innTrading);
        }

        public async Task<List<Trade>> GetAllTrades()
        {
            return await _db.GetAllTradesAsync();
        }

        public async Task<Trade> HentEnTrading(int id)
        {
            return await _db.GetOneTradeAsync(id);
        }

    }
}
