using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Common
{
    /// <summary>
    /// Binary util class
    /// </summary>
    public static class BinaryUtil
    {
        /// <summary>
        /// 在source中从pos开始查找length个元素，返回与target匹配的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="pos"></param>
        /// <param name="length"></param>
        /// <returns>如果找到，返回其索引，否则返回-1</returns>
        public static int IndexOf<T>(this IList<T> source, T target, int pos, int length) where T:IEquatable<T>
        {
            for (int i = pos; i < pos+length; i++)
            {
                if (source[i].Equals(target))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 在source中开始位置，查找所有元素，查找mark
        /// 如果找到，返回第一个匹配的索引，否则返回-1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="mark"></param>
        /// <returns>如果找到，返回第一个匹配的索引，否则返回-1</returns>
        public static int SearchMark<T>(this IList<T> source, T[] mark) where T : IEquatable<T>
        {
            return SearchMark(source, 0, source.Count, mark);
        }

        /// <summary>
        /// 在source中开始位置offset，查找length个长度，查找mark
        /// 如果找到，返回第一个匹配的索引，否则返回-1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="mark"></param>
        /// <returns> 如果找到，返回第一个匹配的索引，否则返回-1</returns>
        public static int SearchMark<T>(this IList<T> source, int offset, int length, T[] mark) where T : IEquatable<T>
        {
            if (mark == null || mark.Length <= 0)
            {
                throw new Exception("the search mark is null or empty");
            }

            for (int i = offset; i <= offset+length-mark.Length; i++)
            {
                for (int j = 0; j < mark.Length; j++)
                {
                    //i+j<=source.Count-1
                    if (!source[i + j].Equals(mark[j]))
                    {
                        break;
                    }

                    if (j == mark.Length - 1)
                    {
                        //已经完全匹配
                        return i;
                    }
                }
            }

            //没有找到匹配
            return -1;
        }

        /// <summary>
        /// source是否以mark开头，如果是返回offset,否则返回-1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public static int StartsWith<T>(this IList<T> source, T[] mark)
        {
            return StartsWith(source, 0, source.Count, mark);
        }

        /// <summary>
        /// source是否以mark开头，如果是返回offset,否则返回-1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public static int StartsWith<T>(this IList<T> source, int offset, int length, T[] mark)
        {
            if (mark == null || mark.Length <= 0)
            {
                throw new Exception("the search mark is null or empty");
            }

            int endOffset = offset + length - 1;

            for (int i = 0; i < mark.Length; i++)
            {
                if (offset + i > endOffset)
                {
                    return -1;
                }

                if (!source[offset + i].Equals(mark[i]))
                {
                    return -1;
                }
            }
            return offset;
        }

        /// <summary>
        /// source是否以mark结尾，如果是返回source.count - mark.length,否则返回-1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public static int EndsWith<T>(this IList<T> source, T[] mark)
        {
            return EndsWith(source, 0, source.Count, mark);
        }

        /// <summary>
        /// source是否以mark结尾，如果是返回length+offset-mark.length,否则返回-1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public static int EndsWith<T>(this IList<T> source, int offset, int length, T[] mark)
        {
            if (mark == null || mark.Length <= 0)
            {
                throw new Exception("the search mark is null or empty");
            }

            if (mark.Length > length)
            {
                return -1;
            }

            for (int i = 0; i < mark.Length; i++)
            {
                if (!source[length + offset - mark.Length+i].Equals(mark[i]))
                {
                    return -1;
                }
            }

            return length+offset-mark.Length;
        }

        /// <summary>
        /// 复制在指定范围内的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] CloneRange<T>(this IList<T> source, int offset, int length)
        {
            T[] target;
            var array = source as T[];

            if (array != null)
            {
                target = new T[length];
                Array.Copy(array,offset,target,0,length);
                return target;
            }

            target = new T[length];

            for (int i = 0; i < length; i++)
            {
                target[i] = source[offset + i];
            }

            return target;
        }

    }
}
