using System;
using System.IO;
using System.Text;

namespace FieldGenerator
{
    public class FieldGenerator
    {
        private static readonly Random Generator;

        static FieldGenerator()
        {
            Generator = new Random();
        }

        public static char[,] Generate(int n, int m)
        {
            var field = new char[n, m];
            for (var i = 0; i < n; ++i)
            {
                for (var j = 0; j < m; ++j)
                {
                    field[i, j] = ChanceToBuildWall() ? '*' : '.';
                }
            }
            field[0, 0] = '.';
            return field;
        }

        public static void WriteTo(string file, char[,] field)
        {
            var result = new StringBuilder();
            for (var i = 0; i < field.GetLength(0); i++)
            {
                for (var j = 0; j < field.GetLength(1); j++)
                {
                    result.Append(field[i, j]);
                }
                result.Append(Environment.NewLine);
            }
            
            File.WriteAllText(file, result.ToString());
        }

        private static bool ChanceToBuildWall() => Generator.Next(100) >= 75;
    }
}