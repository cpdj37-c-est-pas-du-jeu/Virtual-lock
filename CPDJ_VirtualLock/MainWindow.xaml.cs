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
using System.Windows.Threading;

namespace CPDJ_VirtualLock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan remaining_time;

        public MainWindow()
        {
            InitializeComponent();
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
                        OnTimerStop();
                    }
                    remaining_time = remaining_time.Subtract(TimeSpan.FromSeconds(1));
                }, Application.Current.Dispatcher
            );
        }

        private void OnTimerStop()
        {

        }

        private void ui_start_button_clicked(object sender, RoutedEventArgs e)
        {
            var start_button = sender as Button;
            if (start_button == null)
                throw new ArgumentException("ui_start_button_clicked:sender");
            start_button.Visibility = Visibility.Hidden;

            StartTimer(TimeSpan.FromSeconds(10));
        }
    }
}
