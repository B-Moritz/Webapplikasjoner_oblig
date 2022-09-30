using System.Text.Json.Serialization;

namespace AlphaVantageInterface.Models {

    public class Stock 
    {
        [JsonPropertyName("1. symbol")]
        public string? Symbol {get; set;}
        
        [JsonPropertyName("2. name")]
        public string? Name {get; set;}

        override public string ToString() 
        {
            if (Name == null) {
                Name = "Not found";
            }
            if (Symbol == null) {
                Symbol = "Not found";
            }
            string outString = String.Format("   Stock Symbol: {0, 30}\n   Stock name: {1, 30}\n", this.Symbol, this.Name);
            return outString;
        }
    }

}