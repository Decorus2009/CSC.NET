using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BlockingArrayQueue
{
    public class Tests
    {
        private static object[] _queueCases =
        {
            new LockBasedQueue<int>(10),
            new LockFreeQueue<int>(10)
        };


        [TestCaseSource(nameof(_queueCases))]
        public void TestEmptyQueueIsEmpty(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            Assert.That(queue.IsEmpty);
        }
        
        [TestCaseSource(nameof(_queueCases))]
        public void TestEmptyQueueHasSize0(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            Assert.AreEqual(0, queue.Size());
        }

        [TestCaseSource(nameof(_queueCases))]
        public void TestQueueIsNotEmptyAfterEnqueueingElements(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            const int n = 10;
            
            var producers = new List<Task>(n);
            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                producers.Add(new Task(() => queue.Enqueue(value)));
            }
            
            producers.ForEach(p => p.Start());
            producers.ForEach(p => p.Wait());
            
            Assert.False(queue.IsEmpty());
        }

        [TestCaseSource(nameof(_queueCases))]
        public void TestQueueContainsEnqueuedElements(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            const int n = 10;
            
            var elements = new List<int>();
            var producers = new List<Task>(n);

            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                elements.Add(value);
                producers.Add(new Task(() => queue.Enqueue(value)));
            }

            producers.ForEach(p => p.Start());
            producers.ForEach(p => p.Wait());

            while (!queue.IsEmpty())
            {
                Assert.True(elements.Contains(queue.Dequeue()));
            }
        }

        [TestCaseSource(nameof(_queueCases))]
        public void TestQueueIsEmptyAfterDequeueingEnqueuedElements(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            const int n = 10;

            var elements = new List<int>();
            var producers = new List<Task>(n);

            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                elements.Add(value);
                producers.Add(new Task(() => queue.Enqueue(value)));
            }

            producers.ForEach(p => p.Start());
            producers.ForEach(p => p.Wait());

            var consumers = new List<Task>(10);

            for (var i = 0; i < n; i++)
            {
                consumers.Add(new Task(() => queue.Dequeue()));
            }

            consumers.ForEach(c => c.Start());
            consumers.ForEach(c => c.Wait());

            Assert.True(queue.IsEmpty());
        }

        [TestCaseSource(nameof(_queueCases))]
        public void TestQueueRemainsEmptyAfterSameNumberOfSimultaneousEnqueuesAndDequeues(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            const int n = 10;

            var elements = new List<int>();
            var producersAndConsumers = new List<Task>(n);

            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                elements.Add(value);
                producersAndConsumers.Add(new Task(() => queue.Enqueue(value)));
                producersAndConsumers.Add(new Task(() => queue.Dequeue()));
            }

            producersAndConsumers.ForEach(p => p.Start());
            producersAndConsumers.ForEach(p => p.Wait());

            Assert.True(queue.IsEmpty());
        }
        
        [TestCaseSource(nameof(_queueCases))]
        public void TestTryEnqueueIsSuccessfulOnNotFullQueue(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            Assert.True(queue.TryEnqueue(1));
        }
        
        [TestCaseSource(nameof(_queueCases))]
        public void TestTryDequeueIsSuccessfulOnNotEmptyQueue(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            queue.Enqueue(1);
            var value = 0;
            Assert.True(queue.TryDequeue(ref value));
        }
        
        [TestCaseSource(nameof(_queueCases))]
        public void TestTryDequeueDequeuesEnqueuedElement(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            const int element = 1;
            queue.Enqueue(element);
            
            var dequeuedElement = 0;
            queue.TryDequeue(ref dequeuedElement);
            Assert.AreEqual(element, dequeuedElement);
        }

        [TestCaseSource(nameof(_queueCases))]
        public void TestTryEnqueueOnFullQueueFails(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            const int n = 10;

            var producers = new List<Task>(10);
            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                producers.Add(new Task(() => queue.Enqueue(value)));
            }

            producers.ForEach(p => p.Start());
            producers.ForEach(p => p.Wait());

            Assert.False(queue.TryEnqueue(11));
        }

        [TestCaseSource(nameof(_queueCases))]
        public void TestTryDequeueOnEmptyQueueFails(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            var value = 0;
            Assert.False(queue.TryDequeue(ref value));
        }

        [TestCaseSource(nameof(_queueCases))]
        public void TestClearClearesQueue(IBlockingArrayQueue<int> queue)
        {
            queue.Clear();
            
            const int n = 10;

            var producers = new List<Task>(10);
            for (var i = 0; i < n; i++)
            {
                var value = i + 1;
                producers.Add(new Task(() => queue.Enqueue(value)));
            }

            producers.ForEach(p => p.Start());
            producers.ForEach(p => p.Wait());

            queue.Clear();
            Assert.That(queue.IsEmpty);
        }
    }
}