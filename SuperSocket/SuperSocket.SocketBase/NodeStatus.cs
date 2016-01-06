using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase
{
    /// <summary>
    /// The status of one supersocket node
    /// </summary>
    [Serializable]
    public class NodeStatus
    {

        /// <summary>
        /// Gets or sets the bootstrap status
        /// </summary>
        public StatusInfoCollection BootstrapStatus { get; set; }

        /// <summary>
        /// Gets or sets the status of all server instances running in this node
        /// </summary>
        public StatusInfoCollection[] InstancesStatus { get; set; }

        /// <summary>
        /// Saves the specified file path
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(string filePath)
        {
            var serializer = new BinaryFormatter();

            using (var stream = File.Create(filePath))
            {
                serializer.Serialize(stream,this);
                stream.Flush();
            }
        }

        /// <summary>
        /// Loads a NodeStatus instance from a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static NodeStatus LoadFrom(string filePath)
        {
            var serializer = new BinaryFormatter();

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var status = serializer.Deserialize(stream) as NodeStatus;
                return status;
            }
        }

    }
}
