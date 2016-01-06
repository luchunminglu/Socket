using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase
{
    /// <summary>
    /// Listener information
    /// </summary>
    [Serializable]
    public class ListenerInfo
    {

        /// <summary>
        /// Gets or sets the listner endpoint
        /// </summary>
        public IPEndPoint EndPoint { get; set; }

        /// <summary>
        /// Gets or sets the listener backlog
        /// </summary>
        public int BackLog { get; set; }

        /// <summary>
        /// Gets or sets the security protocol
        /// </summary>
        public SslProtocols Security { get; set; }

    }
}
