using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase.Config
{
    /// <summary>
    /// Certificate configuration interface
    /// 证书配置
    /// </summary>
    public interface ICertificateConfig
    {
        /// <summary>
        /// Gets the file path
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Gets the password
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Gets the store name where certificate locates
        /// 
        /// the name of the store
        /// </summary>
        string StoreName { get; }

        /// <summary>
        /// Gets the thumbprint
        /// 获取指纹
        /// </summary>
        string Thumbprint { get; }

        /// <summary>
        /// Gets the store location of the certificate
        /// 指定 X.509 证书存储区的位置。
        /// </summary>
        StoreLocation StoreLocation { get; }

        /// <summary>
        /// Gets a  value indicating whether [client certificate required]
        /// </summary>
        bool ClientCertificateRequired { get; }

        /// <summary>
        /// Gets a value that will be used to instantiate the X509Certificate2 object in the CertificateManager
        /// 定义将 X.509 证书的私钥导出到何处以及如何导出。
        /// </summary>
        X509KeyStorageFlags KeyStorageFlags { get; }

    }
}
