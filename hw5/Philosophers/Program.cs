using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Philosophers
{
    internal class Program
    {
        // example: 5 30 10 10
        public static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.Error.WriteLine(
                    "Usage:\napplication philosophers_number duration (sec) max_think_time (ms) max_eat_time (ms)");
                Environment.Exit(1);
            }
            try
            {
                var philosophersNumber = int.Parse(args[0]);
                var duration = int.Parse(args[1]);
                var maxThinkTime = int.Parse(args[2]);
                var maxEatTime = int.Parse(args[3]);

                var forks = InitForks(philosophersNumber);
                var philosophers = InitPhilosophers(forks, maxThinkTime, maxEatTime);
                var tasks = InitTasks(philosophers);

                tasks.ForEach(t => t.Start());

                // дать поработать другим потокам для набора статистики 
                Console.WriteLine("All philosophers are now eating and thinking...");
                Thread.Sleep(duration * 1000);

                philosophers.ForEach(p => p.Stop());
                tasks.ForEach(t => t.Wait());
                philosophers.ForEach(p => p.PrintStatistics());
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Something went wrong while parsing program arguments");
                Console.Error.WriteLine(e);
                Environment.Exit(1);
            }
        }


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