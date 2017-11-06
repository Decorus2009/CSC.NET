using System.Threading;

namespace LockFreeBlockingArrayQueue
{
    public class LockFreeQueue<T> : ILockFreeQueue<T>
    {
        private readonly T[] _storage;
        private readonly int _capacity;
        private int _head; // откуда можно считать элемент
        private int _tail; // куда будет записан элемент

        private int _size;

        // индекс последнего элемента, который можно читать, 
        // то, что находится правее может быть еще в процессе записи, читать там не надо 
        private int _maxReadIndex;

        public LockFreeQueue(int capacity)
        {
            _capacity = capacity;
            _storage = new T[capacity];
        }

        public void Enqueue(T element)
        {
            int currentTail;
            int currentHead;

            while (true)
            {
                currentTail = _tail;
                currentHead = _head;

                // Если очередь полна, повторить
                if (currentHead == currentTail) continue;

                // Если никто не вклинился и не отодвинул хвост еще дальше, 
                // можно его сдвинуть на 1 позицию правее и, 
                // таким образом, зарезервировать место под запись по индексу currentTail
                if (CAS(ref _tail, (currentTail + 1) % _capacity, currentTail))
                {
                    break;
                }
            }


            // TODO как я могу гарантировать, что тут никто не влезет и не запишет кучу элементов, 
            // TODO а потом еще и не считает вместе с тем, что я не успел здесь записать?
            // TODO Тут же можно нарушить консистентность
            _storage[currentTail] = element;
            // TODO аналогично


            // Этот код должен быть выполнен строго после первого CAS.
            // Если есть только один писатель, то ок. Но если писателей несколько?  
            while (!CAS(ref _maxReadIndex, currentTail + 1, currentTail))
            {
            }
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