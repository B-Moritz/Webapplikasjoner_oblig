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
        private bool _isLimited;

        private static readonly string _connectionStatusCache = "ConnectionStatusCache.json"; 

        private readonly string _baseUri = "https://www.alphavantage.co/query?";
        private static int CallLimitDaily = 10;
        private ConnectionStatus LatestStatus;

        private readonly HttpClient cli = new HttpClient();

        public AlphaVantageConnection(string apiKey, bool isLimited)
        {
            ConnectionStatus initialStatus = new ConnectionStatus {
                ApiKey = apiKey,
                IsLimited = isLimited
            };
            this.LatestStatus = initialStatus;
            this._apiKey = apiKey;
            this._isLimited = isLimited;
        }

        public static async Task<AlphaVantageConnection> BuildAlphaVantageConnection(string apiKey, bool isLimited)
        {
            AlphaVantageConnection newConnection = new AlphaVantageConnection(apiKey, isLimited);
            // Load the temporary stored data for the api connection and verify that calls can be made
            await newConnection.verifyStatusCache();
            return newConnection;
        }

        public ConnectionStatus getConnectionStatus() {
            // Create the summary object
            return LatestStatus;
        }

        // The http rest client used is based on the alpha vantage documentation:
        //          https://www.alphavantage.co/documentation/
        public async Task<SearchResult> findStockAsync(string keyword)
        {
            string function = "SYMBOL_SEARCH";
            string searchUri = $"{_baseUri}function={function}&keywords={keyword}&apikey={_apiKey}";

            // Verify that antother api call is possible
            await verifyStatusCache();

            Stream resp = await cli.GetStreamAsync(searchUri);

            // Increment counter and update infoCache
            await addCallCounter();

            // Deserialization options
            var jsonOptions = new JsonSerializerOptions {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
            // Deserialize to SearchResult Object
            SearchResult? sRes = await JsonSerializer.DeserializeAsync<SearchResult>(resp, jsonOptions);

            if (sRes == null) {
                throw new JsonException("Error during deserialization. The result is null");
            }

            return sRes;
        }

        private async Task addCallCounter()
        {
            await updateStatusCache(1);
        }

        private async Task updateStatusCache(int addCounter = 0) {
            FileStream curStatusStream = File.Open(_connectionStatusCache, System.IO.FileMode.OpenOrCreate);
            // Read the cached status from file
            ConnectionStatus? curStatus = await JsonSerializer.DeserializeAsync<ConnectionStatus>(curStatusStream);
            
            if (curStatus is null) {
                // If the file is empty, add a new cache object
                curStatus = new ConnectionStatus {
                    ApiKey = this._apiKey,
                    LastCallTime = DateTime.Today,
                    CallCounter = addCounter,
                    IsLimited = this._isLimited
                };
            }     
            else {
                cleanStatusCache(curStatus);               

                if (this._isLimited && curStatus.CallCounter >= AlphaVantageConnection.CallLimitDaily) {
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

        private async Task<ConnectionStatus> readStatusFile() 
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
                    IsLimited = this._isLimited
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

        private async Task verifyStatusCache()
        {
            ConnectionStatus curStatus = await readStatusFile();
            // Make sure that call counter is from today
            cleanStatusCache(curStatus);

            if (this._isLimited && curStatus.CallCounter >= AlphaVantageConnection.CallLimitDaily) {
                    throw new AlphaVantageApiCallNotPossible($"The number of calls today ({curStatus.LastCallTime.Date.ToString()}) " + 
                                            $"have reached the limit. Counted {curStatus.CallCounter} compared to " + 
                                            $"{AlphaVantageConnection.CallLimitDaily} possible!");
            }
            // Update the LatestStatus property with the status info which has been verified
            this.LatestStatus = curStatus;
        }
    
        /*private async Task<bool> writeInfoFile() 
        {
            InfoCache info = new InfoCache{Today = DateTime.Today, CallCounter = this.CallCounter};
            return await writeInfoFile(info);
        }

        private async Task<bool> writeInfoFile(InfoCache info)
        {
            // Create or overwrite existing file:
            FileStream infoFileStream = File.Create(_apiKeyInfoPath);
            // Serialize the infocache object and 
            try 
            {
                await JsonSerializer.SerializeAsync<InfoCache>(infoFileStream, info);
            } 
            catch 
            {
                infoFileStream.Dispose();
                return false;
            }

            infoFileStream.Dispose();
            return true;
        }
        */
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

