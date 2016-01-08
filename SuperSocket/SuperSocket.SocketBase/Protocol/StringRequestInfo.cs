using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase.Protocol
{
    /// <summary>
    /// String type request information
    /// </summary>
    public class StringRequestInfo:RequestInfo<String>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="body"></param>
        /// <param name="parameters"></param>
        public StringRequestInfo(string key, string body, string[] parameters):base(key,body)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Gets the parameters
        /// </summary>
        public string[] Parameters { get; private set; }

        /// <summary>
        /// Gets the first param.
        /// </summary>
        /// <returns></returns>
        public string GetFirstParam()
        {
            if (Parameters.Length > 0)
            {
                return Parameters[0];
            }

            return string.Empty;
        }

        /// <summary>
        ///  Gets the <see cref="System.String"/> at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get { return Parameters[index]; }
        }
    }
}
