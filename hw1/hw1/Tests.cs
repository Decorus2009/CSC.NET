using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace hw1
{
    [TestFixture]
    public class Tests
    {
        private ITrie Trie { get; set; }
        private List<string> Words { get; set; }

        [SetUp]
        public void Init()
        {
            Words = new List<string> {"Hello", "Hello, World!", "C#", "Java", "C++", "i", "in", "inn", "inner"};
            Trie = new Trie();
        }

        [Test]
        public void TestAddAllWords()
        {
            foreach (var word in Words)
            {
                Assert.True(Trie.Add(word));
            }
        }

        [Test]
        public void TestTrieContainsAllWords()
        {
            AddWords();
            foreach (var word in Words)
            {
                Assert.True(Trie.Contains(word));
            }
        }

        [Test]
        public void TestTrieSize()
        {
            AddWords();
            foreach (var word in Words)
            {
                Assert.AreEqual(Words.Count, Trie.Size());
            }
        }

        [Test]
        public void TestRemoveTreeWords()
        {
            AddWords();
            var removeList = new List<string> {"inn", "in", "inner", "i"};
            for (var i = 0; i < removeList.Count; ++i)
            {
                Assert.AreEqual(removeList.Count - i, Trie.HowManyStartsWithPrefix("i"));
                Assert.True(Trie.Remove(removeList[i]));
                Assert.False(Trie.Contains(removeList[i]));
                Assert.AreEqual(Words.Count - (i + 1), Trie.Size());
            }
        }

        private void AddWords() => Words.ToList().ForEach(word => Trie.Add(word));
    }
}