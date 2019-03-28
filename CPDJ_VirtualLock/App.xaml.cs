using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
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
        static private Uri CombineUri(string base_URI, string relative_or_absolute_URI)
        {
            return new Uri(new Uri(base_URI), relative_or_absolute_URI);
        }

        private Configuration LoadConfiguration()
        {
            var current_path = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (current_path.Last() != '/' && current_path.Last() != '\\')
                current_path += '/';

            if (!Directory.Exists(current_path))
                throw new Exception(current_path + "\n\nBad execution directory. Make sure that all provided ressources are correct. If not, reinstall.");

            var sounds_ressource_path = CombineUri(current_path, "ressources/sounds").AbsolutePath;
            if (sounds_ressource_path.Last() != '/' && sounds_ressource_path.Last() != '\\')
                sounds_ressource_path += '/';
            sounds_ressource_path = Uri.UnescapeDataString(sounds_ressource_path);

            if (!Directory.Exists(current_path + "ressources/sounds"))
                throw new Exception(sounds_ressource_path + "\n\nBad sounds ressources directory. Make sure that all provided ressources are correct. If not, reinstall.");

            var configuration = new Configuration
            {   // default values
                IsLockFinal = false,
                TotalDuration = TimeSpan.FromMinutes(1),
                LockDuration = TimeSpan.FromSeconds(3),
                Password = "toto",
                TryBeforeLock = 3,
                #region images
                PlayerDefeatImagePath = new Uri("pack://application:,,,/ressources/images/busted.png"),
                PlayerSuccessImagePath = new Uri("pack://application:,,,/ressources/images/top_secret.png"),
                #endregion
                #region audio
                // sounds cannot be embed/packed
                AmbianceMusicSoundPath = CombineUri(sounds_ressource_path, "lesser_vibes_Drone_Low_Resonance_Cave_Underground_Tunnel_Uneasy_046.mp3"),
                IntervalSound = TimeSpan.FromSeconds(15),
                IntervalSoundPath = CombineUri(sounds_ressource_path, "noisecreations_SFX-NCFREE02_Bell-Church-Large.mp3"),
                PlayerBadInputSoundPath = CombineUri(sounds_ressource_path, "science_fiction_computer_glitch_or_malfunction_004.mp3"),
                PlayerDefeatSoundPath = CombineUri(sounds_ressource_path, "zapsplat_explosion_large_boom_slight_distance_25207.mp3"),
                PlayerSuccessSoundPath = CombineUri(sounds_ressource_path, "app_alert_tone_ringtone_002.mp3")
                #endregion
            };

            try
            {
                Serializer.DeSerialize(Configuration.file_path, out configuration);
            }
            catch (Exception) { }

            var config_form = new ConfigurationForm(ref configuration);
            config_form.ShowDialog();

            if (!configuration.IsValid)
                throw new Exception("CPDJ_VirtualLock.VL_App_Startup : Invalid configuration");

            try
            {
                Serializer.Serialize(Configuration.file_path, configuration);
            }
            catch (Exception) { }

            return configuration;
        }

        private void VL_App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Configuration configuration = LoadConfiguration();

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
