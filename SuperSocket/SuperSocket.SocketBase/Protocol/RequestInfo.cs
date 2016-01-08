using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase.Protocol
{
    public class RequestInfo<TRequestBody>:IRequestInfo<TRequestBody>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        protected RequestInfo()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="body">The body</param>
        public RequestInfo(string key, TRequestBody body)
        {
            Initialize(key,body);
        } 

        /// <summary>
        /// init with the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="body"></param>
        protected void Initialize(string key, TRequestBody body)
        {
            this.Key = key;
            this.Body = body;
        }


        #region IRequestInfo接口

        /// <summary>
        /// Gets the body
        /// </summary>
        public TRequestBody Body { get; private set; }

        /// <summary>
        /// Get the key of this request
        /// </summary>
        public string Key{ get; private set; }

        #endregion
    }

    /// <summary>
    /// RequestInfo with header
    /// </summary>
    /// <typeparam name="TRequestHeader"></typeparam>
    /// <typeparam name="TRequestBody"></typeparam>
    public class RequestInfo<TRequestHeader, TRequestBody> : RequestInfo<TRequestBody>, IRequestInfo<TRequestHeader, TRequestBody>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RequestInfo()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="header"></param>
        /// <param name="body"></param>
        public RequestInfo(string key, TRequestHeader header, TRequestBody body):base(key,body)
        {
            this.Header = header;
        }


        /// <summary>
        /// Initializes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="header">The header.</param>
        /// <param name="body">The body.</param>
        public void Initialize(string key, TRequestHeader header, TRequestBody body)
        {
            base.Initialize(key, body);
            Header = header;
        }

        #region IRequestInfo接口的一部分

        /// <summary>
        /// 请求的Header
        /// </summary>
        public TRequestHeader Header { get; private set; }

        #endregion
    }

}
