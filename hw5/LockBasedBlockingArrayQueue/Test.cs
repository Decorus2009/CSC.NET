using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace LockBasedBlockingArrayQueue
{
    public class Test
    {
        [Test]
        public void TestEmptyQueueIsEmpty()
        {
            const int n = 10;
            var emptyQueue = new LockBasedQueue<int>(n);
            Assert.That(emptyQueue.IsEmpty);
        }
        
        [Test]
        public void TestEmptyQueueHasSize0()
        {
            const int n = 10;
            var emptyQueue = new LockBasedQueue<int>(n);
            Assert.AreEqual(0, emptyQueue.Size());
        }

        [Test]
        public void TestQueueIsNotEmptyAfterEnqueueingElements()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);

            var producers = new List<Thread>(10);
            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                producers.Add(new Thread(() => queue.Enqueue(value)));
            }

            producers.ForEach(p => p.Start());
            producers.ForEach(p => p.Join());

            Assert.False(queue.IsEmpty());
        }

        [Test]
        public void TestQueueContainsEnqueuedElements()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);

            var elements = new List<int>();
            var producers = new List<Thread>(10);

            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                elements.Add(value);
                producers.Add(new Thread(() => queue.Enqueue(value)));
            }

            producers.ForEach(p => p.Start());
            producers.ForEach(p => p.Join());

            while (!queue.IsEmpty())
            {
                Assert.True(elements.Contains(queue.Dequeue()));
            }
        }

        [Test]
        public void TestQueueIsEmptyAfterDequeueingEnqueuedElements()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);

            var elements = new List<int>();
            var producers = new List<Thread>(10);

            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                elements.Add(value);
                producers.Add(new Thread(() => queue.Enqueue(value)));
            }

            producers.ForEach(p => p.Start());
            producers.ForEach(p => p.Join());

            var consumers = new List<Thread>(10);

            for (var i = 0; i < n; i++)
            {
                consumers.Add(new Thread(() => queue.Dequeue()));
            }

            consumers.ForEach(c => c.Start());
            consumers.ForEach(c => c.Join());

            Assert.True(queue.IsEmpty());
        }

        [Test]
        public void TestQueueRemainsEmptyAfterSameNumberOfSimultaneousEnqueuesAndDequeues()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);

            var elements = new List<int>();
            var producersAndConsumers = new List<Thread>(10);

            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                elements.Add(value);
                producersAndConsumers.Add(new Thread(() => queue.Enqueue(value)));
                producersAndConsumers.Add(new Thread(() => queue.Dequeue()));
            }

            producersAndConsumers.ForEach(p => p.Start());
            producersAndConsumers.ForEach(p => p.Join());

            Assert.True(queue.IsEmpty());
        }
        
        [Test]
        public void TestTryEnqueueIsSuccessfulOnNotFullQueue()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);
            Assert.True(queue.TryEnqueue(1));
        }
        
        [Test]
        public void TestTryDequeueIsSuccessfulOnNotEmptyQueue()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);
            queue.Enqueue(1);
            var value = 0;
            Assert.True(queue.TryDequeue(ref value));
        }
        
        [Test]
        public void TestTryDequeueDequeuesEnqueuedElement()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);
            
            const int element = 1;
            queue.Enqueue(element);
            
            var dequeuedElement = 0;
            queue.TryDequeue(ref dequeuedElement);
            Assert.AreEqual(element, dequeuedElement);
        }

        [Test]
        public void TestTryEnqueueOnFullQueueFails()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);

            var producers = new List<Thread>(10);
            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                producers.Add(new Thread(() => queue.Enqueue(value)));
            }

            producers.ForEach(p => p.Start());
            producers.ForEach(p => p.Join());

            Assert.False(queue.TryEnqueue(11));
        }

        [Test]
        public void TestTryDequeueOnEmptyQueueFails()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);

            var value = 0;
            Assert.False(queue.TryDequeue(ref value));
        }

        [Test]
        public void TestTryEnqueueOnBlockedQueueFails()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);

            var producers = new List<Thread>(10);
            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                producers.Add(new Thread(() => queue.Enqueue(value)));
            }

            producers.ForEach(p => p.Start());
            producers.ForEach(p => p.Join());

            // blocks queue waiting for a chance to put element
            var extraProducer = new Thread(() => queue.Enqueue(11)) {IsBackground = true};
            extraProducer.Start();

            Assert.False(queue.TryEnqueue(12));
        }

        [Test]
        public void TestTryDequeueOnBlockedQueueFails()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);

            // blocks queue waiting for a chance take element
            var consumer = new Thread(() => queue.Dequeue()) {IsBackground = true};
            consumer.Start();

            var value = 0;
            Assert.False(queue.TryDequeue(ref value));
        }

        [Test]
        public void TestClearClearesQueue()
        {
            const int n = 10;
            var queue = new LockBasedQueue<int>(n);

            var producers = new List<Thread>(10);
            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                producers.Add(new Thread(() => queue.Enqueue(value)));
            }

            producers.ForEach(p => p.Start());
            producers.ForEach(p => p.Join());

            queue.Clear();
            Assert.That(queue.IsEmpty);
        }
    }
}