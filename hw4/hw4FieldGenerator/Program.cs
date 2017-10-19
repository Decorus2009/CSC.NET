using System;

namespace hw4FieldGenerator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: app n m file_for_generated_field");
                return;
            }
            int n;
            int m;
            try
            {
                n = int.Parse(args[0]);
                m = int.Parse(args[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot parse n or m");
                return;
            }

            var field = FieldGenerator.Generate(n, m);
            FieldGenerator.WriteTo(args[2], field);
        }
    }
}