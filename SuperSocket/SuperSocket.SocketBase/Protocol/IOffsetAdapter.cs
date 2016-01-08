using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase.Protocol
{
    /// <summary>
    /// The interface for a receive filter to adapt receiving buffer offset
    /// </summary>
    public interface IOffsetAdapter
    {

        /// <summary>
        /// gets the offset delta
        /// </summary>
        int OffsetDelta { get; }
    }
}
