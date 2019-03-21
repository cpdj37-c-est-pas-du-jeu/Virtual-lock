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
    }
}
