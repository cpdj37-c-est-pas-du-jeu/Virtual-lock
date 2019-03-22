using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPDJ_VirtualLock
{
    struct configuration
    {
        public TimeSpan total_duration;

        public String password;

        #region player attempts
        public uint try_before_lock; // what if 0 ?
        public bool lock_is_final;
        #endregion

        #region audio
        #endregion

        #region on_player_success
        class on_player_success
        {   // one or another
            public bool is_valid()
            {
                if (video_path.Length == 0 &&
                    image_path.Length == 0)
                    return false;



                return true;
            }

            public string video_path;
            public string image_path;
        }
        #endregion

        #region on_player_defeat
        #endregion
    }
}
