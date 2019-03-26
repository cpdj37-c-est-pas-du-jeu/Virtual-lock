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
    namespace ValidationCriteria
    {
        public class NumericalRangeRule : ValidationRule
        {
            private int _min;
            private int _max;

            public NumericalRangeRule()
            {
            }

            public int Min
            {
                get { return _min; }
                set { _min = value; }
            }

            public int Max
            {
                get { return _max; }
                set { _max = value; }
            }

            public override ValidationResult Validate(object obj_value, CultureInfo cultureInfo)
            {
                int value = 0;

                try
                {
                    if (((string)obj_value).Length > 0)
                        value = Int32.Parse((String)obj_value);
                }
                catch (Exception e)
                {
                    return new ValidationResult(false, "Illegal characters or " + e.Message);
                }
                
                if (value < Min || value > Max)
                {
                    return new ValidationResult(false,
                      "range: " + Min + " - " + Max + ".");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }
        }
        public class TimeFragmentRule : NumericalRangeRule
        { }

    }

    /// <summary>
    /// Interaction logic for configuration_form.xaml
    /// </summary>
    public partial class ConfigurationForm : Window
    {
        private Configuration _configuration = null;

        public ConfigurationForm()
        {
            throw new NotImplementedException();
        }
        public ConfigurationForm(ref Configuration config_arg)
        {
            _configuration = config_arg;
            this.DataContext = _configuration;
            InitializeComponent();
        }

        static private String GetFilePath()
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    return file;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    return "";
            }
        }

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


        private void validate(object sender, RoutedEventArgs e)
        {
            // if not valide, warning then cancel
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
    }
}
