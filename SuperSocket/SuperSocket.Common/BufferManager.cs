using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Common
{
    /// <summary>
    /// 创建一个大型缓冲区，该缓冲区可以进行分割并指定给SocketAsyncEventArgs对象以便用在每个套接字I/O操作中。这样可以很方便的重用缓冲区，并防止堆内存碎片化
    /// The operations exposed on the BufferManager class are not thread safe
    /// </summary>
    public class BufferManager
    {

        /// <summary>
        ///  The total number of bytes controlled by the buffer pool
        /// </summary>
        private int _numBytes;

        /// <summary>
        /// the underlying byte arry maintained by the buffer manager
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        /// 堆栈，栈顶存放最近使用的buffer的Offset
        /// </summary>
        private Stack<int> _freeIndexPool;

        /// <summary>
        /// 当前可用的起始索引
        /// </summary>
        private int _currentIndex;

        /// <summary>
        /// 剩余可用的buffer长度
        /// </summary>
        private int _bufferSize;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="totalBytes"></param>
        /// <param name="bufferSize"></param>
        public BufferManager(int totalBytes, int bufferSize)
        {
            _numBytes = totalBytes;
            _currentIndex = 0;
            _bufferSize = bufferSize;
            _freeIndexPool = new Stack<int>();
        }

        /// <summary>
        /// Allocate buffer space used by the buffer bool
        /// </summary>
        public void InitBuffer()
        {
            //Create one big large buffer and divide that out to each SocketAsyncEventArg object
            _buffer = new byte[_numBytes];
        }

        /// <summary>
        /// Assign a buffer from the buffer pool to the specified SocketAsyncEventArgs object
        /// </summary>
        /// <param name="args"></param>
        /// <returns>true, if the buffer was successfully set, else false</returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (_freeIndexPool.Count > 0)
            {
                //设置异步套接字操作的缓冲区
                //从栈顶取出最近使用的buffer的Offset，使用_bufferSize长度
                args.SetBuffer(_buffer, _freeIndexPool.Pop(), _bufferSize);
            }
            else
            {
                //numBytes - bufferSize = 已经使用的个数 在理想情况下应该等于currentIndex, 这样没有碎片
                if (_numBytes - _bufferSize < _currentIndex)
                {
                    return false;
                }

                args.SetBuffer(_buffer, _currentIndex, _bufferSize);
                //设置currentIndex为当前可用的起始索引
                _currentIndex += _bufferSize;
            }

            return true;
        }

        /// <summary>
        /// Removes the buffer from a SocketAsyncEventArg object.  
        /// This frees the buffer back to the buffer pool
        /// </summary>
        /// <param name="args"></param>
        public void FreeBuffer(SocketAsyncEventArgs args)
        {

            _freeIndexPool.Push(args.Offset);
            //此方法将 Buffer 属性设置为 null，将 Count 和Offset 属性设置为 zero
            args.SetBuffer(null, 0, 0);
        }

    }
}
