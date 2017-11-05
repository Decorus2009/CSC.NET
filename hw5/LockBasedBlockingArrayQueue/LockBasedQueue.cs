﻿using System.Threading;

namespace LockBasedBlockingArrayQueue
{
    public class LockBasedQueue<T> : ILockBasedQueue<T>
    {
        private readonly T[] _storage;
        private readonly int _capacity;
        private int _head;
        private int _tail;
        private int _size; // atomic

        private readonly object _headLock;
        private readonly object _tailLock;
        private readonly AutoResetEvent _notEmpty;
        private readonly AutoResetEvent _notFull;


        public LockBasedQueue(int capacity)
        {
            _capacity = capacity;
            _storage = new T[_capacity];

            _headLock = new object();
            _tailLock = new object();
            _notEmpty = new AutoResetEvent(false);
            _notFull = new AutoResetEvent(false);
        }

        /// <summary>
        /// Enqueues element into queue. Blocks until can enqueue
        /// </summary>
        /// <param name="element">Element to enqueue</param>
        public void Enqueue(T element)
        {
            Monitor.Enter(_tailLock);
            try
            {
                // if queue is full, wait
                while (Interlocked.CompareExchange(ref _size, 0, 0) >= _capacity)
                {
                    _notFull.WaitOne();
                }

                PutElementAndSignalIfNotEmpty(element);
            }
            finally
            {
                Monitor.Exit(_tailLock);
            }
        }

        /// <summary>
        /// Dequeues element from queue. Blocks until can dequeue
        /// </summary>
        /// <returns>Dequeued element</returns>
        public T Dequeue()
        {
            T head;

            Monitor.Enter(_headLock);
            try
            {
                // if queue is empty, wait
                while (Interlocked.CompareExchange(ref _size, 0, 0) == 0)
                {
                    _notEmpty.WaitOne();
                }

                head = TakeElementAndSignalIfNotFull();
            }
            finally
            {
                Monitor.Exit(_headLock);
            }

            return head;
        }

        /// <summary>
        /// Tries to lock tail and put element without waiting
        /// </summary>
        /// <param name="element">Element to enqueue</param>
        /// <returns>True if can enqueue, else False</returns>
        public bool TryEnqueue(T element)
        {
            if (!Monitor.TryEnter(_tailLock)) return false;

            try
            {
                // can't enqueue and return false if queue is full
                if (Interlocked.CompareExchange(ref _size, 0, 0) >= _capacity) return false;

                PutElementAndSignalIfNotEmpty(element);
                return true;
            }
            finally
            {
                Monitor.Exit(_tailLock);
            }
        }

        /// <summary>
        /// Tries to lock head and take element without waiting
        /// </summary>
        /// <param name="element">Result of dequeue is put in this parameter</param>
        /// <returns>True if can dequeue, else False</returns>
        public bool TryDequeue(ref T element)
        {
            if (!Monitor.TryEnter(_headLock)) return false;

            try
            {
                // can't enqueue and return false if queue is full
                if (Interlocked.CompareExchange(ref _size, 0, 0) == 0) return false;

                element = TakeElementAndSignalIfNotFull();
                return true;
            }
            finally
            {
                Monitor.Exit(_headLock);
            }
        }

        public bool IsEmpty() => Interlocked.CompareExchange(ref _size, 0, 0) == 0;
        
        public int Size() => Interlocked.CompareExchange(ref _size, 0, 0);

        public void Clear()
        {
            Monitor.Enter(_headLock);
            try
            {
                Monitor.Enter(_tailLock);
                try
                {
                    _head = 0;
                    _tail = 0;
                    _size = 0;
                }
                finally
                {
                    Monitor.Exit(_tailLock);
                }
            }
            finally
            {
                Monitor.Exit(_headLock);
            }
        }


        private void PutElementAndSignalIfNotEmpty(T element)
        {
            _storage[_tail] = element;
            _tail = (_tail + 1) % _capacity;
            // increase size and check if queue is not empty, then signal
            if (Interlocked.Increment(ref _size) > 0)
            {
                _notEmpty.Set();
            }
        }

        private T TakeElementAndSignalIfNotFull()
        {
            var head = _storage[_head];
            _head = (_head + 1) % _capacity;
            // decrease size and check if queue is not full, then signal
            if (Interlocked.Decrement(ref _size) < _capacity)
            {
                _notFull.Set();
            }
            return head;
        }
    }
}