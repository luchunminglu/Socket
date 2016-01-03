using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Common
{
    /// <summary>
    /// Socket Extension Class
    /// </summary>
    public static class SocketEx
    {

        /// <summary>
        /// Close the socket safely 
        /// </summary>
        /// <param name="socket"></param>
        public static void SafeClose(ref Socket socket)
        {
            if (socket == null)
            {
                return;
            }

            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {

            }

            try
            {
                socket.Close();
            }
            catch (Exception)
            {

            }
            finally
            {
                socket = null;
            }
        }

        /// <summary>
        /// Sends the data.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="data">The data.</param>
        public static void SendData(this Socket client, byte[] data)
        {
            SendData(client, data, 0, data.Length);
        }

        /// <summary>
        /// sends  the  data
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="data">要发送的数据</param>
        /// <param name="offset">发送数据起始的位置</param>
        /// <param name="length">要发送数据的数量</param>
        public static void SendData(this Socket client, byte[] data, int offset, int length)
        {
            int sent = 0;
            int thisSent = 0;

            while ((length - sent) > 0)
            {
                thisSent = client.Send(data, offset + sent, length - sent,SocketFlags.None);
                sent += thisSent;
            }
        }

    }
}
