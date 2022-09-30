namespace AlphaVantageInterface.Models {

    public class ConnectionStatus
    {
        public string? apiKey{ get; set;}
        public DateTime lastCallTime {get;set;}
        public int callCounter {get;set;}

        public string toString() 
        {
            string Out = String.Format("Alpha Vantage Connection Summary\n" + 
                                       "API key: {0} \n" + 
                                       "Last api call: {1} \n" + 
                                       "Api calls today: {3} \n", apiKey, lastCallTime, callCounter);
            return Out;
        }
    }


}