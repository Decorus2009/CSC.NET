using System;
using System.Diagnostics;
using System.Threading;

namespace Philosophers
{
    public class Philosopher
    {
        private readonly Fork _left;
        private readonly Fork _right;
        private readonly int _id;
        private volatile bool _stopFlag; // check for happens before https://habrahabr.ru/post/133981/
        private readonly Random _random;

        // milliseconds
        private readonly int _maxThinkTime;
        private readonly int _maxEatTime;
        private const int SleepTime = 10; // эмпирическое значение

        private Stopwatch _stopwatch;
        private long _eatCount;
        private long _totalWaitTime;

        private readonly bool _debug;

        public Philosopher(int id, Fork left, Fork right, int maxThinkTime, int maxEatTime, bool debug)
        {
            _left = left;
            _right = right;
            _id = id;
            _stopFlag = false;
            _random = new Random();

            _maxThinkTime = maxThinkTime;
            _maxEatTime = maxEatTime;
            _debug = debug;
            _eatCount = 0;
            _totalWaitTime = 0;

            _stopwatch = new Stopwatch();
        }

        public void Run()
        {
            while (!_stopFlag)
            {
                Think();

                while (true)
                {
                    if (!_left.TryTake())
                    {
                        SleepUpTo(SleepTime);
                        continue;
                    }
                    if (_debug) Console.WriteLine("{0} has taken left fork", _id);

                    if (!_right.TryTake())
                    {
                        _left.Put();
                        if (_debug) Console.WriteLine("{0} has put left fork", _id);
                        SleepUpTo(SleepTime);
                        continue;
                    }
                    if (_debug) Console.WriteLine("{0} has taken right fork", _id);
                    break;
                }

                if (_debug) Console.WriteLine("{0} is eating...", _id);
                Eat();

                _right.Put();
                if (_debug) Console.WriteLine("{0} has put right fork", _id);
                _left.Put();
                if (_debug) Console.WriteLine("{0} has put left fork", _id);
                SleepUpTo(SleepTime);
            }
        }

        public void Stop() => _stopFlag = true;

        public void PrintStatistics() =>
            Console.WriteLine("Philosopher id: {0}, total eatings: {1}, total wait time: {2}",
                _id, _eatCount, _totalWaitTime);

        
        private void Think()
        {
            if (_debug) Console.WriteLine("{0} is thinking...", _id);
            SleepUpTo(_maxThinkTime);
            
            _stopwatch = Stopwatch.StartNew();
        }

        private void Eat()
        {
            _stopwatch.Stop();
            _totalWaitTime += _stopwatch.ElapsedMilliseconds;

            if (_debug) Console.WriteLine("{0} is eating...", _id);
            SleepUpTo(_maxEatTime);
            ++_eatCount;
        }

        private void SleepUpTo(int time) => Thread.Sleep(_random.Next(time));
    }
}