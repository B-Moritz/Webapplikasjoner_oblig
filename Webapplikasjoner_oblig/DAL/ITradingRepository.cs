using System.Text;
using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public interface ITradingRepository
    {
        Task<bool> SaveTradeAsync(Trade innTrading);
        Task<List<Trade>> GetAllTradesAsync();
        Task<Trade> GetOneTradeAsync(int id);

        // okab...
        Task<List<Portfolio>> GetPortfolio(string symbol, DateTime startDate, DateTime endDate);

        Task<List<FavoriteList>> GetFavoriteList(int userId);

    }

}
