using System.Text;
using AlphaVantageInterface.Models;
using Webapplikasjoner_oblig.Model;

namespace Webapplikasjoner_oblig.DAL
{
    public interface ITradingRepository
    {
        Task<bool> SaveTradeAsync(Trade innTrading);
        Task<List<Trade>> GetAllTradesAsync();
        Task<Trade> GetOneTradeAsync(int id);
        Task<Stocks> GetStockAsync(string symbol);
        StockQuotes GetStockQuote(string symbol);

        Task SellStockTransactionAsync(int userId, string symbol, decimal saldo, int count);
        Task BuyStockTransactionAsync(User curUser, Stocks curStock, decimal saldo, int count);
        void RemoveStockQuotes(string symbol);

        Task<User> GetUserAsync(int userId);
        Task<StockQuotes> AddStockQuoteAsync(StockQuote stockQuote);
        Task<FavoriteList> GetFavoriteList(int userId);
        Task AddToFavoriteListAsync(int userId, string symbol);
        Task DeleteFromFavoriteListAsync(int userId, string symbol);

        Task<Users> GetPortfolioAsync(int userId);

    }

}
