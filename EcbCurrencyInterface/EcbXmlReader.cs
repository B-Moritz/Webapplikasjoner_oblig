// Webapplikasjoner oblig 1     OsloMet     28.10.2022

// This file contains code used to handle the http requests sent to the ecb currency rss feed

// The xml reader was created with the following resources:
// https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/read-xml-data-from-url
// https://www.codementor.io/@dewetvanthomas/tutorial-currency-converter-application-for-c-121yicb1es

using System.Xml;

namespace EcbCurrencyInterface;
public class EcbXmlReader
{
    private readonly string rssUri;
    // The xml reader object used to read the xml response
    private XmlTextReader? reader;
    public EcbXmlReader(string rssUri)
    {

        this.rssUri = rssUri;
    }

    /***
     * This method creates a new EcbXmlReader instance and executes the http request to obtian the XML stream object 
     * Parameters:
     *  (string) rssUri: The uri used for the http request
     */ 
    public static async Task<EcbXmlReader> BuildEcbXmlReaderAsync(string rssUri)
    {
        EcbXmlReader outObject = new EcbXmlReader(rssUri);
        // Get the XML reader object for the specified rssUri
        await Task.Run(() => outObject.GetCurrencyData());
        return outObject;
    }

    /***
     * This method makes the request and creates a new XML reader instance.
     */
    public void GetCurrencyData()
    {
        // Makes the http request and createss the XmlTextReader.
        this.reader = new XmlTextReader(this.rssUri);
    }

    /***
     * The GetNodeValues method iterates over the XML reader of the object and finds the values of the targetNodes.
     * Parametere:
     *  (string[]) navigationNodes: The node names that should be passed 
     *  (string[]) targetNodes: The nodes that should be in the result returned
     *  Return: A list of strings containing the values of the target nodes.
     */
    public string[]? GetNodeValues(string[] navigationNodes, string[] targetNodes)
    {
        if (reader is null) 
        {
            // Make sure that the object has a reader
            throw new NullReferenceException("No XmlTextReader referenced. Please read the xml file with readXml before executing htis action!");
        }
        // Define a list for the target nodes that will be obtained
        string[] results = new string[targetNodes.Count()];
        // Navigation flag is set to false until the navigation point is reached
        bool navigationFound = false;

        // Iterate over all navigation nodes to reach the section where the target nodes are
        foreach (string navNode in navigationNodes) 
        {
            while(reader.Read() && !navigationFound)
            {
                // While there still are blocks in the stream
                if (reader.Name == navNode)
                {
                    // NavigationFound settes til true for å avslutte while løkken.
                    navigationFound = true;
                }
            }
        }

        if (!navigationFound)
        {
            // Return null if the navigation nodes were not found
            return null;
        }

        int i = 0;
        // Iterate over all target nodes and extract their values.
        foreach (string node in targetNodes) 
        {
            while(reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && node == reader.Name)
                {
                    reader.Read();
                    // If the reader value is the target, add it to the result aray.
                    results[i] = reader.Value;
                    i++;
                    break;
                }
            }
        }
        return results;
    }
}
