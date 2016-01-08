using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase.Protocol
{
    /// <summary>
    /// Binary type request information
    /// </summary>
    public class BinaryRequestInfo:RequestInfo<byte[]>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="body"></param>
        public BinaryRequestInfo(string key, byte[] body):base(key,body)
        {
            
        }

    }
}
