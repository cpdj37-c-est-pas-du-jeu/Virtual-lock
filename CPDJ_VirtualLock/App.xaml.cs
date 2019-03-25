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
                {
                    var configuration = new Configuration();
                    try
                    {
                        Serializer.DeSerialize(Configuration.file_path, out configuration);
                    }
                    catch (Exception) { }

                    var config_form = new configuration_form(ref configuration);
                    config_form.ShowDialog();

                    try
                    {
                        Serializer.Serialize(Configuration.file_path, configuration);
                    }
                    catch (Exception) { }
                }

                var main_window = new MainWindow();
                main_window.ShowDialog();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            this.Shutdown();
        }
    }
}
