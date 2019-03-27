using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CPDJ_VirtualLock
{
    /// <summary>
    /// Interaction logic for configuration_form.xaml
    /// </summary>
    public partial class ConfigurationForm : Window, INotifyPropertyChanged
    {
        private Configuration _configuration = null; // todo : default configuration

        public ConfigurationForm()
        {
            throw new NotImplementedException();
        }
        public ConfigurationForm(ref Configuration config_arg)
        {
            _configuration = config_arg;
            this.DataContext = _configuration;
            InitializeComponent();

            ui_is_lock_is_final.DataContext = this; // for IsLockFinalWrapper
        }

        public bool IsLockFinalWrapper
        {   // IsChecked ? enable/disable ui_TextBoxDurationTime
            set
            {
                if (value == false)
                {
                    ui_TextBoxDurationTime.IsEnabled = true;
                }
                else
                {
                    _configuration.LockDuration = TimeSpan.FromSeconds(1);
                    ui_TextBoxDurationTime.IsEnabled = false;
                }

                _configuration.IsLockFinal = value;
                OnPropertyChanged();
            }
            get
            {
                return _configuration.IsLockFinal;
            }
        }

        static private String GetFilePath()
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    if (!fileDialog.CheckPathExists)
                        return "";
                    var file = fileDialog.FileName;
                    return file;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    return "";
            }
        }

        #region select image
        private void textBox_PickImageFile(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = GetFilePath();
        }
        private void button_PickImageFile(object sender, RoutedEventArgs e)
        {
            var property_name = (sender as Button).Tag.ToString();
            this._configuration.GetType().GetProperty(property_name).SetValue(this._configuration, GetFilePath());
        }
        private void image_MouseDown_selectSourceFile(object sender, RoutedEventArgs e)
        {
            var image_path = GetFilePath();
            if (image_path == "")
                return;

            BitmapImage image_source = new BitmapImage();
            image_source.BeginInit();
            image_source.UriSource = new Uri(image_path);
            image_source.EndInit();

            (sender as Image).Source = image_source;
        }
        #endregion

        private void validate(object sender, RoutedEventArgs e)
        {
            if (!_configuration.IsValid)
            {
                MessageBox.Show("Certaines valeurs sont invalides", "CPDJ : Virtual-lock : Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.Close();
        }

        #region Text validation rules
        // author : easier than proper Rule-s
        private void TextBox_TimeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text_box = sender as TextBox;
            var time_span = new TimeSpan();
            bool is_convertible = TimeSpan.TryParseExact(text_box.Text, "hh\\:mm\\:ss", CultureInfo.CurrentCulture, out time_span);

            if (!is_convertible)
            {
                text_box.Background = Brushes.LightPink;
            }
            else
                text_box.Background = Brushes.LightGreen;
        }
        private void TextBox_PathValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text_box = sender as TextBox;

            text_box.Text = text_box.Text.Replace("file:///", "");

            if (text_box.Text == "" || !File.Exists(text_box.Text))
            {
                text_box.Background = Brushes.LightPink;
            }
            else
                text_box.Background = Brushes.LightGreen;
        }
        private static readonly Regex _isNumericalRegex = new Regex("[^0-9.-]+");
        private void TextBox_NumericalValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text_box = sender as TextBox;

            if (text_box.Text == "" || _isNumericalRegex.IsMatch(text_box.Text))
            {
                text_box.Background = Brushes.LightPink;
            }
            else
            {
                text_box.Background = Brushes.LightGreen;
            }

        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
