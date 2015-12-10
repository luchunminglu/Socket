using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Common
{
    public sealed class SendingQueue:IList<ArraySegment<byte>>
    {
        private readonly int _offset;
        private readonly int _capacity;
        private int _currentCount = 0;
        private ArraySegment<byte>[] m_GlobalQueue;
        /// <summary>
        /// default(ArraySegment<byte>) = new ArraySegment<byte>(null,0,0)
        /// </summary>
        private static ArraySegment<byte> m_Null = default(ArraySegment<byte>);

        private int m_UpdatingCount;
        private bool m_ReadOnly = false;
        private ushort m_TrackID = 1;
        private int m_InnerOffset = 0;

        #region Public Properties

        /// <summary>
        /// Get the track ID
        /// </summary>
        public ushort TrackID
        {
            get
            {
                return m_TrackID;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="globalQueue"></param>
        /// <param name="offset"></param>
        /// <param name="capacity"></param>
        public SendingQueue(ArraySegment<byte>[] globalQueue, int offset, int capacity)
        {
            m_GlobalQueue = globalQueue;
            _offset = offset;
            _capacity = capacity;
        }

        #endregion

        #region Public Method


        //public bool TryEnqueue(ArraySegment<byte> item, out bool conflict, ushort trackId)
        //{
        //    conflict = false;

        //    var oldCount = _currentCount;
        //}

        #endregion

        #region Ilist 接口
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
