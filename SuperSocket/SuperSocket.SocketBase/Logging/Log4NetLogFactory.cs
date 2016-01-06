using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using log4net;

namespace SuperSocket.SocketBase.Logging
{
    /// <summary>
    /// Log4NetFactory
    /// </summary>
    public class Log4NetLogFactory:LogFactoryBase
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public Log4NetLogFactory(): base("log4net.config")
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="log4netConfig"></param>
        public Log4NetLogFactory(string log4netConfig) : base(log4netConfig)
        {
            if (!IsSharedConfig)
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(ConfigFile));
            }
            else
            {
                //Disable performance logger
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                var docElement = xmlDoc.DocumentElement;
                var perfLogNode = docElement.SelectSingleNode("logger[@name='Performance']");
                if (perfLogNode != null)
                {
                    docElement.RemoveChild(perfLogNode);
                }
                log4net.Config.XmlConfigurator.Configure(docElement);
            }
        }

        #region LogFactoryBase 实现

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override ILog GetLog(string name)
        {
            return new Log4NetLog(LogManager.GetLogger(name));
        }
        #endregion

    }
}
