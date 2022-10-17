// The xml reader was created with the following resources:
// https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/read-xml-data-from-url
// https://www.codementor.io/@dewetvanthomas/tutorial-currency-converter-application-for-c-121yicb1es

using System.Xml;

namespace EcbCurrencyInterface;
public class EcbXmlReader
{
    private readonly string rssUri;

    private XmlTextReader? reader;
    public EcbXmlReader(string rssUri)
    {
        this.rssUri = rssUri;
    }

    public static async Task<EcbXmlReader> BuildEcbXmlReaderAsync(string rssUri)
    {
        EcbXmlReader outObject = new EcbXmlReader(rssUri);
        await Task.Run(() => outObject.readXml());
        return outObject;
    }

    public void readXml()
    {
        this.reader = new XmlTextReader(this.rssUri);
    } 

    public string[]? GetNodeValues(string[] navigationNodes, string[] targetNodes)
    {
        if (reader is null) 
        {
            // Make sure that the object has a reader
            throw new NullReferenceException("No XmlTextReader referenced. Please read the xml file with readXml before executing htis action!");
        }

        string[] results = new string[targetNodes.Count()];
        bool navigationFound = false;

        // Iterate over all navigation nodes to reach the section where the target nodes are
        foreach (string navNode in navigationNodes) 
        {
            while(reader.Read() && !navigationFound)
            {
                if (string.Compare(reader.Name, navNode) == 0)
                {
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
                if (reader.NodeType == XmlNodeType.Element && string.Compare(node, reader.Name) == 0)
                {
                    reader.Read();
                    results[i] = reader.Value;
                    i++;
                    break;
                }
            }
        }
        return results;
    }
}
