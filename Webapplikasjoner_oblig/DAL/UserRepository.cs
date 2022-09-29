using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public class UserRepository
    {
        private readonly TradingContext _tradingContext;
        public UserRepository(TradingContext tradingContext)
        {
            _tradingContext = tradingContext;
        }
     
    }
}
