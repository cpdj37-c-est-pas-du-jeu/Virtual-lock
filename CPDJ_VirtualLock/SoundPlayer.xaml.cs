using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UserControl = System.Windows.Controls.UserControl;

namespace CPDJ_VirtualLock
{
    /// <summary>
    /// Interaction logic for SoundPlayer.xaml
    /// </summary>
    public partial class SoundPlayer : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChange([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        private DispatcherTimer _timer = null;
        private bool _isUserDraggingSlider = false;

        #region MediaSource
        public static readonly DependencyProperty MediaSourceProperty =
            DependencyProperty.Register("MediaSource", typeof(Uri), typeof(SoundPlayer), new PropertyMetadata(default(Uri)));
        
        public Uri MediaSource
        {
            get { return (Uri)GetValue(MediaSourceProperty); }
            set
            {
                ui_MediaPlayer.Stop();
                SetValue(MediaSourceProperty, value);
                RaisePropertyChange();
            }
        }
        #endregion

        public SoundPlayer()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this; // OMG !

            Initialize_ui_SliderProgress_Thumb_Template();

            _timer = new DispatcherTimer
            (
                TimeSpan.FromMilliseconds(100),
                DispatcherPriority.Normal,
                delegate
                {
                    if (ui_MediaPlayer.Source != null &&
                        ui_MediaPlayer.NaturalDuration.HasTimeSpan &&
                        !_isUserDraggingSlider)
                    {
                        ui_SliderProgress.Minimum = 0;
                        ui_SliderProgress.Maximum = ui_MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        ui_SliderProgress.Value = ui_MediaPlayer.Position.TotalSeconds;
                        ui_LabelProgressStatus.Text = ui_MediaPlayer.Position.ToString(@"hh\:mm\:ss");
                    }
                }, System.Windows.Application.Current.Dispatcher
            );
            _timer.Start();
        }

        static private Uri GetFilePath(String filter = "")
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = filter;

            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    if (!fileDialog.CheckPathExists)
                        return null;
                    return new Uri(fileDialog.FileName);
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    return null;
            }
        }

        #region Toolbar buttons
        private void ui_ButtonOpenSource_Click(object sender, RoutedEventArgs e)
        {
            var media_source = GetFilePath("Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*");
            if (media_source != null)
            {
                MediaSource = media_source;
            }

        }
        private void ui_ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            ui_MediaPlayer.Play();
        }
        private void ui_ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            ui_MediaPlayer.Stop();
        }
        private void ui_ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            ui_MediaPlayer.Pause();
        }
        #endregion

        #region Progression slider
        private void ui_SliderProgress_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _isUserDraggingSlider = true;
        }
        private void ui_SliderProgress_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _isUserDraggingSlider = false;
            ui_MediaPlayer.Position = TimeSpan.FromSeconds((sender as Slider).Value);
        }
        private void ui_SliderProgress_Template_Thumb_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.MouseDevice.Captured == null)
            {
                MouseButtonEventArgs args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                args.RoutedEvent = MouseLeftButtonDownEvent;
                (sender as Thumb).RaiseEvent(args);
            }
        }
        private void Initialize_ui_SliderProgress_Thumb_Template()
        {
            ui_SliderProgress.ApplyTemplate();
            Thumb thumb = (ui_SliderProgress.Template.FindName("PART_Track", ui_SliderProgress) as Track).Thumb;
            thumb.MouseEnter += new System.Windows.Input.MouseEventHandler(ui_SliderProgress_Template_Thumb_MouseEnter);
        }
        #endregion
    }
}
