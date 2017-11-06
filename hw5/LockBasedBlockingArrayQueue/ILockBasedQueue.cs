﻿namespace LockBasedBlockingArrayQueue
{
    public interface ILockBasedQueue<T>
    {
        void Enqueue(T element);

        T Dequeue();

        bool TryEnqueue(T element);

        bool TryDequeue(ref T element);

        int Size();

        bool IsEmpty();
        
        void Clear();
    }
}