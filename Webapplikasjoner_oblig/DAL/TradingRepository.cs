using System.Reflection;
using System.Text;
using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public class TradingRepository : ITradingRepository
    {
        private readonly TradingContext -db;

            public TradingRepository(TradingContext db)
        {
            _db = db;
        }
        public async Task<bool> Lagre(Trading innTrading)
        {
            try
            {
                var nyTradingRad = new Tradings();
                nyTradingRad = innTrading.StockName;
                nyTradingRad = innTrading.Dato;

                _db.Tradings.Add(nyTradingRad);
                await _db.SaveChangesAsync();
                return true;

            }
            catch
            {
                return false;
            }
        }



        public async Task<List<Trading>> HentAlleTrading()
        {
            try
            {
                List<Trading> alleTrading = await _db.Tradings.Select(k => new Trading
                {
                    Id = k.Id,
                    StockName = k.StockName,
                    Dato = k.Dato,

                }).ToListAsync();
                return alleTradings;
            }
            catch
            {
                return null;
            }
        }


        public async Task<Trading> HentEnTrading(int id)
        {
            Trading enTrading = await _db.Tradings.FindAsync(id);
            var hentetTrading = new Trading()
            {
                Id = enTrading.Id,
                StockName = enTrading.StockName,
                Dato = enTrading.Dato,

            };
            return hentetTrading;
        }

    }
}