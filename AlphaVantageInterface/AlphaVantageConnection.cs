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
        private readonly bool _isLimited;

        private static readonly string _apiKeyInfoPath = "InfoCache.json"; 

        private readonly string _baseUri = "https://www.alphavantage.co/query?";
        private int CallCounter = 0;
        private static int CallLimitDaily = 10;
        private DateTime DateOfLatstApiCall;

        private readonly HttpClient cli = new HttpClient();

        public AlphaVantageConnection(string apiKey, bool isLimited) 
        {
            this._apiKey = apiKey;
            this._isLimited = isLimited;

            updateInfoCache();
        }

        private async Task<InfoCache> readInfoFile() 
        {
            Stream infoFileStream = File.OpenRead(_apiKeyInfoPath);
            InfoCache? info = await JsonSerializer.DeserializeAsync<InfoCache>(infoFileStream);
            infoFileStream.Dispose();

            if (info is null) {
                // Write to file and return a custom info object
                InfoCache custom = new InfoCache {
                    Today = DateTime.Today,
                    CallCounter = this.CallCounter
                };

                // Write the custom infoCache to file
                await writeInfoFile(custom);

                return custom;
            }

            return info;
        }

        private async Task<bool> writeInfoFile() 
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

        public ConnectionStatus getConnectionStatus() {
            // Create the summary object
            ConnectionStatus Out = new ConnectionStatus(){
                apiKey = _apiKey,
                lastCallTime = DateOfLatstApiCall,
                callCounter = this.CallCounter
            };
            return Out;
        }

        // The http rest client used is based on the alpha vantage documentation:
        //          https://www.alphavantage.co/documentation/
        public async Task<SearchResult> findStockAsync(string keyword)
        {
            string function = "SYMBOL_SEARCH";
            string searchUri = $"{_baseUri}function={function}&keywords={keyword}&apikey={_apiKey}";

            // Increment counter and update infoCache

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

        private async Task<bool> addCallCounter()
        {
            return await updateInfoCache(1);
        }

        private async Task<bool> updateInfoCache(int addCounter = 0) {
            FileStream curInfoStream = File.Open(_apiKeyInfoPath, System.IO.FileMode.OpenOrCreate);
            InfoCache? curInfo = await JsonSerializer.DeserializeAsync<InfoCache>(curInfoStream);
            
            if (curInfo is null) {
                // If the file is empty, add a new cache object
                curInfo = new InfoCache {
                    Today = DateTime.Today,
                    CallCounter = addCounter
                };
                this.DateOfLatstApiCall = DateTime.Today;
                this.CallCounter = addCounter;
            }     
            else {
                if (DateTime.Today.Date == curInfo.Today.AddDays(1).Date) 
                {
                    // The last call was yesterday. Set call counter to 0
                    this.CallCounter = addCounter;
                    curInfo.CallCounter = addCounter;
                    this.DateOfLatstApiCall = DateTime.Today;
                    curInfo.Today = DateTime.Today;
                } else {                
                    this.CallCounter += addCounter;
                }

                if (curInfo.CallCounter >= AlphaVantageConnection.CallLimitDaily) {
                    // Close file with no writing:
                    curInfoStream.Dispose();
                    throw new Exception($"The number of calls today ({curInfo.Today.ToString()}) have reached the limit");
                } else {
                    curInfo.CallCounter += addCounter;
                }
            }
            // Write to file
            await JsonSerializer.SerializeAsync<InfoCache>(curInfoStream, curInfo);
            curInfoStream.Dispose();

            return true;
        }

        
    }
}

