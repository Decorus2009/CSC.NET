namespace LockBasedBlockingArrayQueue
{
    public interface ILockBasedQueue<T>
    {
        void Enqueue(T element);

        T Dequeue();

        bool TryEnqueue(T element);

        bool TryDequeue(ref T element);

        bool IsEmpty();

        int Size();
        
        void Clear();
    }
}