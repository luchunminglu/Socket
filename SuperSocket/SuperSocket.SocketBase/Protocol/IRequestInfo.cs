using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase.Protocol
{
    /// <summary>
    /// Request information interface
    /// </summary>
    public interface IRequestInfo
    {
        /// <summary>
        /// Gets the key of this request
        /// </summary>
        string Key { get; }
    }

    /// <summary>
    /// Request information interface
    /// </summary>
    /// <typeparam name="TRequestBody"></typeparam>
    public interface IRequestInfo<TRequestBody> : IRequestInfo
    {
        /// <summary>
        /// Gets the body of this request
        /// </summary>
        TRequestBody Body { get; }
    }

    /// <summary>
    /// Request information interface
    /// </summary>
    /// <typeparam name="TRequestHeader"></typeparam>
    /// <typeparam name="TRequestBody"></typeparam>
    public interface IRequestInfo<TRequestHeader, TRequestBody> : IRequestInfo<TRequestBody>
    {
        /// <summary>
        /// Gets the header of the request
        /// </summary>
        TRequestHeader Header { get; }
    }

}
