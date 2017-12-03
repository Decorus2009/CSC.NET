using System;

namespace Mini_Roguelike
{
    public class Printer
    {
        public static void PrintField(Game game)
        {
            Clear();
            foreach (var row in game.Field)
            {
                foreach (var cell in row)
                {
                    switch (cell)
                    {
                        case Cell.Free:
                            Console.Write('.');
                            break;
                        case Cell.Wall:
                            Console.Write('*');
                            break;
                        default:
                            throw new ArgumentException(
                                "Only walls and corridors are supported in current version");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine(Environment.NewLine + "Press ESC to exit");
            Console.SetCursorPosition(game.Current.x, game.Current.y);
            Console.Write('@');
        }

        public static void PrintHero(Game game)
        {
            WriteAt(game.Previous.x, game.Previous.y, game.CharUnderPreviousPosition());
            WriteAt(game.Current.x, game.Current.y, '@');
        }

        public static void Clear() => Console.Clear();


        private static void WriteAt(int x, int y, char symbol)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(symbol);
        }
    }
}