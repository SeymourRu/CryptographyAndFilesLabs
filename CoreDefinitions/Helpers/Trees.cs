using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDefinitions.Helpers
{
    public class BinaryTree<T>
    {
        private T _value;
        public T Value
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

        public BinaryTree(T key)
        {
            _value = key;
        }

        public BinaryTree<T> Left;
        public BinaryTree<T> Right;


        public string getTreeView()
        {
            StringBuilder output = new StringBuilder();
            getNode(output, 0);
            return output.ToString();
        }

        private void getNode(StringBuilder output, int depth)
        {

            if (Right != null)
            {
                Right.getNode(output, depth + 1);
            }

            output.Append('\t', depth);
            output.AppendLine(_value.ToString());

            if (Left != null)
            {
                Left.getNode(output, depth + 1);
            }
        }
    }

    public static class BinaryTreeHelpers
    {
        public static BinaryTree<T> NewNode<T>(this BinaryTree<T> source, T key)
        {
            return new BinaryTree<T>(key);
        }

        public static BinaryTree<T> Node<T>(this BinaryTree<T> source)
        {
            return source;
        }

        public static T Key<T>(this BinaryTree<T> source)
        {
            return source.Value;
        }

        public static BinaryTree<T> LLink<T>(this BinaryTree<T> source)
        {
            return source.Left;
        }

        public static BinaryTree<T> RLink<T>(this BinaryTree<T> source)
        {
            return source.Right;
        }


    }
}
