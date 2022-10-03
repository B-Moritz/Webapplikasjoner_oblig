
using Webapplikasjoner_oblig.DAL;
using Webapplikasjoner_oblig.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Webapplikasjoner_oblig.Controllers
{
    [Route("[controller]/[action]")]
    public class TradingController
    {
        private readonly TradingContext _db;
        // private readonly ITradingRepository _db som det er i repository eksempel;


        public TradingController(TradingContext db)
        // public TradingController(ITradingRepository db)
        {
            _db = db;
        }
        public async Task<bool> Lagre(Trading innTrading)
        {
            return await _db.Lagre(innTrading);
        }

        public async Task<List<Trading>> HentAlleTrading()
        {
            return await _db.HentAlleTrading();
        }

        public async Task<Trading> HentEnTrading(int id)
        {
            return await _db.HentEnTrading(id);
        }

    }
}
