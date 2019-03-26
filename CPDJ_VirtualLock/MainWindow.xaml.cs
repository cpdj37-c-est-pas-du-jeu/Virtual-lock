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
        // todo : state machine :
        // ready -> running -> [input_enabled || input_frozen] -> stopped : [success || defeat]

        public Configuration _configuration { get; set; }

        public MainWindow(Configuration configuration_arg)
        {
            _configuration = configuration_arg;

            InitializeComponent();
            DataContext = this;

            ui_grid_success.DataContext = _configuration;
            ui_grid_failure.DataContext = _configuration;

            RemainingTry = _configuration.TryBeforeLock;
            RemainingTime = _configuration.TotalDuration;

            ui_start_button.Focus();
        }

        #region Countdown
        private DispatcherTimer timer;
        private TimeSpan _remainingTime = TimeSpan.Zero;
        public TimeSpan RemainingTime
        {
            get { return _remainingTime; }
            set
            {
                _remainingTime = value;
                OnPropertyChanged();
            }
        }

        private void StartTimer(TimeSpan time)
        {
            ui_remainingTime_progressBar.Visibility = Visibility.Visible;
            ui_remainingTime_progressBar.Minimum = 0;
            ui_remainingTime_progressBar.Maximum = Convert.ToInt32(time.TotalSeconds); // can throw overflow exception

            RemainingTry = _configuration.TryBeforeLock;
            RemainingTime = time;
            timer = new DispatcherTimer
            (
                TimeSpan.FromSeconds(1),
                DispatcherPriority.Normal,
                delegate
                {
                    ui_remainingTime_progressBar.Value = time.TotalSeconds - RemainingTime.TotalSeconds;
                    //ui_countdown.Text = RemainingTime.ToString(@"hh\:mm\:ss"); // "c"
                    if (RemainingTime == TimeSpan.Zero)
                    {
                        timer.Stop();
                        OnPlayerDefeat();
                    }
                    RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
                }, Application.Current.Dispatcher
            );
        }
        #endregion

        #region GamePlay
        private void OnPlayerInput(String input_value)
        {
            if (input_value == _configuration.Password)
                OnPlayerSuccess();
            else
                OnPlayerBadInput();
        }

        public String RemainingTryAsString
        {
            get { return _remainingTry.ToString(); }
        }
        private uint _remainingTry = 1; // set in StartTimer
        public uint RemainingTry
        {
            get { return _remainingTry; }
            set
            {
                _remainingTry = value;
                OnPropertyChanged();
            }
        }

        private void OnPlayerBadInput()
        {
            // add to try list [?] + _configuration.IsTryListVisible

            RemainingTry -= 1;
            OnPropertyChanged("RemainingTryAsString");
            if (RemainingTry == 0)
            {
                if (_configuration.IsLockFinal)
                {   // defeat
                    OnPlayerDefeat();
                    return;
                }
                FreezeInputs(_configuration.LockDuration);
                RemainingTry = _configuration.TryBeforeLock;
                OnPropertyChanged("RemainingTryAsString");
            }
        }
        private BackgroundWorker freeze_inputs_backgroundWorker;
        private void FreezeInputs(TimeSpan duration)
        {
            if (duration == TimeSpan.Zero)
                return;

            if (duration < TimeSpan.FromSeconds(1))
                throw new ArgumentException("FreezeInputs : invalid duration (<1s)");

            ui_remaining_try.Visibility = Visibility.Hidden;
            ui_freeze_inputs_progressBar.Visibility = Visibility.Visible;
            ui_dock_password.IsEnabled = false;

            freeze_inputs_backgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };

            freeze_inputs_backgroundWorker.DoWork += (sender, eventArg) =>
            {
                int duration_fragments = Convert.ToInt32(duration.TotalMilliseconds / 100); // truncates

                for (var i = 0; i < 100; ++i)
                {
                    System.Threading.Thread.Sleep(duration_fragments);
                    freeze_inputs_backgroundWorker.ReportProgress(i);
                }
            };
            freeze_inputs_backgroundWorker.RunWorkerCompleted += (sender, eventArg) =>
            {
                ui_dock_password.IsEnabled = true;
                ui_freeze_inputs_progressBar.Visibility = Visibility.Hidden;
                ui_remaining_try.Visibility = Visibility.Visible;
                ui_password_field.Focus();
            };
            freeze_inputs_backgroundWorker.ProgressChanged += (sender, eventArg) =>
            {
                ui_freeze_inputs_progressBar.Value = eventArg.ProgressPercentage;
            };

            freeze_inputs_backgroundWorker.RunWorkerAsync();
        }
        private void OnPlayerDefeat()
        {
            if (freeze_inputs_backgroundWorker != null && freeze_inputs_backgroundWorker.WorkerSupportsCancellation)
            {
                freeze_inputs_backgroundWorker.CancelAsync();
            }
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
            ui_password_field.Focus();

            StartTimer(_configuration.TotalDuration);
        }
        #region player input validation
        private void ui_passwordField_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender is TextBox)
                ui_passwordField_TextBox_KeyUp(sender, e);
            else if (sender is PasswordBox)
                ui_passwordField_PasswordBox_KeyUp(sender, e);
            else
                throw new InvalidOperationException("ui_passwordField_KeyUp : unhandled sender type");
        }
        // if field is a Textbox
        private void ui_passwordField_TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            var password_box = sender as TextBox;

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                e.Handled = true;
                String value = password_box.Text;
                if (value.Length == 0)
                    return;
                password_box.Clear();
                OnPlayerInput(value);
            }
        }
        // if field is a PasswordBox
        private void ui_passwordField_PasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            var password_box = sender as PasswordBox;

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                e.Handled = true;
                String value = password_box.Password;
                if (value.Length == 0)
                    return;
                password_box.Clear();
                OnPlayerInput(value);
            }
        }
        private void Ui_passwordField_SizeChanged(object sender, SizeChangedEventArgs e)
        {   // quickfix : shameful trick ... (ViewBox sucks for input fields)
            var input_field = sender as Control;
            input_field.FontSize = input_field.ActualHeight / 2;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
