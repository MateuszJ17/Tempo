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
        FuzzyLogic fuzzyLogic = new FuzzyLogic();

        DateTime timer = new DateTime(2018, 1, 1, 0, 0, 0);

        int speed = 1000;
        int targetTemp;
        int counter = 1;
        double power;

        public MainWindow()
        {
            InitializeComponent();
            temp.ReadText();
            temp.ParseTemperature();
            TargetTemperatureValue.Content = TemperatureSlider.Value + "°C";
            TemperatureSlider.ValueChanged += TemperatureSlider_ValueChanged;
            targetTemp = int.Parse(TemperatureSlider.Value.ToString());
            SpeedSlider.Value = 1000;
            SpeedSlider.ValueChanged += SpeedSlider_ValueChanged;
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            speed = int.Parse(Math.Round(SpeedSlider.Value).ToString());
        }

        private void TemperatureSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TargetTemperatureValue.Content = TemperatureSlider.Value + "°C";
            targetTemp = int.Parse(TemperatureSlider.Value.ToString());
            power = fuzzyLogic.GetPower(targetTemp);
            PowerValue.Content = Math.Round(power).ToString() + "%";
            HeatingPowerBar.Value = Math.Round(power);
        }

        private async void WeatherBtn_Click(object sender, RoutedEventArgs e)
        {
            await UpdateTimerAndTemp();
            //InsideTemperatureLabelValue.Dispatcher.Invoke(new Action(() => 
            //{
            //    dt.Interval = TimeSpan.FromSeconds(1);
            //    dt.Tick += Dt_Tick;
            //    dt.Start();
            //})
            //);
        }

        //private void Dt_Tick(object sender, EventArgs e)
        //{
        //    //foreach (var item in temp.ParsedTemperatures)
        //    //{
        //    //    OutsideTemperatureLabelValue.Content = item.ToString();
        //    //}
        //    TimeLabelValue.Content = DateTime.Now.ToLongTimeString();
        //}


        private async Task UpdateTimerAndTemp()
        {
            string time = "";
            var temperatures = temp.ParsedTemperatures;

            while (time != "23:59")
            {
                OutsideTemperatureLabelValue.Content = temperatures[timer.Hour] + "°C";
                time = timer.ToString("HH:mm");
                TimeLabelValue.Content = time;
                await Task.Delay(speed);
                timer = timer.AddMinutes(1);   
            }
            MessageBox.Show("Symulacja została zakończona", "Koniec symulacji");
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
