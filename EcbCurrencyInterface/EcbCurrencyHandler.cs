// Webapplikasjoner oblig 1     OsloMet     28.10.2022

// This file contains code used to handle the http requests sent to the ecb currency rss feed

// The currency calculator was created using the following resources:
// https://www.codementor.io/@dewetvanthomas/tutorial-currency-converter-application-for-c-121yicb1es


using System.Text.RegularExpressions;
using System.Globalization;

namespace EcbCurrencyInterface;

/***
 * This class contains the code used to handle the http connections with the ecb rss feed
 */
public class EcbCurrencyHandler
{
    // The rss base uri used for the http requests
    private static string rssUri = "https://www.ecb.europa.eu/rss/fxref-";
    
    /***
     * This method provides the f
     * */
    public async static Task<decimal> getExchangeRateAsync(string baseCurr, string targetCurr)
    {
        // n base = exhangeRate * n target

        // Verify that the currencies are specified in the right format:
        Regex currencyPattern = new Regex("[a-zA-Z]{3}");
        if (!currencyPattern.IsMatch(baseCurr))
        {
            throw new ArgumentException("The baseCurr argument has not the right format." + 
                                        " Make sure that it is a 3 character string like USD.");
        }

        if (!currencyPattern.IsMatch(baseCurr))
        {
            throw new ArgumentException("The targetcurr argument has not the right format." + 
                                        " Make sure that it is a 3 character string like USD.");
        }

        if (string.Compare(targetCurr, baseCurr) == 0)
        {
            throw new ArgumentException("The provided base and target currents are equal.");
        }

        decimal exchangeRateEuroBase;
        if (string.Compare(targetCurr.ToLower(), "eur") == 0) 
        {
            // Target is euro
            exchangeRateEuroBase = await getExchangeRateEuroBase(baseCurr);
            return 1/exchangeRateEuroBase;
        }
        else 
        {
            // The euro is not target
            exchangeRateEuroBase = await getExchangeRateEuroBase(targetCurr);
            if (!(string.Compare(baseCurr.ToLower(), "eur") == 0)) 
            {
                // We need to get the exchange between euro and the base
                // n euro = exchangeRateEuroBase * n target
                // n euro = echangeRateBaseEuroBase * n base
                //<=> n base = n target * (exchangeRateEuroBase / echangeRateBaseEuroBase)
                // return value = (exchangeRateEuroBase / echangeRateBaseEuroBase)
                decimal echangeRateBaseEuroBase = await getExchangeRateEuroBase(baseCurr);
                return (exchangeRateEuroBase / echangeRateBaseEuroBase);
            }
            // Eur is base
            return exchangeRateEuroBase;
        }



    }

    private async static Task<decimal> getExchangeRateEuroBase(string targetCurr) 
    {
        EcbXmlReader ecbReader = await EcbXmlReader.BuildEcbXmlReaderAsync(rssUri + targetCurr.ToLower() + ".html");

        string[] navigationNodes = {"item"};
        string[] targetNodes = {"dc:date", "cb:value"};

        string[]? result = ecbReader.GetNodeValues(navigationNodes, targetNodes);

        if (result is null)
        {
            throw new Exception($"Error while parsing the xml file: {rssUri}");
        }
        // Parse the echange rate (cb:value)
        CultureInfo ci = new CultureInfo("en-US");
        decimal exchangeRate = Decimal.Parse(result[1], ci);
        return exchangeRate;
    }
}