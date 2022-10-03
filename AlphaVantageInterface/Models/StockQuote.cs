
using System.Text.Json.Serialization;

namespace AlphaVantageInterface.Models  {

    public class StockQuote
    {
        public string? Symbol {get; set;}
        public double Open {get; set;}
        public double High {get; set;}
        public double Low {get; set;}
        public double Price {get; set;}
        public int Volume {get; set;}
        public string? LatestTradingDay {get; set;}
        public double PreviousClose {get; set;}
        public double Change {get; set;}
        public string? ChangePercent {get; set;}

        public override string ToString()
        {
            return $"Stock quote of {Symbol}: \n" + 
                   $"Open: {Open}\nHigh: {High}\nLow: {Low}\nPrice: {Price}\nVolume: {Volume}\n" + 
                   $"Latest trading day: {LatestTradingDay}\nPrevious close: {PreviousClose}\nChange: {Change}\n" + 
                   $"Change percent: {ChangePercent}";
        }
    }

}