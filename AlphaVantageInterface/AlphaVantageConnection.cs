//System namespaces
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

// Library namespaces
using AlphaVantageInterface.Models;

namespace AlphaVantageInterface {

    public class AlphaVantageConnection
    {
        private readonly string _apiKey;
        private readonly HttpClient _cli = new HttpClient();
        private bool IsLimited;
        private static readonly string _connectionStatusCache = "ConnectionStatusCache.json"; 
        private readonly string _baseUri = "https://www.alphavantage.co/query?";
        // Deserialization options
        private readonly JsonSerializerOptions _globalJsonOptions = new JsonSerializerOptions {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
        private ConnectionStatus LatestStatus;
        
        private static int CallLimitDaily = 122;

        public AlphaVantageConnection(string apiKey, bool isLimited)
        {
            ConnectionStatus initialStatus = new ConnectionStatus {
                ApiKey = apiKey,
                IsLimited = isLimited
            };
            this.LatestStatus = initialStatus;
            this._apiKey = apiKey;
            this.IsLimited = isLimited;
        }

        public static async Task<AlphaVantageConnection> BuildAlphaVantageConnection(string apiKey, bool isLimited)
        {
            AlphaVantageConnection newConnection = new AlphaVantageConnection(apiKey, isLimited);
            // Load the temporary stored data for the api connection and verify that calls can be made
            await newConnection.verifyStatusCacheAsync();
            return newConnection;
        }

        public ConnectionStatus getConnectionStatus() {
            // Create the summary object
            return LatestStatus;
        }

        // The http rest client used is based on the alpha vantage documentation and the microsoft documentation:
        //          https://www.alphavantage.co/documentation/
        //          https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient
        public async Task<SearchResult> findStockAsync(string keyword)
        {
            string function = "SYMBOL_SEARCH";
            string searchUri = $"{_baseUri}function={function}&keywords={keyword}&apikey={_apiKey}";

            return await makeApiCallAsync<SearchResult>(searchUri);
        }

        // This method takes a stock object as argument and adds quote data to the object.
        public async Task getStockQuoteAsync(Stock stock) {
            if (stock.Symbol is null) {
                // If the stock object has no symbol, no api call can be made.
                throw new NullReferenceException("Stock symbol is null.");
            }

            StockQuote curQuote = await getStockQuoteAsync(stock.Symbol);
            stock.StockQuoteObject = curQuote;
        }

        public async Task<StockQuote> getStockQuoteAsync(string symbol) {
            string function = "GLOBAL_QUOTE";
            string requestUri = $"{this._baseUri}function={function}&symbol={symbol}&apikey={this._apiKey}";

            StockQuoteTemp quoteTemp = await makeApiCallAsync<StockQuoteTemp>(requestUri);

            if (quoteTemp.GlobalQuote is null) {
                throw new NullReferenceException("No global quote dictionary(property is null)");
            }
            
            // Create the StockQuote object:
            // Documentation used: https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonelement?view=net-6.0
            StockQuote resultObject = new StockQuote {
                Symbol = quoteTemp.GlobalQuote["01. symbol"].GetString(),
                Open = double.Parse(quoteTemp.GlobalQuote["02. open"].GetString().Replace(".", ",")),
                High = double.Parse(quoteTemp.GlobalQuote["03. high"].GetString().Replace(".", ",")),
                Low = double.Parse(quoteTemp.GlobalQuote["04. low"].GetString().Replace(".", ",")),
                Price = double.Parse(quoteTemp.GlobalQuote["05. price"].GetString().Replace(".", ",")),
                Volume = int.Parse(quoteTemp.GlobalQuote["06. volume"].GetString()),
                LatestTradingDay = quoteTemp.GlobalQuote["07. latest trading day"].GetString(),
                PreviousClose = double.Parse(quoteTemp.GlobalQuote["08. previous close"].GetString().Replace(".", ",")),
                Change = double.Parse(quoteTemp.GlobalQuote["09. change"].GetString().Replace(".", ",")),
                ChangePercent = quoteTemp.GlobalQuote["10. change percent"].GetString()
            };

            return resultObject;
        }

        private async Task<T> makeApiCallAsync<T>(string requestUri) {
            // Verify that the api key can be used
            await verifyStatusCacheAsync();

            var respStream = _cli.GetStreamAsync(requestUri);

            await addCallCounterAsync();

            // Deserialize the response
            T? resp = await JsonSerializer.DeserializeAsync<T>(await respStream, _globalJsonOptions);

            // Handle possible null pointer exception
            if (resp == null) {
                throw new JsonException("Error during deserialization. The result is null");
            }
            return resp;

        }

        private async Task addCallCounterAsync()
        {
            await updateStatusCacheAsync(1);
        }

        private async Task updateStatusCacheAsync(int addCounter = 0) {
            FileStream curStatusStream = File.Open(_connectionStatusCache, System.IO.FileMode.OpenOrCreate);
            // Read the cached status from file
            ConnectionStatus? curStatus = await JsonSerializer.DeserializeAsync<ConnectionStatus>(curStatusStream);
            
            if (curStatus is null) {
                // If the file is empty, add a new cache object
                curStatus = new ConnectionStatus {
                    ApiKey = this._apiKey,
                    LastCallTime = DateTime.Today,
                    CallCounter = addCounter,
                    IsLimited = this.IsLimited
                };
            }     
            else {
                cleanStatusCache(curStatus);               

                if (this.IsLimited && curStatus.CallCounter >= AlphaVantageConnection.CallLimitDaily) {
                    // Close file with no writing:
                    curStatusStream.Dispose();
                    throw new AlphaVantageApiCallNotPossible($"The number of calls today ({curStatus.LastCallTime.Date.ToString()}) " + 
                                            $"have reached the limit. Counted {curStatus.CallCounter} compared to " + 
                                            $"{AlphaVantageConnection.CallLimitDaily} possible!");
                }
            }

            curStatus.CallCounter += addCounter;
            
            // Change write possition:
            curStatusStream.Seek(0, SeekOrigin.Begin);
            
            // Write to file
            await JsonSerializer.SerializeAsync<ConnectionStatus>(curStatusStream, curStatus);
            curStatusStream.Dispose();
            this.LatestStatus = curStatus;
        }

        private async Task<ConnectionStatus> readStatusFileAsync() 
        {
            ConnectionStatus? latestStatus;
            Stream statusFileStream;
            try {
                statusFileStream = File.OpenRead(_connectionStatusCache);
                latestStatus = await JsonSerializer.DeserializeAsync<ConnectionStatus>(statusFileStream);
            } catch (FileNotFoundException) 
            {
                // Create the infoCache file
                statusFileStream =  File.Create(_connectionStatusCache);
                latestStatus = null;
            }
            
            if (latestStatus is null) {
                // Write to file and return a custom info object
                ConnectionStatus initialStatus = new ConnectionStatus {
                    ApiKey = this._apiKey,
                    CallCounter = 0,
                    LastCallTime = DateTime.Today,
                    IsLimited = this.IsLimited
                };
                // Write the custom infoCache to file
                await JsonSerializer.SerializeAsync<ConnectionStatus>(statusFileStream, initialStatus);
                statusFileStream.Dispose();
                return initialStatus;
            }
            statusFileStream.Dispose();
            return latestStatus;
        }

        private void cleanStatusCache(ConnectionStatus curStatus)
        {
            if (DateTime.Today.Date == curStatus.LastCallTime.AddDays(1).Date) 
                {
                    // The last call was yesterday. Set call counter to 0
                    curStatus.CallCounter = 0;
                    curStatus.LastCallTime = DateTime.Today;                
                }
        }

        private async Task verifyStatusCacheAsync()
        {
            ConnectionStatus curStatus = await readStatusFileAsync();
            // Make sure that call counter is from today
            cleanStatusCache(curStatus);

            if (this.IsLimited && curStatus.CallCounter >= AlphaVantageConnection.CallLimitDaily) {
                    throw new AlphaVantageApiCallNotPossible($"The number of calls today ({curStatus.LastCallTime.Date.ToString()}) " + 
                                            $"have reached the limit. Counted {curStatus.CallCounter} compared to " + 
                                            $"{AlphaVantageConnection.CallLimitDaily} possible!");
            }
            // Update the LatestStatus property with the status info which has been verified
            this.LatestStatus = curStatus;
        }
    }

    public class AlphaVantageApiCallNotPossible : Exception
    {
        // Defining custom exceptions: 
        // https://learn.microsoft.com/en-us/dotnet/standard/exceptions/how-to-create-user-defined-exceptions
        public AlphaVantageApiCallNotPossible() {}

        public AlphaVantageApiCallNotPossible(string msg) : base(msg) {}

        public AlphaVantageApiCallNotPossible(string msg, Exception inner) : base(msg, inner) {}
    }
}

