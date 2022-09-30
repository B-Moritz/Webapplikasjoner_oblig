using System.Text.Json.Serialization;

namespace AlphaVantageInterface.Models {

    public class SearchResult 
    {
        public int Id {get; set;}

        [JsonPropertyName("bestMatches")]
        public List<Stock>? BestMatches {get; set;}


        override public string ToString() 
        {
            string outString = $"Search Result nr: {this.Id}\n";
            
            if (BestMatches == null)  {
                return "The search result is empy";
            }

            for (int i = 1; i <= BestMatches.Count; i++) {
                outString += String.Format("Stock {0}: \n{1}", i, this.BestMatches[i-1].ToString());
            }
            return outString;
        }
    }

}