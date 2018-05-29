using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.HashFunction;

namespace CoreDefinitions.Helpers
{
    public enum Branch
    {
        Root,
        Right,
        Left,
    }

    public class BinaryTree<T, K>
    {
        private T _key;
        private K _value;
        private int balanceFactor;
        public T Key
        {
            get
            {
                return _key;
            }

            set
            {
                _key = value;
            }
        }
        public K Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public int Balance
        {
            get
            {
                return balanceFactor;
            }

            set
            {
                balanceFactor = value;
            }
        }
        public BinaryTree(T key, K value)
        {
            _key = key;
            _value = value;
        }

        public BinaryTree<T, K> Left;
        public BinaryTree<T, K> Right;

        public string getTreeView(bool markUnused, bool toListBox = true)
        {
            StringBuilder output = new StringBuilder();
            getNode(output, 0, markUnused, toListBox);
            return output.ToString();
        }

        private void getNode(StringBuilder output, int depth, bool markUnused, bool toListBox)
        {
            if (toListBox)
            {
                if (depth > 7)
                {
                    output.Append("***");
                    return;
                }
            }

            if (Right != null)
            {
                Right.getNode(output, depth + 1, markUnused, toListBox);
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }

            if (this.Key == null)
            {
                if (markUnused)
                {
                    output.Append('\t', depth);
                    output.AppendLine("X");
                }
            }
            else
            {
                output.Append('\t', depth);
                output.AppendLine(_key.ToString());
            }


            if (Left != null)
            {
                Left.getNode(output, depth + 1, markUnused, toListBox);
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }
        }

        public string getTreeViewPlus(bool markUnused, bool toListBox = true)
        {
            StringBuilder output = new StringBuilder();
            getNodePlus(output, 0, markUnused, toListBox);
            return output.ToString();
        }

        private void getNodePlus(StringBuilder output, int depth, bool markUnused, bool toListBox)
        {
            if (toListBox)
            {
                if (depth > 7)
                {
                    output.Append("***");
                    return;
                }
            }

            if (Right != null)
            {
                Right.getNodePlus(output, depth + 1, markUnused, toListBox);
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }

            if (this.Key == null)
            {
                if (markUnused)
                {
                    output.Append('\t', depth);
                    output.AppendLine("X");
                }
            }
            else
            {
                output.Append('\t', depth);
                var textToShow = Key.ToString() + "(" + Balance + ")";
                output.AppendLine(textToShow);
            }


            if (Left != null)
            {
                Left.getNodePlus(output, depth + 1, markUnused, toListBox);
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }
        }


        public override string ToString()
        {
            return _key.ToString() + " - " + GetAdressOfObject();
        }

        private IntPtr GetAdressOfObject()
        {
            IntPtr ptr = IntPtr.Zero;
            var xx = this;
            unsafe
            {
                TypedReference tr = __makeref(xx);
                ptr = **(IntPtr**)(&tr);
            }
            return ptr;
        }

        public BinaryTree<T, K> GetByA(int a)
        {
            if (a == -1)
            {
                return this.Left;
            }
            if (a == 1)
            {
                return this.Right;
            }
            throw new Exception("Huh?!");
        }

        public void SetByA(int a, BinaryTree<T, K> node)
        {
            if (a == -1)
            {
                this.Left = node;
            }
            else if (a == 1)
            {
                this.Right = node;
            }
            else
            {
                throw new Exception("Huh?!");
            }
        }
    }

    public static class BinaryTreeHelpers
    {
        public static BinaryTree<T, K> NewNode<T, K>(this BinaryTree<T, K> source, T key, K val)
        {
            return new BinaryTree<T, K>(key, val);
        }

        public static BinaryTree<T, K> Node<T, K>(this BinaryTree<T, K> source)
        {
            return source;
        }

        public static T Key<T, K>(this BinaryTree<T, K> source)
        {
            return source.Key;
        }

        public static BinaryTree<T, K> LLink<T, K>(this BinaryTree<T, K> source)
        {
            return source.Left;
        }

        public static BinaryTree<T, K> RLink<T, K>(this BinaryTree<T, K> source)
        {
            return source.Right;
        }
    }

    //====================================================================//

    public static class TrieHelper
    {
        public static Tuple<string, char, char> GetCommonPart(string word1, string word2)
        {
            string common = "";
            string difference1 = "";
            string difference2 = "";
            int index = 0;
            bool same = true;

            do
            {
                if (word1[index] == word2[index])
                {
                    common += word1[index];
                    ++index;
                }
                else
                {
                    same = false;
                }

            } while (same && index < word1.Length && index < word2.Length);

            for (int i = index; i < word1.Length; i++)
            {
                difference1 += word1[i];
            }

            for (int i = index; i < word2.Length; i++)
            {
                difference2 += word2[i];
            }

            return new Tuple<string, char, char>(common, difference1.Length > 0 ? difference1[0] : '^', difference2.Length > 0 ? difference2[0] : '^');
        }
    }

