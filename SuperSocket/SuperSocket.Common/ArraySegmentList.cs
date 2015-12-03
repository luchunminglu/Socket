using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Common
{
    /// <summary>
    /// A list of ArraySegment
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArraySegmentList<T>:IList<T> where T:IEquatable<T>
    {
        #region Private Member

        /// <summary>
        /// 底层的存储
        /// </summary>
        private readonly List<ArraySegmentEx<T>> _segments;

        /// <summary>
        /// 暴露底层的存储
        /// </summary>
        public List<ArraySegmentEx<T>> Segments
        {
            get
            {
                return _segments;
            }
        }

        private int _count;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public ArraySegmentList()
        {
            _segments = new List<ArraySegmentEx<T>>();
        }


        #endregion

        #region IList 接口实现

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将ArraySegment中的元素复制到array中以arrayIndex开始的位置
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, 0, arrayIndex, Math.Min(array.Length-arrayIndex, _count));
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// List中所有元素T的数目。 是所有ArraySegment中T加起来的数目
        /// </summary>
        public int Count { get { return _count; } }

        /// <summary>
        /// 暗示List只读
        /// </summary>
        public bool IsReadOnly { get { return true; }}

        /// <summary>
        /// 获取在整个List中的第一个匹配元素的索引, 范围[0,_count]
        /// </summary>
        /// <param name="item"></param>
        /// <returns>if found, the index; otherwise, -1</returns>
        public int IndexOf(T item)
        {
            int index = 0;

            for (int i = 0; i < _segments.Count; i++)
            {
                foreach (var element in _segments[i].ArraySegment)
                {
                    if (element.Equals(item))
                    {
                        return index;
                    }
                    else
                    {
                        index++;
                    }
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get or set the element at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 && index >= _count)
                {
                    throw  new IndexOutOfRangeException("Index out  of range");
                }

                Tuple<int, int> tuple = BinarySearch(0, _segments.Count-1, index);

                if (tuple == null)
                {
                    throw  new Exception("发生不可能的逻辑异常，重点关注");
                }

                ArraySegmentEx<T> foundItem = _segments[tuple.Item1];

                return foundItem.ArraySegment.Array[tuple.Item2 + foundItem.Offset];
            }
            set
            {
                if (index < 0 && index >= _count)
                {
                    throw new IndexOutOfRangeException("Index out  of range");
                }

                Tuple<int, int> tuple = BinarySearch(0, _segments.Count - 1, index);

                if (tuple == null)
                {
                    throw new Exception("发生不可能的逻辑异常，重点关注");
                }

                ArraySegmentEx<T> foundItem = _segments[tuple.Item1];

                foundItem.ArraySegment.Array[tuple.Item2 + foundItem.Offset] = value;
            }
        }

        /// <summary>
        /// 查找索引为index的元素，返回tuple的第一项表示找到的ArraySegment的索引，第二项表示项在ArraySegment的位置(在ArraySegment经过Offset的位置)
        /// </summary>
        /// <param name="begin">起始查找的ArraySegment的索引，包含</param>
        /// <param name="end">结束查找的ArraySegment的索引，包含</param>
        /// <param name="index">要在整个list中查找的索引</param>
        /// <returns>返回tuple的第一项表示找到的ArraySegment的索引，第二项表示项在ArraySegment的位置</returns>
        protected Tuple<int,int> BinarySearch(int begin, int end,int index)
        {
            if (begin > end)
            {
                //找不到index
                return null;
            }

            int mid = (begin + end)/2;
            ArraySegmentEx<T> curItem = _segments[mid];
            if (index >= curItem.From && index <= curItem.To)
            {
                //找到所在的ArraySegment,获取其当前ArraySegment的索引
                int curPosition = index - curItem.From;
                return new Tuple<int, int>(mid,curPosition);
            }

            //查找左侧
            if (index < curItem.From)
            {
                return BinarySearch(begin,mid-1,index);
            }
            else
            {
                //index > curItem.To的情况 查找右侧
                return BinarySearch(mid+1,end,index);
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Removes the segment at index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveSegmentAt(int index)
        {
            var removeCount = _segments[index].Count;//要移除的元素
            bool isRemoveLast = _segments.Count - 1 == index;//是否移除最后一个元素
            _segments.RemoveAt(index);//移除元素

            if (!isRemoveLast)
            {
                for (int i = index; i < _segments.Count; i++)
                {
                    //将移除元素之后的元素的From和To前移
                    _segments[i].From -= removeCount;
                    _segments[i].To -= removeCount;
                }
            }

            _count -= removeCount;
        }

        /// <summary>
        /// Adds the segment to the list
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset">在Array中开始复制的位置</param>
        /// <param name="length">要复制的元素数目</param>
        /// <param name="isCreateNewArray">true，不使用array，而创建一个新的数组；false,使用array</param>
        public void AddSegment(T[] array, int offset, int length, bool isCreateNewArray=false)
        {
            if (length <= 0)
            {
                return;
            }

            var currentCount = _count;
            ArraySegmentEx<T> segmentToAdd = null;

            if (!isCreateNewArray)
            {
                segmentToAdd = new ArraySegmentEx<T>(array,offset,length);
            }
            else
            {
                segmentToAdd = new ArraySegmentEx<T>(array.CloneRange(offset,length),0,length);
            }

            segmentToAdd.From = currentCount;
            _count = currentCount + segmentToAdd.Count;
            segmentToAdd.To = _count - 1;
            _segments.Add(segmentToAdd);
        }

        /// <summary>
        /// 清空segment List
        /// </summary>
        public void ClearSegments()
        {
            _segments.Clear();
            _count = 0;
        }

        /// <summary>
        /// 将Array List中的所有数据复制到数组中
        /// </summary>
        /// <returns></returns>
        public T[] ToArrayData()
        {
            return ToArrayData(0,_count);
        }

        /// <summary>
        /// 将Array Segment List中的元素从startIndex开始的length个元素复制到array中
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public T[] ToArrayData(int startIndex, int length)
        {
            var result = new T[length];

            Tuple<int, int> tuple = null;
            if (startIndex == 0)
            {
                //因为通常起始位置是0，所以加一个判断
                tuple = new Tuple<int, int>(0, 0);
            }
            else
            {
                tuple = BinarySearch(0, _segments.Count - 1, startIndex);
            }

            if (tuple == null)
            {
                throw new Exception("未在ArraySegmentList中找到index={0}的元素".FormatWith(startIndex));
            }

            //开始的搜索位置
            int from = startIndex - _segments[tuple.Item1].From;
            int total = 0;//总共复制的length
            int len = 0;//当前需要复制的len
            for (int i = tuple.Item1; i < _segments.Count; i++)
            {
                var curSegment = _segments[i];
                //需要复制的元素的个数
                len = Math.Min(curSegment.Count-from,length-total);
                if (len > 0)
                {
                    //len>0防止空的Segment
                    Array.Copy(curSegment.Array, curSegment.Offset + from, result, total, len);
                    total += len;
                }

                if (total >= length)
                {
                    return result;
                }
                //只有第一个的from不是0
                from = 0;
            }
            return result;
        }

        /// <summary>
        /// Trims the end
        /// 在 arraySegmentlist中删除结尾trimSize个元素
        /// </summary>
        /// <param name="trimSize"></param>
        public void TrimEnd(int trimSize)
        {
            if (trimSize <= 0)
            {
                return;
            }

            //现在的结尾元素索引
            int expectedTo = _count - 1 - trimSize;

            for (int i = _segments.Count - 1; i >= 0; i--)
            {
                var curSegment = _segments[i];
                if (expectedTo >= curSegment.From && expectedTo <= curSegment.To)
                {
                    //减掉该项的结尾
                    _count -= curSegment.To - expectedTo;
                    int curFrom = curSegment.From;
                    //重置to
                    int curTo = expectedTo;
                    _segments[i] = new ArraySegmentEx<T>(_segments[i].Array,_segments[i].Offset,curTo-curFrom+1){From = curFrom,To=curTo};
                  
                    return;
                }

                RemoveSegmentAt(i);
            }
        }

        /// <summary>
        /// 将ArraySegmentList中元素复制到to中
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public int CopyTo(T[] to)
        {
            return CopyTo(to, 0, 0, Math.Min(_count, to.Length));
        }

        /// <summary>
        /// 从ArraySegmentList中复制length个元素到to中
        /// </summary>
        /// <param name="to">要复制元素的目的地</param>
        /// <param name="srcIndex">复制源的起始位置</param>
        /// <param name="toIndex">目的地的起始位置</param>
        /// <param name="length">要复制的元素的个数</param>
        /// <returns>实际复制的数据数量</returns>
        public int CopyTo(T[] to, int srcIndex, int toIndex, int length)
        {
            Tuple<int, int> tuple = null;
            if (srcIndex == 0)
            {
                //因为通常是从0开始复制的，加一个判断
                tuple = new Tuple<int, int>(0,0);
            }
            else
            {
                //查找复制源的起始位置
                tuple = BinarySearch(0, _segments.Count - 1, srcIndex);
            }

            if (tuple == null)
            {
                throw new Exception("未在ArraySegmentList中找到{0}的元素".FormatWith(srcIndex));
            }

            //复制开始的ArraySegment
            int startIndex = tuple.Item1;
            ArraySegmentEx<T> startItem = _segments[startIndex];

            int copied = 0;//已经复制的元素数
            int numNeedToCopy = Math.Min(length, startItem.Count - tuple.Item2);//当前需要复制的元素数
            if (numNeedToCopy > 0)
            {
                //防止空数组
                //tuple.Item2+startItem.Offset表示Array的起始位置
                Array.Copy(startItem.Array, tuple.Item2 + startItem.Offset, to, toIndex,numNeedToCopy);
                copied += numNeedToCopy;
            }

            if (copied >= length)
            {
                //已经复制完成,实际上等于copied
                return copied;
            }
            
            //还需要复制
            for (int i = startIndex+1; i < _segments.Count; i++)
            {
                var segment = _segments[i];
                numNeedToCopy = Math.Min(segment.Count, length - copied);
                if (numNeedToCopy > 0)
                {
                    Array.Copy(segment.Array,segment.Offset,to,toIndex+copied,numNeedToCopy);
                    copied += numNeedToCopy;
                }

                if (copied >= length)
                {
                    return copied;
                }
            }
            //没有复制足length个元素
            return copied;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 返回Segment的数目
        /// </summary>
        public int SegmentCount
        {
            get { return _segments.Count; }
        }

        #endregion

    }

    /// <summary>
    /// a array segment list of byte
    /// </summary>
    public class ArraySegmentList : ArraySegmentList<byte>
    {
        /// <summary>
        /// 使用UTF8解析字符串
        /// </summary>
        /// <returns></returns>
        public string Decode()
        {
            return Decode(Encoding.UTF8);
        }

        /// <summary>
        /// 使用指定的encoding的解析bytes为字符串
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string Decode(Encoding encoding)
        {
            return Decode(encoding, 0, Count);
        }

        /// <summary>
        /// 将arraysegmentlist中从offset开始解析length个字节为字符串
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string Decode(Encoding encoding, int offset, int length)
        {
            if (Segments == null || Segments.Count == 0 || length == 0)
            {
                return string.Empty;
            }

            Tuple<int, int> tuple = null;
            if (offset == 0)
            {
                tuple = new Tuple<int, int>(0,0);
            }
            else
            {
                tuple = BinarySearch(0, Segments.Count - 1, offset);
            }

            if (tuple == null)
            {
                throw new Exception("未找到index={0}的位置".FormatWith(offset));
            }

            //对count数目的字节最大的字符数组
            var charBuffer = new char[encoding.GetMaxCharCount(length)];

            int byteUsed, charUsed;//本次解析已经使用的byte char
            var decoder = encoding.GetDecoder();
            int byteTotal = 0;//所有已经是用的byte char
            int charTotal = 0;
            bool completed;
            for (int i = tuple.Item1; i < SegmentCount; i++)
            {
                ArraySegmentEx<byte> segment = Segments[i];

                int decodeOffset = segment.Offset;
                int numToDecoded = Math.Min(length - byteTotal, segment.Count);

                if (i == tuple.Item1 && offset > 0)
                {
                    decodeOffset = offset - segment.From + segment.Offset;
                    numToDecoded = Math.Min(segment.Count - (offset - segment.From), numToDecoded);
                }

                decoder.Convert(segment.Array,decodeOffset,numToDecoded,charBuffer,charTotal,charBuffer.Length-charTotal,i==SegmentCount -1,out byteUsed,out charUsed,out completed);

                byteTotal += byteUsed;
                charTotal += charUsed;

                if (byteTotal >= length)
                {
                    //解析结束
                    break;
                }
            }

            return new String(charBuffer, 0, charTotal);
        }

    }

    #region 辅助类
    /// <summary>
    /// ArraySegment拓展
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArraySegmentEx<T>
    {
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

        #region Properties

        /// <summary>
        /// 底层存储
        /// </summary>
        public ArraySegment<T> ArraySegment { get; private set; }

        /// <summary>
        /// 底层数组
        /// </summary>
        public T[] Array { get { return ArraySegment.Array; } }

        /// <summary>
        /// ArraySegment中的元素数目
        /// </summary>
        public int Count { get { return ArraySegment.Count; } }

        /// <summary>
        /// 获取ArraySegment在底层数组的偏移量
        /// </summary>
        public int Offset { get { return ArraySegment.Offset; } }

        /// <summary>
        /// 这里的From并不是指Offset，而是在一个ArraySegmentList中的索引起始位置
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// 在一个ArraySegmentList中的索引结束位置=from+count-1
        /// </summary>
        public int To { get; set; }

        #endregion
    }
    #endregion
}
