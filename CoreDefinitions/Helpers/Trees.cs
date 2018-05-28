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

    public class TrieLeafNode
    {
        public string _prefix = "";
        public int _depth;
        public Dictionary<char, TrieNode> _nodesCollection;
        public TrieLeafNode(string prefix, int depth)
        {
            _nodesCollection = new Dictionary<char, TrieNode>();
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

    public class TrieNode
    {
        public char _letter;
        public string _singleWord;
        public List<TrieLeafNode> _leafNodeCollection;

        public TrieNode(char letter, string key)
        {
            _letter = letter;
            _singleWord = key;
            _leafNodeCollection = new List<TrieLeafNode>();
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
                                node._nodesCollection.Add(suitableCandidate.Item2, new TrieNode(suitableCandidate.Item2, copyOfWord));
                            }
                        }
                        else
                        {
                            var newLeaf = new TrieLeafNode(suitableCandidate.Item1, pos);
                            newLeaf._nodesCollection.Add(suitableCandidate.Item2, new TrieNode(suitableCandidate.Item2, copyOfWord));
                            _leafNodeCollection.Add(newLeaf);
                        }
                        return true;
                    }
                    else
                    {
                        var newLeaf = new TrieLeafNode(suitableCandidate.Item1, pos);
                        newLeaf._nodesCollection.Add(suitableCandidate.Item2, new TrieNode(suitableCandidate.Item2, copyOfWord));
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

                        var newLeaf = new TrieLeafNode(key.Item1, pos);
                        newLeaf._nodesCollection.Add(key.Item2, new TrieNode(key.Item2, _singleWord));
                        newLeaf._nodesCollection.Add(key.Item3, new TrieNode(key.Item3, word));
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
            return;

            //if (pos > word.Length - 1)
            //{
            //    return;
            //}

            //var searchChar = word[pos];

            //try
            //{
            //    var nextBranch = collection.FirstOrDefault(x => x.Key == searchChar);
            //    if (nextBranch != null)
            //    {
            //        if (nextBranch.ChildNum != 0)
            //        {
            //            nextBranch.DeleteWord(ref word, pos + 1);
            //        }
            //        else
            //        {
            //            collection.RemoveAll(x => x.Key == searchChar);
            //        }
            //    }
            //    return;
            //}
            //catch (Exception ex)
            //{
            //    return;
            //}
        }

        public TrieNode CheckPath(ref string word, int pos)
        {
            return null;

            //if (pos > word.Length - 1)
            //{
            //    return new TrieNode("\n", pos);
            //}

            //try
            //{

            //    var subKey = word.Substring(0, pos);

            //    //link to next branch exists
            //    if (_singleWord == null)
            //    {
            //        var nextBranch = collection.FirstOrDefault(x => x.Key == subKey);
            //        if ((nextBranch != null))
            //        {
            //            return nextBranch.CheckPath(ref word, pos + 1);
            //        }
            //    }
            //    else
            //    {
            //        if (_singleWord == subKey)
            //        {
            //            return this;
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //    }
            //    return null;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}

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

            //_letter = letter;
            //_singleWord = key;

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

    public class Trie
    {
        private List<TrieNode> collection;

        public Trie()
        {
            collection = new List<TrieNode>();
        }

        public bool AddWord(string word)
        {
            if (word.Length > 0)
            {
                var firstChar = word[0];

                var node = collection.FirstOrDefault(x => x._letter == firstChar);

                if (node != null)
                {
                    return node.AddWord(ref word, 1);
                }
                else
                {
                    var nextNode = new TrieNode(firstChar, word);
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
            return;

            //if (word.Length > 0)
            //{
            //    var firstChar = word[0];

            //    var node = collection.FirstOrDefault(x => x.Key == firstChar);

            //    if (node != null)
            //    {
            //        if (node.ChildNum != 0)
            //        {
            //            node.DeleteWord(ref word, 1);
            //        }
            //        else
            //        {
            //            collection.RemoveAll(x => x.Key == firstChar);
            //        }
            //    }
            //}

            //return;
        }

        public bool WordExists(string word)
        {
            return false;

            //if (word.Length > 0)
            //{
            //    var firstChar = word[0];

            //    var node = collection.FirstOrDefault(x => x.Key == firstChar);

            //    if (node != null)
            //    {
            //        var lastElement = node.CheckPath(ref word, 1);
            //        if (lastElement != null)
            //        {
            //            if (lastElement.Key == '\n')
            //            {
            //                return true;
            //            }
            //        }
            //    }

            //    return false;
            //}
            //else
            //{
            //    return false;
            //}
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

    public class BranchingTreeNode
    {
        private List<BranchingTreeNode> collection;
        private char _key;
        private int _depth;

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
                    if (node.ChildNum != 0)
                    {
                        node.DeleteWord(ref word, 1);
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
