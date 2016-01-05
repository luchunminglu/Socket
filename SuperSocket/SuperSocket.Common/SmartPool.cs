using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperSocket.Common
{
    /// <summary>
    /// The pool information class
    /// </summary>
    public interface IPoolInfo
    {
        /// <summary>
        /// Get the min size of the Pool
        /// </summary>
        int MinPoolSize { get; }

        /// <summary>
        /// Get the max size of the pool
        /// </summary>
        int MaxPoolSize { get; }

        /// <summary>
        /// Get the avialable items count
        /// </summary>
        int AvialableItemsCount { get; }

        /// <summary>
        /// Gets the total items count, include items in the pool and outside the pool
        /// </summary>
        int TotalItemsCount { get; }
    }

    /// <summary>
    /// The basic interface of smart pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISmartPool<T> : IPoolInfo
    {
        /// <summary>
        /// Initializes the specified min pool size
        /// </summary>
        /// <param name="minPoolSize">The min size of the pool.</param>
        /// <param name="maxPoolSize">The max size of the pool.</param>
        /// <param name="sourceCreator">The source creatro</param>
        void Initialize(int minPoolSize, int maxPoolSize, ISmartPoolSourceCreator<T> sourceCreator);

        /// <summary>
        /// Pushes the specified item into the pool
        /// </summary>
        /// <param name="item">The item</param>
        void Push(T item);

        /// <summary>
        /// Try to get one item from the pool
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool TryGet(out T item);
    }

    /// <summary>
    /// ISmartPoolSource
    /// </summary>
    public interface ISmartPoolSource
    {

        /// <summary>
        /// Gets the count
        /// </summary>
        int Count { get; }
    }

    /// <summary>
    /// SmartPoolSource
    /// </summary>
    public class SmartPoolSource : ISmartPoolSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmartPoolSource" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="itemsCount">The items count.</param>
        public SmartPoolSource(object source, int itemsCount)
        {
            Source = source;
            Count = itemsCount;
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public object Source { get; private set; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; private set; }
    }

    /// <summary>
    /// ISmartPoolSourceCreator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISmartPoolSourceCreator<T>
    {
        /// <summary>
        /// Creates the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="poolItems">The pool items.</param>
        /// <returns></returns>
        ISmartPoolSource Create(int size, out T[] poolItems);
    }

    /// <summary>
    /// The smart pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SmartPool<T> : ISmartPool<T>
    {

        #region Private Member

        private ConcurrentStack<T> m_GlobalStack;

        private ISmartPoolSource[] m_ItemsSource;

        /// <summary>
        /// 当前source的个数
        /// </summary>
        private int m_CurrentSourceCount;

        private ISmartPoolSourceCreator<T> m_SourceCreator;

        private int m_MinPoolSize;

        private int m_MaxPoolSize;

        private int m_TotalItemsCount;

        private int m_IsIncreasing = 0;
        #endregion

        #region ISmartPool 接口实现

        /// <summary>
        /// The size of the min pool
        /// </summary>
        public int MinPoolSize { get{return m_MinPoolSize;} }

        /// <summary>
        /// The size of max pool
        /// </summary>
        public int MaxPoolSize { get{return m_MaxPoolSize;} }

        /// <summary>
        /// Get the avialable items count.
        /// </summary>
        public int AvialableItemsCount { get { return m_GlobalStack.Count; } }

        /// <summary>
        /// Get the total items count, include items in the pool and outside the pool
        /// </summary>
        public int TotalItemsCount { get{return m_TotalItemsCount;} }

        /// <summary>
        /// Initializes the specified min and max pool size
        /// </summary>
        /// <param name="minPoolSize">The min size of the pool</param>
        /// <param name="maxPoolSize">The max size of the pool</param>
        /// <param name="sourceCreator">The source creator</param>
        public void Initialize(int minPoolSize, int maxPoolSize, ISmartPoolSourceCreator<T> sourceCreator)
        {
            m_MinPoolSize = minPoolSize;
            m_MaxPoolSize = maxPoolSize;
            m_SourceCreator = sourceCreator;
            m_GlobalStack = new ConcurrentStack<T>();

            int n = 0;

            if (minPoolSize != maxPoolSize)
            {
                int currentValue = minPoolSize;

                while (true)
                {
                    n++;

                    int thisValue = currentValue*2;
                    if (thisValue >= maxPoolSize)
                    {
                        break;
                    }
                    currentValue = thisValue;
                }
            }
            //n至少是0
            m_ItemsSource = new ISmartPoolSource[n+1];

            T[] items;
            m_ItemsSource[0] = sourceCreator.Create(minPoolSize, out items);
            m_CurrentSourceCount = 1;

            for (var i = 0; i < items.Length; i++)
            {
                m_GlobalStack.Push(items[i]);
            }

            m_TotalItemsCount = m_MinPoolSize;
        }

        /// <summary>
        /// Push the specified item into the pool
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            m_GlobalStack.Push(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="waitTicks"></param>
        /// <returns></returns>
        private bool TryPopWithWait(out T item, int waitTicks)
        {
            var spinWait = new SpinWait();
            while (true)
            {
                spinWait.SpinOnce();//SpinOnce是很快的空转
                if (m_GlobalStack.TryPop(out item))
                {
                    return true;
                }

                if (spinWait.Count >= waitTicks)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Try to get one item from the pool
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryGet(out T item)
        {
            if (m_GlobalStack.TryPop(out item))
            {
                return true;
            }

            var currentSourceCount = m_CurrentSourceCount;

            if (currentSourceCount >= m_ItemsSource.Length)
            {
                return TryPopWithWait(out item,100);
            }

            int isIncreasing = m_IsIncreasing;

            if (isIncreasing == 1)
            {
                return TryPopWithWait(out item,100);
            }

            if (Interlocked.CompareExchange(ref m_IsIncreasing, 1, isIncreasing) != isIncreasing)
            {
                return TryPopWithWait(out item,100);
            }

            IncreaseCapacity();

            m_IsIncreasing = 0;

            if (!m_GlobalStack.TryPop(out item))
            {
                return false;
            }

            return true;
        }


        private void IncreaseCapacity()
        {
            var newItemsCount = Math.Min(m_TotalItemsCount, m_MaxPoolSize - m_TotalItemsCount);
            T[] items;
            m_ItemsSource[m_CurrentSourceCount++] = m_SourceCreator.Create(newItemsCount, out items);

            m_TotalItemsCount += newItemsCount;

            for (var i = 0; i < items.Length; i++)
            {
                m_GlobalStack.Push(items[i]);
            }
        }

        #endregion
    }

}