    public class oTrieLeafNode
    {
        public string _prefix = "";
        public int _depth;
        public Dictionary<char, oTrieNode> _nodesCollection;

        public oTrieLeafNode(string prefix, int depth)
        {
            _nodesCollection = new Dictionary<char, oTrieNode>();
            _prefix = prefix;
            _depth = depth;
        }

        public void getNode(StringBuilder output, int depth, bool markUnused, bool toListBox)
        {
            if (toListBox)
            {
                if (depth > 7)
                {
                    output.Append("***");
                    return;
                }
            }

            if (_nodesCollection.Count > 0)
            {
                var left = _nodesCollection.Take((int)Math.Floor(_nodesCollection.Count / 2d));
                foreach (var item in left)
                {
                    item.Value.getNode(output, depth + 1, markUnused, toListBox);
                }
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }

            if (this._prefix == null)
            {
                if (markUnused)
                {
                    output.Append('\t', depth);
                    output.AppendLine("[ X ]");
                }
            }
            else
            {
                output.Append('\t', depth);
                output.AppendLine("[ " + _prefix + " ]");
            }

            if (_nodesCollection.Count > 0)
            {
                var rignt = _nodesCollection.Skip((int)Math.Floor(_nodesCollection.Count / 2d)).Take(_nodesCollection.Count);
                foreach (var item in rignt)
                {
                    item.Value.getNode(output, depth + 1, markUnused, toListBox);
                }
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }
        }
    }

    public class oTrieNode
    {
        public char _letter;
        public string _singleWord;
        public List<oTrieLeafNode> _leafNodeCollection;

        public oTrieNode(char letter, string key)
        {
            _letter = letter;
            _singleWord = key;
            _leafNodeCollection = new List<oTrieLeafNode>();
        }

