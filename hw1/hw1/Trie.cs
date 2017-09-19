using System;
using System.Collections.Generic;

namespace hw1
{
    public class Trie
    {
        private class TrieNode
        {
            private readonly Dictionary<char, TrieNode> _children = new Dictionary<char, TrieNode>();

            internal TrieNode Parent { get; private set; }
            internal bool IsTerminal { get; set; }
            internal int TraversalNumber { get; set; }

            internal bool HasChildBy(char c) => _children.ContainsKey(c);

            internal void AddChildBy(char c) => _children[c] = new TrieNode {Parent = this};

            internal bool HasChildren() => _children.Count != 0;

            internal TrieNode NextChildBy(char c) => _children[c];

            // Should I make deleted nodes null to help GC? There're no more references to each deleted node
            public void RemoveChildBy(char c)
            {
                var child = _children[c];
                // remove node
                child = null;
                // remove edge
                _children.Remove(c);
            }
//             public void RemoveChildBy(char c) => _children.Remove(c);
        }

        private class TrieNodeException : Exception
        {
        }
        private readonly TrieNode _root = new TrieNode();
        private int _size;

        /// Expected complexity: O(|element|)
        /// Returns true if this set did not already contain the specified element
        public bool Add(string element)
        {
            if (Contains(element))
            {
                return false;
            }

            var curNode = _root;
            foreach (var c in element)
            {
                if (!curNode.HasChildBy(c))
                {
                    curNode.AddChildBy(c);
                }
                // move to the next node
                curNode = curNode.NextChildBy(c);
                curNode.TraversalNumber++;
            }
            curNode.IsTerminal = true;
            _size++;
            return true;
        }

        /// Expected complexity: O(|element|)
        public bool Contains(string element)
        {
            try
            {
                // last found node must be terminal (otherwise it might be some intermediate node in another word)
                return FindLastNodeFor(element).IsTerminal;
            }
            catch (TrieNodeException e)
            {
                return false;
            }
        }

        /// Expected complexity: O(|element|)
        /// Returns true if this set contained the specified element
        public bool Remove(string element)
        {
            try
            {
                var curNode = FindLastNodeFor(element);
                if (!curNode.IsTerminal)
                {
                    return false;
                }
                // in the terminal node now...

                curNode.IsTerminal = false;
                if (!curNode.HasChildren())
                {
                    // remove nodes until: 
                    //     another terminal node of another shorter word is found, 
                    //     the root node is reached.
                    for (var i = element.Length - 1; i >= 0 && !curNode.IsTerminal; i--)
                    {
                        var c = element[i];
                        curNode = curNode.Parent;
                        curNode.RemoveChildBy(c);
                    }
                }
                // decrease traversal number for the remaining nodes
                while (curNode != _root)
                {
                    curNode.TraversalNumber--;
                    curNode = curNode.Parent;
                }
                _size--;

                return true;
            }
            catch (TrieNodeException e)
            {
                return false;
            }
        }

        /// Expected complexity: O(1)
        public int Size() => _size;

        /// Expected complexity: O(|prefix|)
        /// suppose that prefix is contained in the Trie
        public int HowManyStartsWithPrefix(string prefix)
        {
            try
            {
                return FindLastNodeFor(prefix).TraversalNumber;
            }
            catch (TrieNodeException e)
            {
                return 0;
            }
        }
        

        private TrieNode FindLastNodeFor(string element)
        {
            var curNode = _root;
            foreach (var c in element)
            {
                if (!curNode.HasChildBy(c))
                {
                    throw new TrieNodeException();
                }
                curNode = curNode.NextChildBy(c);
            }
            return curNode;
        }
    }
}