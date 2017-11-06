namespace LockFreeBlockingArrayQueue
{
    public interface ILockFreeQueue<T>
    {
        void Enqueue(T element);

    }
}