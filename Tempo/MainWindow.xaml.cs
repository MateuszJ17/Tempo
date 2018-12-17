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
        double insideTemp;
        double outsideTemp;
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
        }

        private async void WeatherBtn_Click(object sender, RoutedEventArgs e)
        {
            await UpdateTimerAndTemp();
        }

        private async Task UpdateTimerAndTemp()
        {
            WeatherBtn.IsEnabled = false;
            string time = "";
            var temperatures = temp.ParsedTemperatures;
            double initTemp = 21;
            insideTemp = initTemp;

            while (time != "23:59")
            {
                outsideTemp = temperatures[timer.Hour];
                OutsideTemperatureLabelValue.Content = outsideTemp + "°C";

                if(insideTemp>15) insideTemp = Math.Round(insideTemp - (outsideTemp / 10), 2);
                InsideTemperatureLabelValue.Content = insideTemp + "°C";

                time = timer.ToString("HH:mm");
                TimeLabelValue.Content = time;

                var deltaTemp = targetTemp - insideTemp;

                power = fuzzyLogic.GetPower(Math.Round(deltaTemp * 2, MidpointRounding.AwayFromZero) / 2);
                PowerValue.Content = Math.Round(power).ToString() + "%";
                HeatingPowerBar.Value = Math.Round(power);

                await Task.Delay(speed);
                timer = timer.AddMinutes(1);   
            }
            MessageBox.Show("Symulacja została zakończona", "Koniec symulacji");
            WeatherBtn.IsEnabled = true;
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
