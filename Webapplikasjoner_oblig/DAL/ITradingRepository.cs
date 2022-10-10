using System.Text;
using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public interface ITradingRepository
    {
        Task<bool> SaveTradeAsync(Trade innTrading);
        Task<List<Trade>> GetAllTradesAsync();
        Task<Trade> GetOneTradeAsync(int id);
        Task<FavoriteList> GetFavoriteList(int userId);
        Task<Portfolio> GetPortfolioAsync(int userId);

    }

}
