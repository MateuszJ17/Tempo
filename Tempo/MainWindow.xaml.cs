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
        int counter = 1;

        public MainWindow()
        {
            InitializeComponent();
            temp.ReadText();
            temp.ParseTemperature();
        }

        private void WeatherBtn_Click(object sender, RoutedEventArgs e)
        {
            InsideTemperatureLabelValue.Dispatcher.Invoke(new Action(() => 
            {
                dt.Interval = TimeSpan.FromSeconds(1);
                dt.Tick += Dt_Tick;
                dt.Start();
            })
            );
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            //foreach (var item in temp.ParsedTemperatures)
            //{
            //    OutsideTemperatureLabelValue.Content = item.ToString();
            //}
            InsideTemperatureLabelValue.Content = DateTime.Now.ToLongTimeString();
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new OptionsWindow();
            optionsWindow.ShowDialog();
            if (!optionsWindow.IsVisible)
            {
                optionsWindow.Show();
                optionsWindow.Activate();
                optionsWindow.Focus();
            }
            else
                optionsWindow.Activate();
        }
    }
}
