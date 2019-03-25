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

namespace CPDJ_VirtualLock
{
    /// <summary>
    /// Interaction logic for configuration_form.xaml
    /// </summary>
    public partial class configuration_form : Window
    {
        private Configuration _configuration = null;

        public configuration_form()
        {
            throw new NotImplementedException();
        }
        public configuration_form(ref Configuration config_arg)
        {
            _configuration = config_arg;
            this.DataContext = _configuration;
            InitializeComponent();
        }

        private void textBox_PickImageFile(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    (sender as TextBox).Text = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    (sender as TextBox).Text = null;
                    break;
            }
        }
        private void button_PickImageFile(object sender, RoutedEventArgs e)
        {

        }

        private void validate(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
