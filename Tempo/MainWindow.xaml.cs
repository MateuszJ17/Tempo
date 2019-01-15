using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        DateTime heatingTimer = new DateTime(2018, 1, 1, 0, 0, 0);
        DateTime powerTimer = new DateTime(2018, 1, 1, 0, 0, 0);

        int speed = 1000;
        int targetTemp;
        double insideTemp;
        double outsideTemp;
        double power;
        //double loss;

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
            double change = 0;
            double loss;
            while (time != "23:59")
            {
                outsideTemp = temperatures[timer.Hour];
                OutsideTemperatureLabelValue.Content = outsideTemp + "°C";
                if (outsideTemp < 0)
                {
                    loss = 1.5;
                }
                if (outsideTemp >= 0 & outsideTemp <= 2)
                {
                    loss = 1.3;
                }
                if (outsideTemp > 2 && outsideTemp <= 3.5)
                {
                    loss = 1.2;
                }
                if (outsideTemp > 3.5 && outsideTemp <= 4.5)
                {
                    loss = 1.15;
                }
                if (outsideTemp > 4.5)
                {
                    loss = 1.1;
                }
                else
                    loss = 1;
                //if(insideTemp>15) insideTemp = Math.Round(insideTemp - (outsideTemp / 10), 2);
                //InsideTemperatureLabelValue.Content = insideTemp + "°C";

                //if (insideTemp > 0)
                //{
                //    insideTemp = Math.Round(((insideTemp - outsideTemp) / 100) * loss, 2);
                //    InsideTemperatureLabelValue.Content = insideTemp + "°C";
                //}
                //else
                //{
                //    insideTemp = Math.Round((insideTemp - outsideTemp) / 100, 2);
                //    InsideTemperatureLabelValue.Content = insideTemp.ToString() + "°C";
                //}
                if (insideTemp < 30 && insideTemp > 15 && !(insideTemp == TemperatureSlider.Value || insideTemp == 30 || insideTemp == 15 || insideTemp > TemperatureSlider.Value))
                {
                    if (power > 60)
                    {
                        change = 50;
                    }
                    if (power > 40 && power < 60)
                    {
                        change = 30;
                    }
                    if (power > 20 && power < 40)
                    {
                        change = 15;
                    }
                    if (power > 0 && power < 20)
                    {
                        change = 5;
                    }
                }
                if (insideTemp == TemperatureSlider.Value || insideTemp == 30 || insideTemp == 15)
                {
                    heatingTimer = heatingTimer.AddMinutes(1);
                    change = 0;
                    power = 0;
                    while (powerTimer.Minute < 25)
                    {
                        power = 0;
                        powerTimer = powerTimer.AddMinutes(1);
                    }
                    powerTimer = new DateTime(2018, 1, 1, 0, 0, 0);

                    if (heatingTimer.Minute >= 10)
                    {
                        change = -15;
                        heatingTimer = new DateTime(2018, 1, 1, 0, 0, 0);
                    }
                }
                if (insideTemp > TemperatureSlider.Value)
                {
                    heatingTimer = heatingTimer.AddMinutes(1);
                    change = 0;
                    power = 0;
                    while (powerTimer.Minute < 25)
                    {
                        power = 0;
                        powerTimer = powerTimer.AddMinutes(1);
                    }
                    powerTimer = new DateTime(2018, 1, 1, 0, 0, 0);

                    if (heatingTimer.Minute >= 10)
                    {
                        change = -15;
                        heatingTimer = new DateTime(2018, 1, 1, 0, 0, 0);
                    }
                }
                insideTemp = Math.Round(insideTemp + double.Parse((change * loss / 100).ToString()), 2);
                InsideTemperatureLabelValue.Content = insideTemp.ToString();

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
            optionsWindow.IsActivated = true;

            optionsWindow.ShowDialog();

            if (!optionsWindow.IsVisible && optionsWindow.IsActivated == true)
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
