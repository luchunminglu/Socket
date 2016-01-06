using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Logging;

namespace SuperSocket.SocketBase
{
    /// <summary>
    /// The interface for who provider logger
    /// </summary>
    public interface ILoggerProvider
    {
        /// <summary>
        /// Get the logger 
        /// </summary>
        ILog Logger { get; }
    }
}
