using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Tempo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Temperature temp = new Temperature();
        WeatherData weatherData = new WeatherData();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WeatherBtn_Click(object sender, RoutedEventArgs e)
        {
            //var lblText = weatherData.GetTemp();
            //TemperatureLbl.Content = lblText;
            temp.ReadText(25);
            temp.ParseTemperature();
            foreach (var item in temp.Temperatures)
            {
                Console.WriteLine(item);
            }
        }
    }
}
