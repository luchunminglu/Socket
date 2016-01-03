using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Common
{
    /// <summary>
    /// this class is designed to detect platform attribute in runtime
    /// </summary>
    public static class Platform
    {
        static Platform()
        {
            Socket socket = null;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.IOControl(IOControlCode.KeepAliveValues, null, null);
                SupportSocketIOControlByCodeEnum = true;
            }
            catch (NotSupportedException)
            {
                SupportSocketIOControlByCodeEnum = false;
            }
            catch (NotImplementedException)
            {
                SupportSocketIOControlByCodeEnum = false;
            }
            catch (Exception)
            {
                SupportSocketIOControlByCodeEnum = true;
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }

            Type t = Type.GetType("Mono.Runtime");
            IsMono = t != null;
        }

        /// <summary>
        /// Gets a value indicating whether [support socket IO control by code enum].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [support socket IO control by code enum]; otherwise, <c>false</c>.
        /// </value>
        public static bool SupportSocketIOControlByCodeEnum { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is mono.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is mono; otherwise, <c>false</c>.
        /// </value>
        public static bool IsMono { get; private set; }

    }
}
