using static System.Console;
using System.Collections.Generic;

namespace hw1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // some simple tests
            var trie = new Trie();
            var words = new List<string> {"Hello", "Hello, World!", "C#", "Java", "C++", "i", "in", "inn", "inner"};
            foreach (var word in words)
            {
                WriteLine($"Adding \"{word}\": {trie.Add(word)}");
                WriteLine($"Contains \"{word}\"?: {trie.Contains("Hello")}\n");                
            }

            
            var removeList = new List<string> {"inn", "in", "inner", "i"};
            foreach (var removeWord in removeList)
            {
                WriteLine($"Removing \"{removeWord}\": {trie.Remove(removeWord)}");   
                WriteLine($"Trie size: {trie.Size()}");
            }
            
            foreach (var removeWord in removeList)
            {
                WriteLine($"Contains \"{removeWord}\"?: {trie.Contains(removeWord)}\n");                
            }
        }
    }
}