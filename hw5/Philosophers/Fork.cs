using System.Threading;

namespace Philosophers
{
    public class Fork
    {
        private readonly object _lock;

        public Fork()
        {
            _lock = new object();
        }

        public bool TryTake() => Monitor.TryEnter(_lock);

        public void Put()
        {
            if (Monitor.IsEntered(_lock))
            {
                Monitor.Exit(_lock);
            }   
        }
    }
}