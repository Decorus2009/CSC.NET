using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Philosophers
{
    internal class Program
    {
        // example: 5 30 10 10 False
        public static void Main(string[] args)
        {
            if (args.Length != 5)
            {
                Console.Error.WriteLine(
                    "Usage:\napplication philosophers_number duration (sec) max_think_time (ms) max_eat_time (ms) debug");
                Environment.Exit(1);
            }
            try
            {
                var philosophersNumber = int.Parse(args[0]);
                var duration = int.Parse(args[1]);
                var maxThinkTime = int.Parse(args[2]);
                var maxEatTime = int.Parse(args[3]);
                var debug = bool.Parse(args[4]); // True / False

                var forks = InitForks(philosophersNumber);
                var philosophers = InitPhilosophers(forks, maxThinkTime, maxEatTime, debug);
                var threads = InitThreads(philosophers);

                threads.ForEach(t => t.Start());

                // дать поработать другим потокам для набора статистики 
                Console.WriteLine("All philosophers are now eating and thinking...");
                Thread.Sleep(duration * 1000);

                philosophers.ForEach(p => p.Stop());
                threads.ForEach(t => t.Join());
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
            int maxThinkTime, int maxEatTime, bool debug)
        {
            var n = forks.Count;
            var philosophers = new List<Philosopher>();
            for (var i = 0; i < n; i++)
            {
                philosophers.Add(new Philosopher(i + 1, forks[i], forks[(i + 1) / n], maxThinkTime, maxEatTime, debug));
            }
            return philosophers;
        }

        private static List<Thread> InitThreads(IEnumerable<Philosopher> philosophers) =>
            philosophers.Select(philosopher => new Thread(philosopher.Run)).ToList();
    }
}