using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Common
{
    /// <summary>
    /// ArraySegment拓展
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArraySegmentEx<T>
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public ArraySegmentEx(T[] array, int offset, int count)
        {
            ArraySegment = new ArraySegment<T>(array, offset, count);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 底层实际存储
        /// </summary>
        public ArraySegment<T> ArraySegment { get; private set; }

        /// <summary>
        /// 底层数组
        /// </summary>
        public T[] Array { get { return ArraySegment.Array; } }

        /// <summary>
        /// ArraySegment的元素数目
        /// </summary>
        public int Count { get { return ArraySegment.Count; } }

        /// <summary>
        /// 获取ArraySegment在底层数组中的偏移量
        /// </summary>
        public int Offset { get { return ArraySegment.Offset; } }

        /// <summary>
        /// 这里的From并不是指Offset,而是指其在一个ArraySegmentList中全盘考虑的索引起始位置
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// 在一个ArraySegmentList中的索引结束位置（包含）= from+count-1
        /// </summary>
        public int To { get; set; }
        #endregion
    }
}
