using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase.Logging
{
    /// <summary>
    /// Console log factory
    /// </summary>
    public class ConsoleLogFactory:ILogFactory
    {
        /// <summary>
        /// Get the log by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ILog GetLog(string name)
        {
            return new ConsoleLog(name);
        }
    }
}
