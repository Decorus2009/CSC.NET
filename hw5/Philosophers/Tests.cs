using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Philosophers
{
    [TestFixture]
    public class Tests
    {
        private List<Philosopher> _philosophers;

        [OneTimeSetUp]
        public void ObtainStatistics()
        {
            const int philosophersNumber = 5;
            const int duration = 20;
            const int maxThinkTime = 10;
            const int maxEatTime = 10;

            var forks = InitForks(philosophersNumber);
            _philosophers = InitPhilosophers(forks, maxThinkTime, maxEatTime);
            var tasks = InitTasks(_philosophers);

            tasks.ForEach(t => t.Start());

            // дать поработать другим потокам для набора статистики 
            Thread.Sleep(duration * 1000);

            _philosophers.ForEach(p => p.Stop());
            tasks.ForEach(t => t.Wait());
            _philosophers.ForEach(p => p.PrintStatistics());
        }

        [Test]
        public void TestEachPhilosopherEatsAtLeastOneTime() => 
            _philosophers.ForEach(p => Assert.True(p.EatCount >= 1));

        [Test]
        public void TestEachPhilosopherEatCountDeviatesByNoMoreThan20PercentFromAverageValue()
        {
            var average = _philosophers.Select(p => p.EatCount).Average();
            _philosophers.ForEach(p => Assert.True(Math.Abs(average - p.EatCount) / average < 0.2));
        }

        [Test]
        public void TestEachPhilosopherTotalWaitTimeDeviatesByNoMoreThan20PercentFromAverageValue()
        {
            var average = _philosophers.Select(p => p.EatCount).Average();
            _philosophers.ForEach(p => Assert.True(Math.Abs(average - p.EatCount) / average < 0.2));
        }
        
        
        // Эти методы приватные в классе Program. Можно их, конечно, сделать публичными и не дублировать код
        private static List<Fork> InitForks(int n)
        {
            var forks = new List<Fork>();
            for (var i = 0; i < n; i++)
            {
                forks.Add(new Fork());
            }
            return forks;
        }

        private static List<Philosopher> InitPhilosophers(IReadOnlyList<Fork> forks,
            int maxThinkTime, int maxEatTime)
        {
            var n = forks.Count;
            var philosophers = new List<Philosopher>();
            for (var i = 0; i < n; i++)
            {
                philosophers.Add(new Philosopher(i + 1, forks[i], forks[(i + 1) % n], maxThinkTime, maxEatTime));
            }
            return philosophers;
        }

        private static List<Task> InitTasks(IEnumerable<Philosopher> philosophers) =>
            philosophers.Select(philosopher => new Task(philosopher.Run)).ToList();
    }
}