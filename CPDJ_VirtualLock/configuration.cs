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

        public static readonly string file_path = "./configuration.xml";

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
        [XmlElement("lock_duration")]
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
        [XmlIgnore]
        private Uri _playerSuccessImagePath = null;
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
        #endregion

        #region audio
        [XmlIgnore]
        private Uri _ambianceMusicSoundPath = null;
        public Uri AmbianceMusicSoundPath
        {
            get { return _ambianceMusicSoundPath; }
            set
            {
                _ambianceMusicSoundPath = value;
                RaisePropertyChange();
            }
        }
        [XmlIgnore]
        private Uri _playerDefeatSoundPath = null;
        public Uri PlayerDefeatSoundPath
        {
            get { return _playerDefeatSoundPath; }
            set
            {
                _playerDefeatSoundPath = value;
                RaisePropertyChange();
            }
        }
        [XmlIgnore]
        private Uri _playerSuccessSoundPath = null;
        public Uri PlayerSuccessSoundPath
        {
            get { return _playerSuccessSoundPath; }
            set
            {
                _playerSuccessSoundPath = value;
                RaisePropertyChange();
            }
        }
        [XmlIgnore]
        private Uri _playerBadInputSoundPath = null;
        public Uri PlayerBadInputSoundPath
        {
            get { return _playerBadInputSoundPath; }
            set
            {
                _playerBadInputSoundPath = value;
                RaisePropertyChange();
            }
        }
        [XmlIgnore]
        private Uri _intervalSoundPath = null;
        public Uri IntervalSoundPath
        {
            get { return _intervalSoundPath; }
            set
            {
                _intervalSoundPath = value;
                RaisePropertyChange();
            }
        }

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
                (value.IsFile && File.Exists(value.AbsolutePath)))
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
                is_uri_valid(IntervalSoundPath)
                ;
        }
        #endregion
    }
}
