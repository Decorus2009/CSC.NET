using System;
using System.Collections.Generic;
using System.Linq;

namespace FieldGenerator
{
    public class FieldGenerator
    {
        private static readonly Random Generator = new Random();

        public static List<List<char>> Generate(int n, int m)
        {
            var field = new List<List<char>>();
            for (var i = 0; i < n; ++i)
            {
                var row = new List<char>();
                for (var j = 0; j < m; ++j)
                {
                    row.Add(ChanceToBuildWall() ? '*' : '.');
                }
                field.Add(row);
            }
            field[0][0] = '.';
            return field;
        }

        public static void WriteTo(string file, List<List<char>> field) =>
            System.IO.File.WriteAllLines(file, field.Select(row => string.Join("", row)));

        private static bool ChanceToBuildWall() => Generator.Next(100) >= 75;
    }
}