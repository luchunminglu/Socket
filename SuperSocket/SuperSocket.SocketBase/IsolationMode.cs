using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase
{
    /// <summary>
    /// Appserver instance running isolation mode
    /// </summary>
    public enum IsolationMode
    {
        /// <summary>
        /// No isolation
        /// </summary>
        None,

        /// <summary>
        /// Isolation by appdomain
        /// </summary>
        AppDomain,

        /// <summary>
        /// Isolation by process
        /// </summary>
        Process,

    }
}
