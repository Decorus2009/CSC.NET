using System.Threading;

namespace LockFreeBlockingArrayQueue
{
    public class LockFreeQueue<T> : ILockFreeQueue<T>
    {
        private readonly T[] _storage;
        private readonly int _capacity;
        
        // откуда можно считать элемент
        private int _head; 
        // куда будет записан элемент
        private int _tail; 
        // индекс, указывающий на элемент до которого (не включая) можно честно читать
        // то, что находится правее может быть еще в процессе записи, читать там не надо 
        private int _maxReadIndex;

        public LockFreeQueue(int capacity)
        {
            // реализация содержит одну лишнюю пустую ячейку, 
            // которая помогает правильно сравнивать _head == (_tail + 1) % _capacity - очередь полностью заполнена. 
            // Изначально _head = _tail = 0
            _capacity = capacity + 1;
            _storage = new T[_capacity];
        }

        
        public void Enqueue(T element)
        {
            // локальные для потока!
            int currentTail;
            int currentHead;

            while (true)
            {
                currentTail = _tail;
                currentHead = _head;

                // если очередь полна, повторить
                if (currentHead == (currentTail + 1) % _capacity)
                {
                    continue;
                }

                // Если никто не вклинился и не отодвинул хвост еще дальше, 
                // можно его сдвинуть на 1 позицию правее и, 
                // таким образом, зарезервировать место под запись по индексу currentTail
                if (CAS(ref _tail, (currentTail + 1) % _capacity, currentTail))
                {
                    break;
                }
            }

            _storage[currentTail] = element;

            // _maxReadIndex может быть увеличен только в том потоке, в котором currentTail соответствует ячейке, 
            // следующей за последним надежно записанным элементом, фактическим хвостом (когда currentTail == _maxReadIndex). 
            // На _tail опираться нельзя, он может кучу раз сдвинут другими потоками вправо,
            // Поэтому тут сможет работать только один поток, остальные будут просто крутиться
            while (!CAS(ref _maxReadIndex, (currentTail + 1) % _capacity, currentTail))
            {
                // можно теперь отдохнуть, пусть другие пашут
                Thread.Yield();
            }
        }

        // крутится в user-space, пока не сможет вернуть что-то (т.к. blocking queue)
        public T Dequeue()
        {
            // локальные для потока!
            int currentHead;
            int currentMaxReadIndex;
            T currentResult;

            while (true)
            {
                currentHead = _head;
                currentMaxReadIndex = _maxReadIndex;

                // если очередь пуста, повторить
                if (currentHead == currentMaxReadIndex)
                {
                    continue;
                }
               
                // обращение к элементу массива атомарно?  
                currentResult = _storage[currentHead];

                // если голова не изменилась, то предыдущее присвоение (currentResult = _storage[currentHead]) валидно
                // и можно возвращаться
                if (CAS(ref _head, (currentHead + 1) % _capacity, currentHead))
                {
                    return currentResult;
                }
            }
        }

        public bool TryEnqueue(T element)
        {
            var currentTail = _tail;
            var currentHead = _head;
            
            // если очередь полна, конец
            if (currentHead == (currentTail + 1) % _capacity)
            {
                return false;
            }

            // попытка продивинуть хвост
            if (!CAS(ref _tail, (currentTail + 1) % _capacity, currentTail))
            {
                return false;
            }
            
            // ячейка с номером currentTail уже зарезервирована, 
            // к ней никто не сможет обратиться на чтение / запись из-за значений _tail и _maxReadIndex
            // так что, по идее, можно записывать
            _storage[currentTail] = element;
            
            // успех теперь определяется только результатом cas для _maxReadIndex. Если и его удалось продвинуть, то все ок
            return CAS(ref _maxReadIndex, (currentTail + 1) % _capacity, currentTail);
        }

        public bool TryDequeue(ref T element)
        {
            int currentHead;
            int currentMaxReadIndex;

            currentHead = _head;
            currentMaxReadIndex = _maxReadIndex;

            // если очередь пуста, конец
            if (currentHead == currentMaxReadIndex)
            {
                return false;
            }
               
            element = _storage[currentHead];

            // если голова не изменилась, то предыдущее присвоение (currentResult = _storage[currentHead]) валидно, все ок
            return CAS(ref _head, (currentHead + 1) % _capacity, currentHead); 
        }

        
        // Наверное, стоит полагать, что Size(), IsEmpty() и Clear() стоит запускать без конкуренции
        public int Size()
        {
            if (_tail >= _head)
            {
                return _tail - _head;
            }
            return _capacity - (_head - _tail); 
        }

        public bool IsEmpty()
        {
            return _head == _maxReadIndex;
        }

        // тут как-то непросто, _head, _tail и _maxReadIndex в ноль атомарно не установить без лока, 
        // запускать 3 cas'а - можно получить неконсистентное состояние, когда отработает только часть cas'ов.
        public void Clear()
        {
            _head = 0;
            _tail = 0;
            _maxReadIndex = 0;
        }


        private static bool CAS(ref int location, int newValue, int comparand)
        {
            // 1. если comparand и location равны, то другой поток не трогал значение location 
            // 2. location будет присвоен newValue
            // 3. Метод вернёт старое значение location независимо от присвоения
            // 4. comparand сравнится со старым значением location (т.е. oldLocation)
            // 5. если oldLocation(старый, возвращённый) не был изменён другим потоком то и CAS вернёт true, т.к. он совпадёт с comparand

            var oldLocation = Interlocked.CompareExchange(ref location, newValue, comparand);
            return comparand == oldLocation;
        }
    }
}