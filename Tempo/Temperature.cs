using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tempo
{
    class Temperature
    {
        private readonly string path = @"..\..\Data\Temperature.txt";
        private List<string> temperatures = new List<string>();
        private List<double> parsedTemperatures = new List<double>();

        public List<string> Temperatures { get => temperatures; set => temperatures = value; }
        public List<double> ParsedTemperatures { get => parsedTemperatures; set => parsedTemperatures = value; }
        public double tempLoss;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Temperature()
        {
            Temperatures = temperatures;
            ParsedTemperatures = parsedTemperatures;
        }

        /// <summary>
        /// Reads data from text file
        /// </summary>
        public void ReadText()
        {
            var lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Count(); i++)
            {
                if (i == 0)
                {
                    temperatures.Add(lines[i]);
                    continue;
                }
                temperatures.Add(lines[i]);
            }
        }

        /// <summary>
        /// Parsing data
        /// </summary>
        public void ParseTemperature()
        {
            double singleTemp = 0.0;

            foreach (var item in temperatures)
            {
                string replacedItem = item.Replace('.', ',');
                singleTemp = double.Parse(replacedItem);
                parsedTemperatures.Add(singleTemp);
            }
        }

        public void GetTempLoss(double tempOutside, double tempInside)
        {
            tempLoss = tempOutside / 100;
        }

        public double CalculateHeatLoss(double tempInside)
        {
            return tempInside - tempLoss;
        }

    }
}
