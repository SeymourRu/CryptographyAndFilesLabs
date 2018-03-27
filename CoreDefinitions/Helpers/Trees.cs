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

        public void SetByA(int a,BinaryTree<T, K> node)
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
}
