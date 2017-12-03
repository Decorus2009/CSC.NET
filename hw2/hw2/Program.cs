using System;

namespace Hw2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Usage:\napplication assembiles_path");
                /* так можно завершать приложение с кодом ошибки? */
                Environment.Exit(1);
            }
            var path = args[0]; 
            var testGroups = TestDiscoverer.DiscoverIn(path);
            TestRunner.Run(testGroups);
        }
    }
}
