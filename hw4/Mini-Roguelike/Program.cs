using System;

namespace Mini_Roguelike
{
    internal class Program
    {
        /*
        Терминал в райдере как-то непонятно работает, поэтому запуск программы проводил в обычном терминале:
        cd <путь к папке с решением>/Mini-Roguelike/obj/Debug
        mono Mini_Roguelike.exe <путь к папке с решением>/Mini-Roguelike/field/generated_field.txt
        */
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