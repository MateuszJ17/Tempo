using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tempo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Temperature temp = new Temperature();
        WeatherData weatherData = new WeatherData();
        DispatcherTimer dt = new DispatcherTimer();
        int counter;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WeatherBtn_Click(object sender, RoutedEventArgs e)
        {
            //var lblText = weatherData.GetTemp();
            //TemperatureLbl.Content = lblText;
            temp.ReadText();
            temp.ParseTemperature();
            //foreach (var item in temp.Temperatures)
            //{
            //    Console.WriteLine(item);
            //}

            counter = 0;
            dt.Interval = TimeSpan.FromMilliseconds(1000);
            dt.Tick += Dt_Tick;
            dt.Start();
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            if(counter<temp.Temperatures.Count)
            {
                //TemperatureLbl.Content = "Temperature outside " + (counter+1) + ": " + temp.Temperatures[counter] + " ºC";
                counter++;
            }
            else
            {
                dt.Stop();
            }
        }
    }
}
