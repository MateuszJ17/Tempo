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
using System.Windows.Shapes;

namespace Tempo
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public string InputMode { get; set; }
        public int Interval { get; set; }
        public double Loss { get; set; }
        public bool IsActivated { get; set; } = false;

        public OptionsWindow()
        {
            InitializeComponent();
            AutoInput.IsChecked = true;
            CheckMode();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region Input type check
            CheckMode();
            #endregion

            #region Get interval length if manual
            if (InputMode == "Manual")
            {
                try
                {
                    Interval = int.Parse(IntervalLength.Text);
                }
                catch (Exception ex)
                {
                    IntervalLength.Text = "Podaj prawidłową wartość!";
                }
            }
            else
                IntervalLength.IsEnabled = false;
            #endregion


            IsActivated = false;
            this.Hide();
        }

        private void CheckMode()
        {
            if (ManualInput.IsChecked == true)
            {
                InputMode = "Manual";
                AutoInput.IsChecked = false;
            }
            else
            {
                InputMode = "Auto";
                ManualInput.IsChecked = false;
            }
        }
    }
}
