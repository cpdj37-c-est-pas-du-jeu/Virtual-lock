﻿using System;
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
        private Configuration LoadConfiguration()
        {
            var configuration = new Configuration
            {   // default values
                IsLockFinal = false,
                TotalDuration = TimeSpan.FromMinutes(1),
                LockDuration = TimeSpan.FromSeconds(3),
                Password = "toto",
                TryBeforeLock = 3,
                #region images
                PlayerDefeatImagePath = new Uri("pack://application:,,,/ressources/images/busted.png").AbsolutePath,
                PlayerSuccessImagePath = new Uri("pack://application:,,,/ressources/images/top_secret.png").AbsolutePath,
                #endregion
                #region audio
                AmbianceMusicSoundPath = new Uri("pack://application:,,,/ressources/sounds/lesser_vibes_Drone_Low_Resonance_Cave_Underground_Tunnel_Uneasy_046.mp3").AbsolutePath,
                IntervalSound = TimeSpan.FromSeconds(15),
                IntervalSoundPath = new Uri("pack://application:,,,/ressources/sounds/noisecreations_SFX-NCFREE02_Bell-Church-Large.mp3").AbsolutePath,
                PlayerBadInputSoundPath = new Uri("pack://application:,,,/ressources/sounds/science_fiction_computer_glitch_or_malfunction_004.mp3").AbsolutePath,
                PlayerDefeatSoundPath = new Uri("pack://application:,,,/ressources/sounds/zapsplat_explosion_large_boom_slight_distance_25207.mp3").AbsolutePath,
                PlayerSuccessSoundPath = new Uri("pack://application:,,,/ressources/sounds/app_alert_tone_ringtone_002.mp3").AbsolutePath
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
