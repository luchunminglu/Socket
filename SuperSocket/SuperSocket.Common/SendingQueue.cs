using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Common
{
    /// <summary>
    /// SendingQueue
    /// </summary>
    public sealed class SendingQueue : IList<ArraySegment<byte>>
    {
        #region Private Member

        /// <summary>
        /// 
        /// </summary>
        private readonly int m_Offset;

        private readonly int m_Capacity;

        private int m_CurrentCount = 0;

        private ArraySegment<byte>[] m_GlobalQueue;

        private static ArraySegment<byte> m_Null = default(ArraySegment<byte>);

        private int m_UpdatingCount;

        private bool m_ReadOnly = false;

        private ushort m_TrackID = 1;

        private int m_InnerOffset = 0;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="globalQueue"></param>
        /// <param name="offset"></param>
        /// <param name="capacity"></param>
        public SendingQueue(ArraySegment<byte>[] globalQueue, int offset, int capacity)
        {
            m_GlobalQueue = globalQueue;
            m_Offset = offset;
            m_Capacity = capacity;
        }

        #endregion

        #region Public Member

        /// <summary>
        /// Get the track ID
        /// </summary>
        public ushort TrackID
        {
            get { return m_TrackID;}
        }

        #endregion

        #region Method


        private bool TryEnqueue(ArraySegment<byte> item, out bool conflict, ushort trackID)
        {
            conflict = false;

            var oldCount = m_CurrentCount;

        }

        #endregion

        #region IList接口
        public int IndexOf(ArraySegment<byte> item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, ArraySegment<byte> item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public ArraySegment<byte> this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(ArraySegment<byte> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(ArraySegment<byte> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(ArraySegment<byte>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(ArraySegment<byte> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ArraySegment<byte>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
