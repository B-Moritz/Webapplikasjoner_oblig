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
        Stocks GetStock(string symbol);
        StockQuotes GetStockQuote(string symbol);

        Task<bool> RemoveStocks(int userId, StockQuote stock, int count);

        Task AddStockQuoteAsync(StockQuote stockQuote, Stocks stock);
        Task<FavoriteList> GetFavoriteList(int userId);
        Task<Portfolio> GetPortfolioAsync(int userId);

    }

}
