using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public class Configuration// : GCL.MetaConfiguration
    {
        public static readonly string file_path = "./configuration.xml";

        public Configuration()
        {
            // base.Load();
        }

        public TimeSpan total_duration = TimeSpan.FromMinutes(1);

        public String password = "toto";

        #region player attempts
        public uint try_before_lock = 0;
        public TimeSpan lock_duration = TimeSpan.FromSeconds(10);
        public bool lock_is_final = false;
        #endregion

        #region audio
        #endregion

        #region game_over
  
        #endregion
    }
}
