using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.SocketBase
{
    /// <summary>
    /// Status information collection
    /// </summary>
    [Serializable]
    public class StatusInfoCollection
    {
        /// <summary>
        /// 底层存储
        /// </summary>
        [NonSerialized]
        private Dictionary<string,object> m_Values = new Dictionary<string, object>();

        /// <summary>
        /// 序列化辅助对象
        /// </summary>
        private List<KeyValuePair<string, object>> m_InternaList; 

        /// <summary>
        /// Gets the values
        /// </summary>
        public Dictionary<string, object> Values
        {
            get { return m_Values;}
        }

        /// <summary>
        /// Get or set the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or set the tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Get or set the collected time
        /// </summary>
        public DateTime CollectedTime { get; set; }

        /// <summary>
        /// Gets or sets the Object with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                object value;
                if (m_Values.TryGetValue(name, out value))
                {
                    return value;
                }

                return null;
            }
            set { m_Values[name] = value; }
        }

        /// <summary>
        /// Get the value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValue<T>(string name, T defaultValue) where T : struct
        {
            object value;

            if (m_Values.TryGetValue(name, out value))
            {
                return (T)value;
            }

            return defaultValue;
        }

        /// <summary>
        /// 序列化之前调用
        /// </summary>
        /// <param name="context"></param>
        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {
            m_InternaList = new List<KeyValuePair<string, object>>(m_Values.Count);
            foreach (var entry in m_Values)
            {
                m_InternaList.Add(new KeyValuePair<string, object>(entry.Key,entry.Value));
            }
        }

        /// <summary>
        /// 反序列化之后调用
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            if (m_InternaList == null || m_InternaList.Count <= 0)
            {
                return;
            }

            if (m_Values == null)
            {
                m_Values = new Dictionary<string, object>();
            }

            foreach (var entry in m_InternaList)
            {
                m_Values.Add(entry.Key,entry.Value);
            }

            m_InternaList = null;
        }

    }
}
