using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Tempo
{
    /// <summary>
    /// This class has methods for getting and processing wather data
    /// </summary>
    public class WeatherData
    {
        private string city = "Koszalin";
        private string API_KEY = "";
        private string query;

        /// <summary>
        /// Constructor
        /// </summary>
        public WeatherData()
        {
            query = "http://api.openweathermap.org/data/2.5/weather?q=" 
                + city + "&mode=xml&units=metric&APPID=" + API_KEY;
        }

        /// <summary>
        /// Gets weather data from url in xml format
        /// </summary>
        public XmlDocument GetXmlData()
        {
            using(WebClient client = new WebClient())
            {
                string xmlData = client.DownloadString(query);

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlData);
                return xmlDocument;
            }
        }

        /// <summary>
        /// Extracts temperature from xml
        /// </summary>
        /// <returns></returns>
        public string GetTemp()
        {
            var document = GetXmlData();
            XmlNode temperatureNode = document.SelectSingleNode("//temperature");
            XmlAttribute temperatureValue = temperatureNode.Attributes["value"];
            string temperature = temperatureValue.Value;
            return temperature.ToString();
        }
    }
}