        public bool AddWord(ref string word, int pos)
        {
            if (pos > word.Length - 1)
            {
                return true;
            }

            try
            {
                var searchChar = word[pos];

                //link to next branch exists
                if (_singleWord == null)
                {
                    var copyOfWord = word;
                    var mimeKeys = _leafNodeCollection.Select(x => TrieHelper.GetCommonPart(copyOfWord, x._prefix)).OrderByDescending(x => x.Item1.Length);
                    var suitableCandidate = mimeKeys.FirstOrDefault();
                    if (suitableCandidate != null)
                    {
                        var node = _leafNodeCollection.Where(x => x._prefix == suitableCandidate.Item1).FirstOrDefault();

                        if (node != null)
                        {
                            if (node._nodesCollection.ContainsKey(suitableCandidate.Item2))
                            {
                                node._nodesCollection[suitableCandidate.Item2].AddWord(ref word, suitableCandidate.Item1.Length);
                            }
                            else
                            {
                                node._nodesCollection.Add(suitableCandidate.Item2, new oTrieNode(suitableCandidate.Item2, copyOfWord));
                            }
                        }
                        else
                        {
                            var newLeaf = new oTrieLeafNode(suitableCandidate.Item1, pos);
                            newLeaf._nodesCollection.Add(suitableCandidate.Item2, new oTrieNode(suitableCandidate.Item2, copyOfWord));
                            _leafNodeCollection.Add(newLeaf);
                        }
                        return true;
                    }
                    else
                    {
                        var newLeaf = new oTrieLeafNode(suitableCandidate.Item1, pos);
                        newLeaf._nodesCollection.Add(suitableCandidate.Item2, new oTrieNode(suitableCandidate.Item2, copyOfWord));
                        _leafNodeCollection.Add(newLeaf);
                        return true;
                    }
                }
                else // have link to other node
                {
                    //still free
                    if (_singleWord == "")
                    {
                        _letter = searchChar;
                        _singleWord = word;
                    }
                    else // convert to collection
                    {
                        var key = TrieHelper.GetCommonPart(_singleWord, word);

                        var newLeaf = new oTrieLeafNode(key.Item1, pos);
                        newLeaf._nodesCollection.Add(key.Item2, new oTrieNode(key.Item2, _singleWord));
                        newLeaf._nodesCollection.Add(key.Item3, new oTrieNode(key.Item3, word));
                        _leafNodeCollection.Add(newLeaf);
                        _singleWord = null;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void DeleteWord(ref string word, int pos)
        {
            if (pos > word.Length - 1)
            {
                return;
            }

            try
            {
                var searchChar = word[pos];

                //link to next branch exists
                if (_singleWord == null)
                {
                    var copyOfWord = word;
                    var mimeKeys = _leafNodeCollection.Select(x => TrieHelper.GetCommonPart(copyOfWord, x._prefix)).OrderByDescending(x => x.Item1.Length);
                    var suitableCandidate = mimeKeys.FirstOrDefault();
                    if (suitableCandidate != null)
                    {
                        var node = _leafNodeCollection.Where(x => x._prefix == suitableCandidate.Item1).FirstOrDefault();

                        if (node != null)
                        {
                            if (node._nodesCollection.ContainsKey(suitableCandidate.Item2))
                            {
                                node._nodesCollection[suitableCandidate.Item2].DeleteWord(ref word, suitableCandidate.Item1.Length);

                                var toDelete = node._nodesCollection.Values.First(x => x._singleWord == "^*^");
                                if (toDelete != null)
                                {
                                    node._nodesCollection.Remove(toDelete._letter);
                                    if (node._nodesCollection.Count == 0)
                                    {
                                        node._prefix = "^*^";
                                    }
                                }

                                var freeLeafs = _leafNodeCollection.Where(x => x._prefix == "^*^");
                                if (freeLeafs != null)
                                {
                                    foreach (var leaf in freeLeafs.ToArray())
                                    {
                                        _leafNodeCollection.Remove(leaf);
                                    }

                                    if (_leafNodeCollection.Count == 0)
                                    {
                                        _singleWord = "^*^";
                                    }
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        return;
                    }
                    return;
                }
                else // have link to word
                {
                    if (_singleWord == word)
                    {
                        _singleWord = "^*^";
                        return;
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public oTrieNode CheckPath(ref string word, int pos)
        {
            if (pos > word.Length - 1)
            {
                return new oTrieNode('\n', _singleWord);
            }

            try
            {
                var searchChar = word[pos];

                //link to next branch exists
                if (_singleWord == null)
                {
                    var copyOfWord = word;
                    var mimeKeys = _leafNodeCollection.Select(x => TrieHelper.GetCommonPart(copyOfWord, x._prefix)).OrderByDescending(x => x.Item1.Length);
                    var suitableCandidate = mimeKeys.FirstOrDefault();
                    if (suitableCandidate != null)
                    {
                        var node = _leafNodeCollection.Where(x => x._prefix == suitableCandidate.Item1).FirstOrDefault();

                        if (node != null)
                        {
                            if (node._nodesCollection.ContainsKey(suitableCandidate.Item2))
                            {
                                return node._nodesCollection[suitableCandidate.Item2].CheckPath(ref word, suitableCandidate.Item1.Length);
                            }
                            else
                            {
                                return null;
                            }
                        }
                        return null;
                    }
                    return null;
                }
                else // have link to other node
                {
                    if (_singleWord == word)
                    {
                        return this;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void getNode(StringBuilder output, int depth, bool markUnused, bool toListBox)
        {
            if (toListBox)
            {
                if (depth > 7)
                {
                    output.Append("***");
                    return;
                }
            }

            if (_leafNodeCollection.Count > 0)
            {
                for (int i = 0; i < (int)Math.Floor(_leafNodeCollection.Count / 2d); i++)
                {
                    _leafNodeCollection[i].getNode(output, depth + 1, markUnused, toListBox);
                }
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }

            if (this._singleWord != null)
            {
                output.Append('\t', depth);
                output.AppendLine("{ " + _letter + " }: " + _singleWord);
            }
            else
            {
                output.Append('\t', depth);
                output.AppendLine("{ " + _letter + " } ->");
            }

            if (_leafNodeCollection.Count > 0)
            {
                for (int i = (int)Math.Floor(_leafNodeCollection.Count / 2d); i < _leafNodeCollection.Count; i++)
                {
                    _leafNodeCollection[i].getNode(output, depth + 1, markUnused, toListBox);
                }
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }
        }
    }

    public class oTrieTree
    {
        private List<oTrieNode> collection;

        public oTrieTree()
        {
            collection = new List<oTrieNode>();
        }

        public bool AddWord(string word)
        {
            if (word.Length > 0)
            {
                var firstChar = word[0];

                var node = collection.FirstOrDefault(x => x._letter == firstChar);

                if (node != null)
                {
                    if (word.Length == 1)
                    {
                        //var mimeKeys = _leafNodeCollection.Select(x => TrieHelper.GetCommonPart(copyOfWord, x._prefix)).OrderByDescending(x => x.Item1.Length);
                        //var suitableCandidate = mimeKeys.FirstOrDefault();
                        //if (suitableCandidate != null)

                        var insertNode = collection.Select(x => x._leafNodeCollection.Where(y => y._prefix == word).FirstOrDefault()).FirstOrDefault();
                        if (insertNode != null)
                        {
                            if (!(insertNode._nodesCollection.ContainsKey('^')))
                            {
                                insertNode._nodesCollection.Add('^', new oTrieNode('^', word));
                                return true;
                            }
                        }
                        else
                        {
                            collection.Add(new oTrieNode('^', word));
                            return true;
                        }
                    }

                    return node.AddWord(ref word, 1);
                }
                else
                {
                    var nextNode = new oTrieNode(firstChar, word);
                    collection.Add(nextNode);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public void DeleteWord(string word)
        {
            if (word.Length > 0)
            {
                var firstChar = word[0];

                if (word.Length == 1)
                {
                    var existenceWord = collection.FirstOrDefault(x => x._singleWord == word);
                    var existenceNode = collection.FirstOrDefault(x => x._leafNodeCollection.Count(y => y._prefix == word && y._nodesCollection.ContainsKey('^')) > 0);

                    if (existenceWord != null)
                    {
                        collection.RemoveAll(x => x._singleWord == word);
                    }

                    if (existenceNode != null)
                    {
                        var delNode = existenceNode._leafNodeCollection.Where(y => y._prefix == word).FirstOrDefault();
                        delNode._nodesCollection.Remove('^');

                        if (delNode._nodesCollection.Count == 0)
                        {
                            delNode._prefix = "^*^";
                        }

                        existenceNode._leafNodeCollection.RemoveAll(x => x._prefix == "^*^");

                        if (existenceNode._leafNodeCollection.Count == 0)
                        {
                            existenceNode._singleWord = "^*^";
                        }

                        collection.RemoveAll(x => x._singleWord == "^*^");
                    }

                    return;
                }

                var node = collection.FirstOrDefault(x => x._letter == firstChar);

                if (node != null)
                {
                    if (node._singleWord == null)
                    {
                        if (node._leafNodeCollection.Count != 0)
                        {
                            node.DeleteWord(ref word, 1);
                        }
                        else
                        {
                            collection.RemoveAll(x => x._letter == firstChar);
                        }

                        if (node._singleWord == "^*^")
                        {
                            collection.RemoveAll(x => x._letter == node._letter);
                        }
                    }
                    else
                    {
                        collection.RemoveAll(x => x._letter == firstChar);
                    }
                }
            }

            return;
        }

        public bool WordExists(string word)
        {
            if (word.Length > 0)
            {
                if (word.Length == 1)
                {
                    var existence = collection.FirstOrDefault(x => (x._singleWord == word) || (x._leafNodeCollection.Count(y => y._nodesCollection.ContainsKey('^')) > 0));
                    if (existence != null)
                    {
                        return true;
                    }
                    return false;
                }

                var firstChar = word[0];

                var node = collection.FirstOrDefault(x => x._letter == firstChar);

                if (node != null)
                {
                    var lastElement = node.CheckPath(ref word, 1);
                    if (lastElement != null)
                    {
                        if (lastElement._letter == '\n')
                        {
                            return true;
                        }

                        if (lastElement._singleWord == word)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public string getTreeView(bool markUnused, bool toListBox = true)
        {
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < (int)Math.Floor(collection.Count / 2d); i++)
            {
                collection[i].getNode(output, 0, markUnused, toListBox);
            }

            for (int i = (int)Math.Floor(collection.Count / 2d); i < collection.Count; i++)
            {
                collection[i].getNode(output, 0, markUnused, toListBox);
            }
            return output.ToString();
        }

        public override string ToString()
        {
            return "Trie tree - " + GetAdressOfObject();
        }

        private IntPtr GetAdressOfObject()
        {
            IntPtr ptr = IntPtr.Zero;
            var xx = this;
            unsafe
            {
                TypedReference tr = __makeref(xx);
                ptr = **(IntPtr**)(&tr);
            }
            return ptr;
        }
    }

    //====================================================================//

    public abstract class TrieNodeBase : IEquatable<TrieNodeBase>
    {
        public char Character { get; private set; }
        private readonly IDictionary<char, TrieNodeBase> children;

        public void getNode(StringBuilder output, int depth, bool markUnused, bool toListBox)
        {
            if (toListBox)
            {
                if (depth > 7)
                {
                    output.Append("***");
                    return;
                }
            }

            if (children.Count > 0)
            {
                var left = children.Take((int)Math.Floor(children.Count / 2d));
                foreach (var item in left)
                {
                    item.Value.getNode(output, depth + 1, markUnused, toListBox);
                }
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }

            if (this.Character == null)
            {
                if (markUnused)
                {
                    output.Append('\t', depth);
                    output.AppendLine("[ X ]");
                }
            }
            else
            {
                output.Append('\t', depth);
                output.AppendLine("[ " + Character + " ]");
            }

            if (children.Count > 0)
            {
                var rignt = children.Skip((int)Math.Floor(children.Count / 2d)).Take(children.Count);
                foreach (var item in rignt)
                {
                    item.Value.getNode(output, depth + 1, markUnused, toListBox);
                }
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }
        }


        public IDictionary<char, TrieNodeBase> Elements
        {
            get
            {
                return children;
            }
        }

        internal TrieNodeBase(char character)
        {
            Character = character;
            children = new Dictionary<char, TrieNodeBase>();
        }

        internal IEnumerable<TrieNodeBase> GetChildrenInner()
        {
            return children.Values;
        }

        internal TrieNodeBase GetChildInner(char character)
        {
            TrieNodeBase trieNode;
            children.TryGetValue(character, out trieNode);
            return trieNode;
        }

        internal TrieNodeBase GetTrieNodeInner(string prefix)
        {
            TrieNodeBase trieNode = this;
            foreach (var prefixChar in prefix)
            {
                trieNode = trieNode.GetChildInner(prefixChar);
                if (trieNode == null)
                {
                    break;
                }
            }
            return trieNode;
        }

        internal void SetChild(TrieNodeBase child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(FutureExtensions.nameof(() => child));
            }
            children[child.Character] = child;
        }

        internal void RemoveChild(char character)
        {
            children.Remove(character);
        }

        internal virtual void Clear()
        {
            children.Clear();
        }

        public bool Equals(TrieNodeBase other)
        {
            return Character == other.Character;
        }
    }

    public class TrieNode : TrieNodeBase
    {
        public bool IsWord
        {
            get { return WordCount > 0; }
        }

        public int WordCount { get; internal set; }

        internal TrieNode(char character)
            : base(character)
        {
            WordCount = 0;
        }

        internal override void Clear()
        {
            base.Clear();
            WordCount = 0;
        }

        public TrieNode GetChild(char character)
        {
            return base.GetChildInner(character) as TrieNode;
        }

        public bool HasChild(char character)
        {
            return GetChild(character) != null;
        }

        public TrieNode GetTrieNode(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(FutureExtensions.nameof(() => prefix));
            }
            return base.GetTrieNodeInner(prefix) as TrieNode;
        }

        public IEnumerable<TrieNode> GetChildren()
        {
            return base.GetChildrenInner().Cast<TrieNode>();
        }
    }

    public class Trie
    {
        private readonly TrieNode rootTrieNode;

        public void getNode(StringBuilder output, int depth, bool markUnused, bool toListBox)
        {
            if (toListBox)
            {
                if (depth > 7)
                {
                    output.Append("***");
                    return;
                }
            }

            var lst = rootTrieNode.Elements.Values.ToList();

            if (rootTrieNode.Elements.Count > 0)
            {
                for (int i = 0; i < (int)Math.Floor(rootTrieNode.Elements.Count / 2d); i++)
                {
                    lst[i].getNode(output, depth + 1, markUnused, toListBox);
                }
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }


            output.Append('\t', depth);
            output.AppendLine("{ " + this.rootTrieNode.Character + " }");


            if (rootTrieNode.Elements.Count > 0)
            {
                for (int i = (int)Math.Floor(rootTrieNode.Elements.Count / 2d); i < rootTrieNode.Elements.Count; i++)
                {
                    lst[i].getNode(output, depth + 1, markUnused, toListBox);
                }
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }
        }

        public string getTreeView(bool markUnused, bool toListBox = true)
        {
            StringBuilder output = new StringBuilder();

            var lst = rootTrieNode.Elements.Values.ToList();

            for (int i = 0; i < (int)Math.Floor(lst.Count / 2d); i++)
            {
                lst[i].getNode(output, 0, markUnused, toListBox);
            }

            for (int i = (int)Math.Floor(lst.Count / 2d); i < lst.Count; i++)
            {
                lst[i].getNode(output, 0, markUnused, toListBox);
            }
            return output.ToString();
        }

        public Trie()
        {
            rootTrieNode = new TrieNode(' ');
        }

        public void AddWord(string word)
        {
            if (word == null)
            {
                throw new ArgumentNullException(FutureExtensions.nameof(() => word));
            }
            AddWord(rootTrieNode, word.ToCharArray());
        }

        public int RemoveWord(string word)
        {
            if (word == null)
            {
                throw new ArgumentNullException(FutureExtensions.nameof(() => word));
            }
            return RemoveWord(GetTrieNodesStack(word));
        }

        public void RemovePrefix(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(FutureExtensions.nameof(() => prefix));
            }
            RemovePrefix(GetTrieNodesStack(prefix, false));
        }

        public IEnumerable<string> GetWords()
        {
            return GetWords("");
        }

        public IEnumerable<string> GetWords(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(FutureExtensions.nameof(() => prefix));
            }
            foreach (var word in Traverse(GetTrieNode(prefix), new StringBuilder(prefix)))
            {
                yield return word;
            }
        }

        public bool HasWord(string word)
        {
            if (word == null)
            {
                throw new ArgumentNullException(FutureExtensions.nameof(() => word));
            }
            var trieNode = GetTrieNode(word);
            if (trieNode != null)
            {
                return trieNode.IsWord;
            }
            return false;
        }

        public bool HasPrefix(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(FutureExtensions.nameof(() => prefix));
            }
            return GetTrieNode(prefix) != null;
        }

        /// </summary>
        public TrieNode GetTrieNode(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(FutureExtensions.nameof(() => prefix));
            }
            return rootTrieNode.GetTrieNode(prefix);
        }

        public int WordCount(string word)
        {
            if (word == null)
            {
                throw new ArgumentNullException(FutureExtensions.nameof(() => word));
            }
            var trieNode = GetTrieNode(word);
            if (trieNode != null)
            {
                return trieNode.WordCount;
            }
            return 0;
        }

        public ICollection<string> GetLongestWords()
        {
            var longestWords = new List<string>();
            var buffer = new StringBuilder();
            var length = new Wrapped<int>(0);
            GetLongestWords(rootTrieNode, longestWords, buffer, length);
            return longestWords;
        }

        public ICollection<string> GetShortestWords()
        {
            var shortestWords = new List<string>();
            var buffer = new StringBuilder();
            var length = new Wrapped<int>(int.MaxValue);
            GetShortestWords(rootTrieNode, shortestWords, buffer, length);
            return shortestWords;
        }

        public void Clear()
        {
            rootTrieNode.Clear();
        }

        public int Count()
        {
            var count = new Wrapped<int>(0);
            GetCount(rootTrieNode, count, false);
            return count.Value;
        }

        public int UniqueCount()
        {
            var count = new Wrapped<int>(0);
            GetCount(rootTrieNode, count, true);
            return count.Value;
        }

        public TrieNode GetRootTrieNode()
        {
            return rootTrieNode;
        }

        private void AddWord(TrieNode trieNode, char[] word)
        {
            foreach (var c in word)
            {
                var child = trieNode.GetChild(c);
                if (child == null)
                {
                    child = new TrieNode(c);
                    trieNode.SetChild(child);
                }
                trieNode = child;
            }
            trieNode.WordCount++;
        }

        private IEnumerable<string> Traverse(TrieNode trieNode, StringBuilder buffer)
        {
            if (trieNode == null)
            {
                yield break;
            }
            if (trieNode.IsWord)
            {
                yield return buffer.ToString();
            }
            foreach (var child in trieNode.GetChildren())
            {
                buffer.Append(child.Character);
                foreach (var word in Traverse(child, buffer))
                {
                    yield return word;
                }
                buffer.Length--;
            }
        }

        private void GetLongestWords(TrieNode trieNode,
            ICollection<string> longestWords, StringBuilder buffer, Wrapped<int> length)
        {
            if (trieNode.IsWord)
            {
                if (buffer.Length > length.Value)
                {
                    longestWords.Clear();
                    length.Value = buffer.Length;
                }
                if (buffer.Length == length.Value)
                {
                    longestWords.Add(buffer.ToString());
                }
            }
            foreach (var child in trieNode.GetChildren())
            {
                buffer.Append(child.Character);
                GetLongestWords(child, longestWords, buffer, length);
                buffer.Length--;
            }
        }

        private void GetShortestWords(TrieNode trieNode,
            ICollection<string> shortestWords, StringBuilder buffer, Wrapped<int> length)
        {
            if (trieNode.IsWord)
            {
                if (buffer.Length < length.Value)
                {
                    shortestWords.Clear();
                    length.Value = buffer.Length;
                }
                if (buffer.Length == length.Value)
                {
                    shortestWords.Add(buffer.ToString());
                }
            }
            foreach (var child in trieNode.GetChildren())
            {
                buffer.Append(child.Character);
                GetShortestWords(child, shortestWords, buffer, length);
                buffer.Length--;
            }
        }

        private Stack<TrieNode> GetTrieNodesStack(string s, bool isWord = true)
        {
            var nodes = new Stack<TrieNode>(s.Length + 1);
            var trieNode = rootTrieNode;
            nodes.Push(trieNode);
            foreach (var c in s)
            {
                trieNode = trieNode.GetChild(c);
                if (trieNode == null)
                {
                    nodes.Clear();
                    break;
                }
                nodes.Push(trieNode);
            }
            if (isWord)
            {
                if (trieNode != null)
                {
                    if (trieNode.IsWord)
                    {
                        throw new ArgumentOutOfRangeException(s + " does not exist in trie.");
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(s + " does not exist in trie.");
                }
            }
            return nodes;
        }

        private int RemoveWord(Stack<TrieNode> trieNodes)
        {
            var removeCount = trieNodes.Peek().WordCount;
            // Mark the last trieNode as not a word
            trieNodes.Peek().WordCount = 0;
            // Trim excess trieNodes
            Trim(trieNodes);
            return removeCount;
        }

        private void RemovePrefix(Stack<TrieNode> trieNodes)
        {
            if (trieNodes.Any())
            {
                // Clear the last trieNode
                trieNodes.Peek().Clear();
                // Trim excess trieNodes
                Trim(trieNodes);
            }
        }

        private void Trim(Stack<TrieNode> trieNodes)
        {
            while (trieNodes.Count > 1)
            {
                var trieNode = trieNodes.Pop();
                var parentTrieNode = trieNodes.Peek();
                if (trieNode.IsWord || trieNode.GetChildren().Any())
                {
                    break;
                }
                parentTrieNode.RemoveChild(trieNode.Character);
            }
        }

        private void GetCount(TrieNode trieNode, Wrapped<int> count, bool isUnique)
        {
            if (trieNode.IsWord)
            {
                count.Value += isUnique ? 1 : trieNode.WordCount;
            }
            foreach (var child in trieNode.GetChildren())
            {
                GetCount(child, count, isUnique);
            }
        }
    }

    public class TrieNode<TValue> : TrieNodeBase
    {
        public TValue Value
        {
            get { return _value; }
            internal set
            {
                if (value == null)
                {
                    hasValue = false;
                    _value = value;
                    return;
                }
                _value = value;
                hasValue = true;
            }
        }
        private bool hasValue;
        private TValue _value;

        internal TrieNode(char character)
            : base(character)
        { }

        public bool HasValue()
        {
            return hasValue;
        }

        internal override void Clear()
        {
            base.Clear();
            Value = default(TValue);
            hasValue = false;
        }

        public TrieNode<TValue> GetChild(char character)
        {
            return base.GetChildInner(character) as TrieNode<TValue>;
        }

        public bool HasChild(char character)
        {
            return GetChild(character) != null;
        }

        public TrieNode<TValue> GetTrieNode(string prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(FutureExtensions.nameof(() => prefix));
            }
            return base.GetTrieNodeInner(prefix) as TrieNode<TValue>;
        }

        public IEnumerable<TrieNode<TValue>> GetChildren()
        {
            return base.GetChildrenInner().Cast<TrieNode<TValue>>();
        }
    }

    public class Wrapped<T>
    {
        public T Value { get; set; }
        public Wrapped(T value)
        {
            Value = value;
        }
    }

    //====================================================================//

    public class BranchingTreeNode
    {
        private List<BranchingTreeNode> collection;
        private char _key;
        private int _depth;

        public bool AlsoSingle = false;

        public char Key
        {
            get
            {
                return _key;
            }
        }

        public int ChildNum
        {
            get
            {
                return collection.Count;
            }
        }

        public BranchingTreeNode(char key, int depth)
        {
            _key = key;
            _depth = depth;

            collection = new List<BranchingTreeNode>();
        }

        public BranchingTreeNode(char key, List<char> elements, int length, int depth)
        {
            _key = key;
            _depth = depth;

            collection = new List<BranchingTreeNode>();

            if (length > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    collection.Add(new BranchingTreeNode(elements[i], elements, length - 1, depth + 1));
                }
            }
            else
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    collection.Add(null);// new BranchingTreeNode('\n', elements, 0, true);
                }
            }
        }

        public BranchingTreeNode CheckPath(ref string word, int pos)
        {
            if (pos > word.Length - 1)
            {
                return new BranchingTreeNode('\n', pos);
            }

            var searchChar = word[pos];

            try
            {
                if (collection != null)
                {
                    var nextBranch = collection.FirstOrDefault(x => x.Key == searchChar);
                    if ((nextBranch != null))
                    {
                        return nextBranch.CheckPath(ref word, pos + 1);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public bool AddWord(ref string word, int pos)
        {
            if (pos > word.Length - 1)
            {
                return true;
            }

            var searchChar = word[pos];

            try
            {
                var nextBranch = collection.FirstOrDefault(x => x.Key == searchChar);
                if (nextBranch != null)
                {
                    return nextBranch.AddWord(ref word, pos + 1);
                }
                else
                {
                    var nextNode = new BranchingTreeNode(searchChar, pos);
                    collection.Add(nextNode);
                    return nextNode.AddWord(ref word, pos + 1);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void DeleteWord(ref string word, int pos)
        {
            if (pos > word.Length - 1)
            {
                return;
            }

            var searchChar = word[pos];

            try
            {
                var nextBranch = collection.FirstOrDefault(x => x.Key == searchChar);
                if (nextBranch != null)
                {
                    if (nextBranch.ChildNum != 0)
                    {
                        nextBranch.DeleteWord(ref word, pos + 1);
                    }
                    else
                    {
                        collection.RemoveAll(x => x.Key == searchChar);
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void getNode(StringBuilder output, int depth, bool markUnused, bool toListBox)
        {
            if (toListBox)
            {
                if (depth > 7)
                {
                    output.Append("***");
                    return;
                }
            }

            if (collection.Count > 0)
            {
                for (int i = 0; i < (int)Math.Floor(collection.Count / 2d); i++)
                {
                    collection[i].getNode(output, depth + 1, markUnused, toListBox);
                }
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }

            if (this.Key == null)
            {
                if (markUnused)
                {
                    output.Append('\t', depth);
                    output.AppendLine("X");
                }
            }
            else
            {
                output.Append('\t', depth);
                output.AppendLine(_key.ToString());
            }

            if (collection.Count > 0)
            {
                for (int i = (int)Math.Floor(collection.Count / 2d); i < collection.Count; i++)
                {
                    collection[i].getNode(output, depth + 1, markUnused, toListBox);
                }
            }
            else
            {
                if (markUnused)
                {
                    output.Append('\t', depth + 1);
                    output.AppendLine("X");
                }
            }
        }
    }

    public class BranchingTree
    {
        private List<BranchingTreeNode> collection;

        public BranchingTree()
        {
            collection = new List<BranchingTreeNode>();
        }

        public BranchingTree(List<char> elements, int length)
        {
            if (length == 0)
            {
                throw new Exception("length can not be equial to zero");
            }

            collection = new List<BranchingTreeNode>();

            for (int i = 0; i < elements.Count; i++)
            {
                collection.Add(new BranchingTreeNode(elements[i], elements, length - 1, 1));
            }
        }

        public bool WordExists(string word)
        {
            if (word.Length > 0)
            {
                var firstChar = word[0];

                if (word.Length == 1)
                {
                    var existence = collection.FirstOrDefault(x => x.Key == firstChar && x.AlsoSingle);
                    if (existence != null)
                    {
                        return true;
                    }
                    return false;
                }

                var node = collection.FirstOrDefault(x => x.Key == firstChar);

                if (node != null)
                {
                    var lastElement = node.CheckPath(ref word, 1);
                    if (lastElement != null)
                    {
                        if (lastElement.Key == '\n')
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public bool AddWord(string word)
        {
            if (word.Length > 0)
            {
                var firstChar = word[0];

                var node = collection.FirstOrDefault(x => x.Key == firstChar);

                if (node != null)
                {
                    return node.AddWord(ref word, 1);
                }
                else
                {
                    var nextNode = new BranchingTreeNode(firstChar, 0);
                    if (word.Length == 1)
                    {
                        nextNode.AlsoSingle = true;
                    }

                    collection.Add(nextNode);
                    return nextNode.AddWord(ref word, 1);
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public void DeleteWord(string word)
        {
            if (word.Length > 0)
            {
                var firstChar = word[0];

                var node = collection.FirstOrDefault(x => x.Key == firstChar);

                if (node != null)
                {
                    if (word.Length == 1)
                    {
                        node.AlsoSingle = false;
                    }

                    if (node.ChildNum != 0)
                    {
                        node.DeleteWord(ref word, 1);
                        if ((node.ChildNum == 0) && (node.AlsoSingle == false))
                        {
                            collection.RemoveAll(x => x.Key == firstChar);
                        }
                    }
                    else
                    {
                        collection.RemoveAll(x => x.Key == firstChar);
                    }
                }
            }

            return;
        }

        public string getTreeView(bool markUnused, bool toListBox = true)
        {
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < (int)Math.Floor(collection.Count / 2d); i++)
            {
                collection[i].getNode(output, 0, markUnused, toListBox);
            }

            for (int i = (int)Math.Floor(collection.Count / 2d); i < collection.Count; i++)
            {
                collection[i].getNode(output, 0, markUnused, toListBox);
            }
            return output.ToString();
        }

        public override string ToString()
        {
            return "Branching tree - " + GetAdressOfObject();
        }

        private IntPtr GetAdressOfObject()
        {
            IntPtr ptr = IntPtr.Zero;
            var xx = this;
            unsafe
            {
                TypedReference tr = __makeref(xx);
                ptr = **(IntPtr**)(&tr);
            }
            return ptr;
        }
    }

    //class BranchingTreeCollection<T>
    //{
    //    private T[] collection;

    //    public BranchingTreeCollection(List<char> letters, int length)
    //    {
    //        collection = new T[letters.Count];
    //    }
    //    public T this[int i]
    //    {
    //        get
    //        {
    //            return collection[i];
    //        }
    //        set
    //        {
    //            collection[i] = value;
    //        }
    //    }
    //}
}
