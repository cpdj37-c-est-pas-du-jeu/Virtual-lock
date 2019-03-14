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
        private DispatcherTimer timer;
        private TimeSpan remaining_time = TimeSpan.Zero;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

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

        private void OnPlayerDefeat()
        {
            ui_grid_countdown.Visibility = Visibility.Collapsed;
            ui_grid_failure.Visibility = Visibility.Visible;
        }

        private void ui_start_button_clicked(object sender, RoutedEventArgs e)
        {
            var start_button = sender as Button;
            if (start_button == null)
                throw new ArgumentException("ui_start_button_clicked:sender");
            start_button.Visibility = Visibility.Hidden;

            StartTimer(TimeSpan.FromSeconds(10));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
