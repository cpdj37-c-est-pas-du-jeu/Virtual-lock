using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CPDJ_VirtualLock
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void VL_App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                var configuration = new Configuration();
                {
                    try
                    {
                        Serializer.DeSerialize(Configuration.file_path, out configuration);
                    }
                    catch (Exception) { }

                    var config_form = new ConfigurationForm(ref configuration);
                    config_form.ShowDialog();

                    try
                    {
                        Serializer.Serialize(Configuration.file_path, configuration);
                    }
                    catch (Exception) { }
                }

                if (!configuration.IsValid)
                    return;

                var main_window = new MainWindow(configuration);
                main_window.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Please report this issue at :\r\n"
                    + "https://github.com/cpdj37-c-est-pas-du-jeu/Virtual-lock/issues" + "\r\n\r\n" + ex.Message,
                    "CPDJ Virtual-lock : Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine(ex.Message);
            }

            this.Shutdown();
        }
    }
}
