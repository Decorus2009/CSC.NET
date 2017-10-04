using System;
using System.Collections.Generic;
using System.Linq;

namespace hw1
{
    public class Trie : ITrie
    {
        private class TrieNode
        {
            private readonly Dictionary<char, TrieNode> _children;

            public TrieNode()
            {
                _children = new Dictionary<char, TrieNode>();
            }

            public TrieNode Parent { get; private set; }
            public bool IsTerminal { get; set; }
            public int TraversalNumber { get; set; }

            public bool HasChildBy(char c) => _children.ContainsKey(c);

            public void AddChildBy(char c) => _children[c] = new TrieNode {Parent = this};

            public bool HasChildren() => _children.Any();

            public TrieNode NextChildBy(char c) => _children[c];

            public void RemoveChildBy(char c) => _children.Remove(c);
        }

        private readonly TrieNode _root = new TrieNode();
        private int _size;


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
            ++_size;
            return true;
        }

        // last found node must be terminal (otherwise it might be some intermediate node in another word)
        public bool Contains(string element) => FindLastNodeFor(element)?.IsTerminal ?? false;

        public bool Remove(string element)
        {
            var curNode = FindLastNodeFor(element);
            if (curNode == null || !curNode.IsTerminal)
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
                foreach (var c in element.Reverse())
                {
                    if (curNode.IsTerminal)
                    {
                        break;
                    }
                    curNode = curNode.Parent;
                    curNode.RemoveChildBy(c);
                }
            }
            // decrease traversal number for remaining nodes
            while (curNode != _root)
            {
                curNode.TraversalNumber--;
                curNode = curNode.Parent;
            }
            --_size;
            return true;
        }

        public int Size() => _size;

        public int HowManyStartsWithPrefix(string prefix) => FindLastNodeFor(prefix)?.TraversalNumber ?? 0;


        private TrieNode FindLastNodeFor(string element)
        {
            var curNode = _root;
            foreach (var c in element)
            {
                if (!curNode.HasChildBy(c))
                {
                    return null;
                }
                curNode = curNode.NextChildBy(c);
            }
            return curNode;
        }
    }
}