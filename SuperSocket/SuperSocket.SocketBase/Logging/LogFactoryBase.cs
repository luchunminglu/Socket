using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase.Logging
{
    /// <summary>
    /// Log factory base class
    /// </summary>
    public abstract class LogFactoryBase:ILogFactory
    {

        /// <summary>
        /// Gets teh config file path
        /// </summary>
        protected string ConfigFile { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the server instance is running  in isolation mode and the mulitple server instances share the same logging configuration
        /// </summary>
        protected bool IsSharedConfig { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configFile"></param>
        protected LogFactoryBase(string configFile)
        {
            if (Path.IsPathRooted(configFile))
            {
                //路径中包含根的值 如C:\
                ConfigFile = configFile;
                return;
            }

            if (Path.DirectorySeparatorChar != '\\')
            {
                //不是windows平台
                configFile = Path.GetFileNameWithoutExtension(configFile) + ".unix" + Path.GetExtension(configFile);
            }


            var currentAppDomain = AppDomain.CurrentDomain;
            var isolation = IsolationMode.None;

            var isolationValue = currentAppDomain.GetData(typeof (IsolationMode).Name);

            if (isolationValue != null)
            {
                isolation = (IsolationMode)isolationValue;
            }

            if (isolation == IsolationMode.None)
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);

                if (File.Exists(filePath))
                {
                    ConfigFile = filePath;
                    return;
                }

                filePath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config"), configFile);

                if (File.Exists(filePath))
                {
                    ConfigFile = filePath;
                    return;
                }

                ConfigFile = configFile;
                return;
            }
            else
            {
                //the running appserver is in isolated appdomain
                //1、search the appDomain's base directory
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);

                if (File.Exists(filePath))
                {
                    ConfigFile = filePath;
                    return;
                }

                //go to the application's root
                //the appdomain's root is /workingDir/DomainName, so get parent path twice to reach the application root
                var rootDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;

                //2.search the file with appdomain's name as prefix in the application's root
                //the config file whose name have appDomain's has higher priority

                filePath = Path.Combine(rootDir, AppDomain.CurrentDomain.FriendlyName + "." + configFile);

                if (File.Exists(filePath))
                {
                    ConfigFile = filePath;
                    return;
                }

                //3. search in the application's root without appdomain's name as prefix
                filePath = Path.Combine(rootDir, configFile);

                if (File.Exists(filePath))
                {
                    ConfigFile = filePath;
                    IsSharedConfig = true;
                    return;
                }

                rootDir = Path.Combine(rootDir, "Config");
                //Search the config file with appdomain's name as prefix in the Config dir
                filePath = Path.Combine(rootDir, AppDomain.CurrentDomain.FriendlyName + "." + configFile);

                if (File.Exists(filePath))
                {
                    ConfigFile = filePath;
                    return;
                }

                filePath = Path.Combine(rootDir, configFile);

                if (File.Exists(filePath))
                {
                    ConfigFile = filePath;
                    IsSharedConfig = true;
                    return;
                }

                ConfigFile = configFile;
                return;
            }

        }



        #region ILogFactory接口

        /// <summary>
        /// 根据名称获取ILog
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract ILog GetLog(string name);

        #endregion
    }
}
