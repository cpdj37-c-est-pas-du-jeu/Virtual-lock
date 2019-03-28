using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPDJ_VirtualLock
{
    public class Serializer
    {
        static public void Serialize<T>(string file_path, T value)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(file_path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, value);
            stream.Close();
        }
        static public void DeSerialize<T>(string file_path, out T value)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(file_path, FileMode.Open, FileAccess.Read, FileShare.Read);
            value = (T)formatter.Deserialize(stream);
            stream.Close();
        }
    }

    [Serializable]
    public class Configuration : INotifyPropertyChanged
        // : GCL.MetaConfiguration
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChange([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public Configuration()
        {
            // base.Load();
        }

        #region total duration
        [XmlIgnore]
        private TimeSpan _totalDuration = TimeSpan.Zero;
        [XmlIgnore]
        public TimeSpan TotalDuration
        {
            get { return _totalDuration; }
            set
            {
                _totalDuration = value;
                RaisePropertyChange();
            }
        }
        [XmlElement("TotalDuration")]
        public long TotalDurationTicks
        {
            get { return TotalDuration.Ticks; }
            set { TotalDuration = new TimeSpan(value); }
        }
        #endregion

        #region password
        [XmlIgnore]
        private String _password = "toto";
        public String Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChange();
            }
        }
        #endregion

        #region player attempts
        [XmlIgnore]
        private uint _tryBeforeLock = 1;
        public uint TryBeforeLock
        {
            get { return _tryBeforeLock; }
            set
            {
                _tryBeforeLock = value;
                RaisePropertyChange();
            }
        }

        [XmlIgnore]
        private TimeSpan _lockDuration = TimeSpan.Zero;
        [XmlIgnore]
        public TimeSpan LockDuration
        {
            get { return _lockDuration; }
            set
            {
                _lockDuration = value;
                RaisePropertyChange();
            }
        }
        [XmlElement("LockDuration")]
        public long LockDurationTicks
        {
            get { return LockDuration.Ticks; }
            set { LockDuration = new TimeSpan(value); }
        }
        [XmlIgnore]
        private bool _is_lock_final = false;
        public bool IsLockFinal
        {
            get
            {
                return _is_lock_final;
            }
            set
            {
                _is_lock_final = value;
                RaisePropertyChange();
            }
        }
        #endregion

        #region game_over
        [XmlIgnore]
        private readonly Uri defaultMissingImagePath = new Uri("pack://application:,,,/ressources/images/icons/missing_image.png");

        [XmlIgnore]
        private Uri _playerDefeatImagePath = null;
        [XmlIgnore]
        public Uri PlayerDefeatImagePath
        {
            get
            {
                if (_playerDefeatImagePath == null)
                    return defaultMissingImagePath;
                return _playerDefeatImagePath;
            }
            set
            {
                _playerDefeatImagePath = value;
                RaisePropertyChange();
            }
        }
        [XmlElement("PlayerDefeatImagePath")]
        public string PlayerDefeatImagePathAsString
        {
            get { return PlayerDefeatImagePath.ToString(); }
            set { PlayerDefeatImagePath = (value == null ? null : new Uri(value.ToString())); }
        }

        [XmlIgnore]
        private Uri _playerSuccessImagePath = null;
        [XmlIgnore]
        public Uri PlayerSuccessImagePath
        {
            get
            {
                if (_playerSuccessImagePath == null)
                    return defaultMissingImagePath;
                return _playerSuccessImagePath;
            }
            set
            {
                _playerSuccessImagePath = value;
                RaisePropertyChange();
            }
        }
        [XmlElement("PlayerSuccessImagePath")]
        public string PlayerSuccessImagePathAsString
        {
            get { return PlayerSuccessImagePath.ToString(); }
            set { PlayerSuccessImagePath = (value == null ? null : new Uri(value.ToString())); }
        }
        #endregion

        #region audio
        #region AmbianceMusicSoundPath
        [XmlIgnore]
        private Uri _ambianceMusicSoundPath = null;
        [XmlIgnore]
        public Uri AmbianceMusicSoundPath
        {
            get { return _ambianceMusicSoundPath; }
            set
            {
                _ambianceMusicSoundPath = value;
                RaisePropertyChange();
            }
        }
        [XmlElement("AmbianceMusicSoundPath")]
        public string AmbianceMusicSoundPathAsString
        {
            get { return AmbianceMusicSoundPath.ToString(); }
            set { AmbianceMusicSoundPath = (value == null ? null : new Uri(value.ToString())); }
        }
        #endregion

        #region PlayerDefeatSoundPath
        [XmlIgnore]
        private Uri _playerDefeatSoundPath = null;
        [XmlIgnore]
        public Uri PlayerDefeatSoundPath
        {
            get { return _playerDefeatSoundPath; }
            set
            {
                _playerDefeatSoundPath = value;
                RaisePropertyChange();
            }
        }
        [XmlElement("PlayerDefeatSoundPath")]
        public string PlayerDefeatSoundPathAsString
        {
            get { return PlayerDefeatSoundPath.ToString(); }
            set { PlayerDefeatSoundPath = (value == null ? null : new Uri(value.ToString())); }
        }
        #endregion

        #region PlayerSuccessSoundPath
        [XmlIgnore]
        private Uri _playerSuccessSoundPath = null;
        [XmlIgnore]
        public Uri PlayerSuccessSoundPath
        {
            get { return _playerSuccessSoundPath; }
            set
            {
                _playerSuccessSoundPath = value;
                RaisePropertyChange();
            }
        }
        [XmlElement("PlayerSuccessSoundPath")]
        public string PlayerSuccessSoundPathAsString
        {
            get { return PlayerSuccessSoundPath.ToString(); }
            set { PlayerSuccessSoundPath = (value == null ? null : new Uri(value.ToString())); }
        }
        #endregion

        #region PlayerBadInputSoundPath
        [XmlIgnore]
        private Uri _playerBadInputSoundPath = null;
        [XmlIgnore]
        public Uri PlayerBadInputSoundPath
        {
            get { return _playerBadInputSoundPath; }
            set
            {
                _playerBadInputSoundPath = value;
                RaisePropertyChange();
            }
        }
        [XmlElement("PlayerBadInputSoundPath")]
        public string PlayerBadInputSoundPathAsString
        {
            get { return PlayerBadInputSoundPath.ToString(); }
            set { PlayerBadInputSoundPath = (value == null ? null : new Uri(value.ToString())); }
        }
        #endregion

        #region IntervalSoundPath
        [XmlIgnore]
        private Uri _intervalSoundPath = null;
        [XmlIgnore]
        public Uri IntervalSoundPath
        {
            get { return _intervalSoundPath; }
            set
            {
                _intervalSoundPath = value;
                RaisePropertyChange();
            }
        }
        [XmlElement("IntervalSoundPath")]
        public string IntervalSoundPathAsString
        {
            get { return IntervalSoundPath.ToString(); }
            set { IntervalSoundPath = (value == null ? null : new Uri(value.ToString())); }
        }
        #endregion

        [XmlIgnore]
        private TimeSpan _intervalSound = TimeSpan.Zero;
        [XmlIgnore]
        public TimeSpan IntervalSound
        {
            get { return _intervalSound; }
            set
            {
                _intervalSound = value;
                RaisePropertyChange();
            }
        }
        [XmlElement("IntervalSound")]
        public long IntervalSoundTicks
        {
            get { return IntervalSound.Ticks; }
            set { IntervalSound = new TimeSpan(value); }
        }
        #endregion

        #region validation
        [XmlIgnore]
        public bool IsValid
        {
            get { return is_valid(); }
        }
        private bool is_uri_valid(Uri value)
        {
            return
            (
                value != null &&
                (value.Scheme == "pack" ||
                (value.IsFile && File.Exists(Uri.UnescapeDataString(value.AbsolutePath))))
            );
        }
        private bool is_valid()
        {
            return
                is_uri_valid(_playerDefeatImagePath) &&
                is_uri_valid(_playerSuccessImagePath) &&
                (TotalDuration > TimeSpan.Zero) &&
                ((TotalDuration > LockDuration) || IsLockFinal) &&
                is_uri_valid(AmbianceMusicSoundPath) &&
                is_uri_valid(PlayerDefeatSoundPath) &&
                is_uri_valid(PlayerSuccessSoundPath) &&
                is_uri_valid(PlayerBadInputSoundPath) &&

                is_uri_valid(IntervalSoundPath) &&
                IntervalSound > TimeSpan.Zero
                ;
        }
        #endregion
    }
}
