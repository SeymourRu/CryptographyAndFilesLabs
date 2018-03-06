using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Security.Cryptography;

namespace CoreDefinitions.Helpers
{
    public enum TaskAppType
    {
        GUI = 0,
        Console = 1,
    }

    public enum HashType
    {
        MD5 = 0,
        SHA256 = 1,
    }

    public class Node
    {
        public int position;
        public int value;

        public override string ToString()
        {
            return "pos:" + position.ToString() + ",val:" + value.ToString();
        }

        public static List<int> CalculateDelta(List<int> shuff)
        {
            var deltaLst = new List<int>();
            for (int i = 0; i < shuff.Count; i++)
            {
                var delta = (shuff.ElementAt(i) - i) - 1;
                deltaLst.Add(delta);
            }
            return deltaLst;
        }
    }

    [Serializable]
    public struct SelfOrganizeFileNode
    {
        public string text;

        public override string ToString()
        {
            return "text:" + text.ToString();
        }
    }

    [Serializable]
    public struct SelfOrganizeIndexNode
    {
        public string hash;
        public long offset;
        public int len;

        public override string ToString()
        {
            return "hash:" + hash + ", offset:" + offset.ToString();
        }
    }

    [Serializable]
    public struct SelfOrganizeIndexHeader
    {
        public int version;
        public HashType hashType;
        public long count;
        public long nodesBeginOffset;

        public override string ToString()
        {
            return "hashType:" + hashType.ToString() + ", nodes offset:" + nodesBeginOffset.ToString();
        }
    }

    public static class Helper
    {
        public static void LoadFile(string tip, string ext, List<int> lst)
        {
            var dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = string.Format("{0} (*.{1})|*.{1}", tip, ext);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var lines = System.IO.File.ReadAllLines(dlg.FileName);
                    lst.Clear();
                    foreach (var line in lines.Where(x => !string.IsNullOrEmpty(x)))
                    {
                        lst.Add(int.Parse(line));
                    }
                }
                catch
                {
                    MessageBox.Show("Не верный файл:" + dlg.FileName);
                }
            }
        }

        public static void LoadFileNode(string tip, string ext, List<Node> lst)
        {
            var dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = string.Format("{0} (*.{1})|*.{1}", tip, ext);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var lines = System.IO.File.ReadAllLines(dlg.FileName);
                    lst.Clear();
                    int counter = 0;
                    foreach (var line in lines.Where(x => !string.IsNullOrEmpty(x)))
                    {
                        lst.Add(new Node()
                        {
                            position = counter,
                            value = int.Parse(line)
                        });
                        counter++;
                    }
                }
                catch
                {
                    MessageBox.Show("Не верный файл:" + dlg.FileName);
                }
            }
        }

        //Write to file functions
        public static long WriteToBinaryFile<T>(string filePath, T objectToWrite, FileMode mode, long moveToOffset = -1, bool before = true)
        {
            long offset = 0;
            using (Stream stream = File.Open(filePath, mode))
            {
                if (moveToOffset != -1)
                {
                    stream.Position = moveToOffset;
                }
                if (before)
                {
                    offset = stream.Position;
                }

                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
                if (!before)
                {
                    offset = stream.Position;
                }
            }
            return offset;
        }
        public static long WriteToBinaryFileList<T>(string filePath, List<T> objectsToWrite, FileMode mode, long moveToOffset = -1, bool before = true)
        {
            long offset = 0;
            using (Stream stream = File.Open(filePath, mode))
            {
                if (moveToOffset != -1)
                {
                    stream.Position = moveToOffset;
                }

                if (before)
                {
                    offset = stream.Position;
                }
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                foreach (var item in objectsToWrite)
                {
                    binaryFormatter.Serialize(stream, item);
                }
                if (!before)
                {
                    offset = stream.Position;
                }
            }
            return offset;
        }
        public static long WriteToBinaryFileLinkedList<T>(string filePath, LinkedList<T> objectsToWrite, FileMode mode, long moveToOffset = -1, bool before = true)
        {
            long offset = 0;
            using (Stream stream = File.Open(filePath, mode))
            {
                if (moveToOffset != -1)
                {
                    stream.Position = moveToOffset;
                }

                if (before)
                {
                    offset = stream.Position;
                }
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                var st = objectsToWrite.First;
                while (st.Next != null)
                {
                    binaryFormatter.Serialize(stream, st.Value);
                    st = st.Next;
                }

                binaryFormatter.Serialize(stream, st.Value);

                if (!before)
                {
                    offset = stream.Position;
                }
            }
            return offset;
        }

        //Read from file functions
        public static T ReadFromBinaryFile<T>(string filePath, long pos)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                stream.Position = pos;
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                try
                {
                    var deser = (T)binaryFormatter.Deserialize(stream);
                    return deser;
                }
                catch
                {
                    return default(T);
                }
            }
        }
        public static List<T> ReadFromBinaryFileList<T>(string filePath, long pos, int count, bool all = false)
        {
            var res = new List<T>();
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                stream.Position = pos;
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                if (all)
                {
                    count = 1;
                }

                while (true && count > 0)
                {
                    try
                    {
                        var deser = (T)binaryFormatter.Deserialize(stream);
                        res.Add(deser);
                    }
                    catch
                    {
                        break;
                    }

                    if (!all)
                    {
                        count = count - 1;
                    }
                }

                return res;
            }
        }
        public static LinkedList<T> ReadFromBinaryFileLinkedList<T>(string filePath, long pos, int count, bool all = false)
        {
            var res = new LinkedList<T>();
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                stream.Position = pos;
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                if (all)
                {
                    count = 1;
                }

                while (true && count > 0)
                {
                    try
                    {
                        var deser = (T)binaryFormatter.Deserialize(stream);
                        var node = new LinkedListNode<T>(deser);
                        res.AddLast(node);
                    }
                    catch
                    {
                        break;
                    }

                    if (!all)
                    {
                        count = count - 1;
                    }
                }

                return res;
            }
        }

        //Hash functions
        private static string GenerateMD5(string randomString)
        {
            string result;
            using (MD5 hash = MD5.Create())
            {
                result = String.Join
                (
                    "",
                    from ba in hash.ComputeHash
                    (
                        Encoding.UTF8.GetBytes(randomString)
                    )
                    select ba.ToString("x2")
                );
            }
            return result;
        }
        private static string GenerateSHA256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        public static string GetHash(string obj, HashType hashType)
        {
            switch (hashType)
            {
                case HashType.MD5:
                    {
                        return GenerateMD5(obj);
                    }
                case HashType.SHA256:
                    {
                        return GenerateSHA256(obj);
                    }
                default:
                    {
                        throw new Exception("Set hash function type");
                    }
            }
        }
    }

    public static class LinkedListExtensions
    {
        public static LinkedList<T> AddAndMove<T>(this LinkedList<T> source, LinkedListNode<T> first, LinkedListNode<T> second)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return null;
        }

        public static LinkedList<T> SwapPairwise<T>(this LinkedList<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var current = source.First;

            if (current == null)
            {
                return source;
            }

            while (current.Next != null)
            {
                current.SwapWith(current.Next);
                current = current.Next;

                if (current != null)
                {
                    current = current.Next;
                }
            }

            return source;
        }

        public static void SwapWith<T>(this LinkedListNode<T> first, LinkedListNode<T> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            if (second == null)
                throw new ArgumentNullException("second");

            var tmp = first.Value;
            first.Value = second.Value;
            second.Value = tmp;
        }
    }
}
