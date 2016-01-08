using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.Common;
using SuperSocket.SocketBase.Config;

namespace SuperSocket.SocketBase.Security
{
    /// <summary>
    /// 证书管理器
    /// </summary>
    internal static class CertificateManager
    {

        internal static X509Certificate Initialize(ICertificateConfig cerConfig, Func<string, string> relativePathHandler)
        {
            if (!string.IsNullOrEmpty(cerConfig.FilePath))
            {
                //To keep compatible with website hosting
                string filePath;

                if (Path.IsPathRooted(cerConfig.FilePath))
                {
                    filePath = cerConfig.FilePath;
                }
                else
                {
                    filePath = relativePathHandler(cerConfig.FilePath);
                }

                //使用一个证书文件名、一个密码和一个密钥存储标志初始化X509Certificate2类的实例
                return new X509Certificate2(filePath, cerConfig.Password, cerConfig.KeyStorageFlags);
            }
            else
            {
                var storeName = cerConfig.StoreName;
                if (string.IsNullOrEmpty(storeName))
                {
                    storeName = "Root";
                }

                var store = new X509Store(storeName,cerConfig.StoreLocation);
                store.Open(OpenFlags.ReadOnly);

                var cert = store.Certificates.OfType<X509Certificate2>().Where(c =>StringUtils.EqualsEx(c.Thumbprint,cerConfig.Thumbprint)).FirstOrDefault();
                store.Close();

                return cert;
            }
        }

    }
}
