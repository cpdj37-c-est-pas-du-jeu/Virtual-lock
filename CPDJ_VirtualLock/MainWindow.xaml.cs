using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;

namespace CPDJ_VirtualLock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        #region Countdown
        private DispatcherTimer timer;
        private TimeSpan remaining_time = TimeSpan.Zero;

        private void StartTimer(TimeSpan time)
        {
            remaining_time = time;
            timer = new DispatcherTimer
            (
                TimeSpan.FromSeconds(1),
                DispatcherPriority.Normal,
                delegate
                {
                    ui_countdown.Text = remaining_time.ToString("c");
                    if (remaining_time == TimeSpan.Zero)
                    {
                        timer.Stop();
                        OnPlayerDefeat();
                    }
                    remaining_time = remaining_time.Subtract(TimeSpan.FromSeconds(1));
                    OnPropertyChanged("remaining_time");

                }, Application.Current.Dispatcher
            );
        }
        #endregion

        #region GamePlay
        private void OnPlayerInput(String input_value)
        {
            if (input_value == "toto") // todo : as configuration
                OnPlayerSuccess();
            else
                OnPlayerBadInput();
        }
        public String remaining_try
        {
            get { return _remaining_try.ToString(); }
        }
        private int _remaining_try = 3;
        private void OnPlayerBadInput()
        {
            // add to try list

            _remaining_try -= 1;
            OnPropertyChanged("remaining_try");
            if (_remaining_try == 0)
            {
                // freeze
                _remaining_try = 3;
            }
        }
        private void OnPlayerDefeat()
        {
            timer.Stop(); // useless
            ui_grid_countdown.Visibility = Visibility.Collapsed;
            ui_grid_failure.Visibility = Visibility.Visible;
        }
        private void OnPlayerSuccess()
        {
            timer.Stop();
            ui_grid_countdown.Visibility = Visibility.Collapsed;
            ui_grid_success.Visibility = Visibility.Visible;
        }
        #endregion

        private void ui_start_button_clicked(object sender, RoutedEventArgs e)
        {
            var start_button = sender as Button;
            if (start_button == null)
                throw new ArgumentException("ui_start_button_clicked:sender");

            start_button.Visibility = Visibility.Hidden;
            ui_dock_password.Visibility = Visibility.Visible;
            ui_passwordBox.Focus();

            StartTimer(TimeSpan.FromSeconds(10));
        }
        private void ui_passwordBox_KeyUp(object sender, KeyEventArgs e)
        {
            var password_box = sender as PasswordBox;

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                e.Handled = true;
                String value = password_box.Password;
                password_box.Clear();
                OnPlayerInput(value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
