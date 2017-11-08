using System.Threading;

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


        // блокируется до тех пор, пока не сможет вставить элемент 
        public void Enqueue(T element)
        {
            Monitor.Enter(_tailLock);
            try
            {
                // если очередь полна, ждать
                while (_size >= _capacity)
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

        // блокируется до тех пор, пока не сможет взять элемент 
        public T Dequeue()
        {
            T head;

            Monitor.Enter(_headLock);
            try
            {
                // если очередь пуста, ждать
                while (_size == 0)
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

        // пытается вставить элемент без блокировки
        public bool TryEnqueue(T element)
        {
            if (!Monitor.TryEnter(_tailLock))
            {
                return false;
            }
            
            try
            {
                if (_size >= _capacity)
                {
                    return false;
                }

                PutElementAndSignalIfNotEmpty(element);
                return true;
            }
            finally
            {
                Monitor.Exit(_tailLock);
            }
        }

        // пытается взять элемент без блокировки
        public bool TryDequeue(ref T element)
        {
            if (!Monitor.TryEnter(_headLock))
            {
                return false;
            }

            try
            {
                if (_size == 0)
                {
                    return false;
                }

                element = TakeElementAndSignalIfNotFull();
                return true;
            }
            finally
            {
                Monitor.Exit(_headLock);
            }
        }

        public int Size() => _size;
        
        public bool IsEmpty() => Size() == 0;
        
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
            // увеличить размер и проверить, что очередь не пуста, тогда просигналить
            if (Interlocked.Increment(ref _size) > 0)
            {
                _notEmpty.Set();
            }
        }

        private T TakeElementAndSignalIfNotFull()
        {
            var head = _storage[_head];
            _head = (_head + 1) % _capacity;
            // уменьшить размер и проверить, что очередь не полна, тогда просигналить
            if (Interlocked.Decrement(ref _size) < _capacity)
            {
                _notFull.Set();
            }
            return head;
        }
    }
}