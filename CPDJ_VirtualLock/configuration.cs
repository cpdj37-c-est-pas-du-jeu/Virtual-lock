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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public static readonly string file_path = "./configuration.xml";

        public Configuration()
        {
            // base.Load();
        }

        [XmlIgnore]
        private TimeSpan _totalDuration = TimeSpan.FromMinutes(1);
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
        private long TotalDurationTicks
        {
            get { return TotalDuration.Ticks; }
            set { TotalDuration = new TimeSpan(value); }
        }

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

        #region player attempts
        [XmlIgnore]
        private uint _tryBeforeLock = 0;
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
        private TimeSpan _lockDuration = TimeSpan.FromSeconds(10);
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
        private long LockDurationTicks
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
                if (value == true)
                    LockDuration = TimeSpan.MaxValue;
                _is_lock_final = value;
                RaisePropertyChange();
            }
        }
        #endregion

        #region game_over
        [XmlIgnore]
        private String _playerDefeatImagePath = new Uri("pack://application:,,,/ressources/images/icons/missing_image.png").AbsolutePath;
        public String PlayerDefeatImagePath
        {
            get { return _playerDefeatImagePath; }
            set
            {
                _playerDefeatImagePath = value;
                RaisePropertyChange();
            }
        }
        [XmlIgnore]
        private String _playerSuccessImagePath = new Uri("pack://application:,,,/ressources/images/icons/missing_image.png").AbsolutePath;
        public String PlayerSuccessImagePath
        {
            get { return _playerSuccessImagePath; }
            set
            {
                _playerSuccessImagePath = value;
                RaisePropertyChange();
            }
        }
        #endregion

        #region audio
        #endregion
    }
}
