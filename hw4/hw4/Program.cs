using System;

namespace hw4
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: app field_file");
                return;
            }
            var game = new Game(args[0]);
            game.MoveHandler += Printer.PrintHero;
            
            var eventLoop = new EventLoop();
            eventLoop.UpHandler += game.OnUp;
            eventLoop.DownHandler += game.OnDown;
            eventLoop.LeftHandler += game.OnLeft;
            eventLoop.RightHandler += game.OnRight;
            
            Printer.PrintField(game);
            eventLoop.Run();
            Printer.Clear();
        }
    }
}