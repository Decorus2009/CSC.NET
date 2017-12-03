using System;

namespace Mini_Roguelike
{
    public class EventLoop
    {
        public event Action UpHandler;
        public event Action DownHandler;
        public event Action LeftHandler;
        public event Action RightHandler;

        public void Run()
        {
            while (true)
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        UpHandler?.Invoke();
                        break;
                    case ConsoleKey.DownArrow:
                        DownHandler?.Invoke();
                        break;
                    case ConsoleKey.LeftArrow:
                        LeftHandler?.Invoke();
                        break;
                    case ConsoleKey.RightArrow:
                        RightHandler?.Invoke();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}