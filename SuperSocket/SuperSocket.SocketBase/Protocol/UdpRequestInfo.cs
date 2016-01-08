using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase.Protocol
{
    /// <summary>
    /// UdpRequestInfo , it is designed for passing in business sesssion ID to udp request info
    /// </summary>
    public class UdpRequestInfo:IRequestInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sessionID"></param>
        public UdpRequestInfo(string key, string sessionID)
        {
            this.Key = key;
            this.SessionID = sessionID;
        }


        /// <summary>
        /// Gets the session ID
        /// </summary>
        public string SessionID { get; private set; }

        #region IRequestInfo

        /// <summary>
        /// Gets the key of this request
        /// </summary>
        public string Key { get; private set; }

        #endregion
    }
}
