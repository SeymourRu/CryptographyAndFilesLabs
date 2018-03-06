using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Configuration;

namespace CoreDefinitions.Tasks
{
    public class Files_Task1_2 : ITask<Files_Task1_2>, IBaseTask
    {
        TaskAppType _subSystemType;
        HashType _hashType;
        int _version;
        ConsoleHandler _console;

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Files_Task1_2()
        {
            _subSystemType = Helpers.TaskAppType.Console;
            string result = ConfigurationManager.AppSettings["HashEncryption"];
            var type = 0;
            int.TryParse(result, out type);
            _hashType = (Helpers.HashType)type;
            _version = 1;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            _console = console;
            Console.WriteLine("To exit press ctrl + c");

            try
            {
                MailLoop();
            }
            catch (Exception ex)
            {
                if (!ex.StackTrace.Contains("System.IO"))
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void MailLoop()
        {
            while (true)
            {
                Console.WriteLine("0-exit\r\n1-Generate new self-organizing file\r\n2-Read index header\r\n3-Add new values (if it does not exists)\r\n4-Check if file still correct\r\n");
                var input = Console.ReadLine();
                try
                {
                    var res = int.Parse(input);
                    _console.Busy();
                    switch (res)
                    {
                        case 0:
                            ConsoleImports.FreeConsole();
                            return;
                        case 1:
                            {
                                GenerateNewSelfOrganizingFile();
                                Console.WriteLine("Ok,file generated\r\n");
                            }
                            break;
                        case 2:
                            {
                                ReadHeader();
                                Console.WriteLine("Ready\r\n");
                            }
                            break;
                        case 3:
                            {
                                AddNewValue();
                                Console.WriteLine("Ready\r\n");
                            }
                            break;
                        case 4:
                            {
                                CheckCorrectness();
                                Console.WriteLine("Ready\r\n");
                            }
                            break;
                    }
                    _console.Free();
                }
                catch
                {
                    Console.WriteLine("Could not parse.Wrong menu id?");
                }
            }
        }

        string GenerateNameForNewFile()
        {
            return DateTime.Now.ToString("Genera\\te_yy_dd_MM_H_mm_ss");
        }

        void GenerateNewSelfOrganizingFile()
        {
            OpenFileDialog openDic = new OpenFileDialog();
            openDic.Multiselect = false;

            var items = new List<SelfOrganizeIndexNode>();

            if (openDic.ShowDialog() == DialogResult.OK)
            {
                var fileName = GenerateNameForNewFile();
                var lines = System.IO.File.ReadAllLines(openDic.FileName);
                if (lines.Count() > 0)
                {
                    Console.WriteLine("Be patient!Operation may take some time,depending on input file");
                    var indexFile = fileName + ".index";
                    var baseFile = fileName + ".base";

                    //Write 1st text element
                    var firstFileNode = new SelfOrganizeFileNode() { text = lines.ElementAt(0) };
                    var firstFileNodePos = Helper.WriteToBinaryFile<SelfOrganizeFileNode>(baseFile, firstFileNode, FileMode.CreateNew);

                    var header = new SelfOrganizeIndexHeader()
                    {
                        version = _version,
                        hashType = _hashType,
                        count = lines.Count(),
                        nodesBeginOffset = 0,
                    };

                    //Write header
                    var firstIndexPos = Helper.WriteToBinaryFile<SelfOrganizeIndexHeader>(indexFile, header, FileMode.CreateNew, -1, false);

                    //Update header with correct information
                    header.nodesBeginOffset = firstIndexPos;
                    Helper.WriteToBinaryFile<SelfOrganizeIndexHeader>(indexFile, header, FileMode.Open, 0);

                    var firstIndexNode = new SelfOrganizeIndexNode()
                        {
                            hash = Helper.GetHash(firstFileNode.text, _hashType),
                            len = Marshal.SizeOf(firstFileNode),
                            offset = firstFileNodePos
                        };

                    //Add 1st index
                    Helper.WriteToBinaryFile<SelfOrganizeIndexNode>(indexFile, firstIndexNode, FileMode.Append);

                    var res = new List<SelfOrganizeIndexNode>();

                    for (int i = 1; i < lines.Count(); i++)
                    {
                        var item = lines[i];

                        var fileNode = new SelfOrganizeFileNode() { text = item };
                        var lastNodePos = Helper.WriteToBinaryFile<SelfOrganizeFileNode>(baseFile, fileNode, FileMode.Append);

                        var indexNode = new SelfOrganizeIndexNode()
                            {
                                hash = Helper.GetHash(item, _hashType),
                                len = System.Runtime.InteropServices.Marshal.SizeOf(fileNode),
                                offset = lastNodePos
                            };

                        res.Add(indexNode);
                    }
                    Helper.WriteToBinaryFileList<SelfOrganizeIndexNode>(indexFile, res, FileMode.Append);
                    res.Clear();
                }
            }
        }

        void ReadHeader()
        {
            OpenFileDialog openDic = new OpenFileDialog();
            openDic.Multiselect = false;
            openDic.Filter = string.Format("{0} (*.{1})|*.{1}", "Index файл", "index");
            if (openDic.ShowDialog() == DialogResult.OK)
            {
                var header = Helper.ReadFromBinaryFile<SelfOrganizeIndexHeader>(openDic.FileName, 0);

                if (header.version == 0)
                {
                    Console.WriteLine("Incorrect file header");
                    MessageBox.Show("Incorrect file header");
                    return;
                }

                if (header.version != _version)
                {
                    Console.WriteLine("Old version,fix it");
                    MessageBox.Show("Old version, fix it");
                    return;
                }

                Console.WriteLine("Version:" + header.version + "; nodes count:" + header.count + "; hash type:" + header.hashType.ToString());

                Console.WriteLine("First 10 records");
                var lst = Helper.ReadFromBinaryFileList<SelfOrganizeIndexNode>(openDic.FileName, header.nodesBeginOffset, 10);

                for (int i = 0; i < (lst.Count > 10 ? 10 : lst.Count); i++)
                {
                    var item = lst.ElementAt(i);
                    Console.WriteLine(item);
                }
            }
        }

        void AddNewValue()
        {
            OpenFileDialog openDic = new OpenFileDialog();
            openDic.Multiselect = false;
            openDic.Filter = string.Format("{0} (*.{1})|*.{1}", "Index файл", "index");
            if (openDic.ShowDialog() == DialogResult.OK)
            {
                var basePath = System.IO.Path.ChangeExtension(openDic.FileName, null);
                var indexFile = basePath + ".index";
                var baseFile = basePath + ".base";

                var header = Helper.ReadFromBinaryFile<SelfOrganizeIndexHeader>(openDic.FileName, 0);

                if (header.version == 0)
                {
                    Console.WriteLine("Incorrect file header");
                    MessageBox.Show("Incorrect file header");
                    return;
                }

                if (header.version != _version)
                {
                    Console.WriteLine("Old version,fix it");
                    MessageBox.Show("Old version, fix it");
                    return;
                }

                Console.WriteLine("Be patient!Operation may take some time,depending on index file");
                var lst = Helper.ReadFromBinaryFileList<SelfOrganizeIndexNode>(indexFile, header.nodesBeginOffset, 0, true);

                while (true)
                {
                    Console.WriteLine("Enter value to add, -1 to exit");
                    var data = Console.ReadLine();

                    if (data.Contains("-1"))
                    {
                        break;
                    }

                    Console.WriteLine("Wait...");
                    var hash = Helper.GetHash(data, _hashType);
                    var exist = lst.Where(x => x.hash == hash);
                    if (exist.Any())
                    {
                        Console.WriteLine("This value already exists");
                    }
                    else
                    {
                        var newNode = new SelfOrganizeFileNode() { text = data };
                        var lastNodePos = Helper.WriteToBinaryFile<SelfOrganizeFileNode>(baseFile, newNode, FileMode.Append);
                        var indexNode = new SelfOrganizeIndexNode()
                        {
                            hash = Helper.GetHash(data, _hashType),
                            len = System.Runtime.InteropServices.Marshal.SizeOf(newNode),
                            offset = lastNodePos
                        };
                        lst.Add(indexNode);
                        Helper.WriteToBinaryFile<SelfOrganizeIndexNode>(indexFile, indexNode, FileMode.Append);
                        header.count += 1;
                        Helper.WriteToBinaryFile<SelfOrganizeIndexHeader>(indexFile, header, FileMode.Open, 0);
                    }
                }
            }
        }

        void CheckCorrectness()
        {
            OpenFileDialog openDic = new OpenFileDialog();
            openDic.Multiselect = false;
            openDic.Filter = string.Format("{0} (*.{1})|*.{1}", "Index файл", "index");
            if (openDic.ShowDialog() == DialogResult.OK)
            {
                var basePath = System.IO.Path.ChangeExtension(openDic.FileName, null);
                var indexFile = basePath + ".index";
                var baseFile = basePath + ".base";

                var header = Helper.ReadFromBinaryFile<SelfOrganizeIndexHeader>(indexFile, 0);

                if (header.version == 0)
                {
                    Console.WriteLine("Incorrect file header");
                    MessageBox.Show("Incorrect file header");
                    return;
                }

                if (header.version != _version)
                {
                    Console.WriteLine("Old version,fix it");
                    MessageBox.Show("Old version, fix it");
                    return;
                }

                Console.WriteLine("Version:" + header.version + "; nodes count:" + header.count + "; hash type:" + header.hashType.ToString());

                var lst = Helper.ReadFromBinaryFileList<SelfOrganizeIndexNode>(indexFile, header.nodesBeginOffset, 10, true);

                if (header.count == lst.Count)
                {
                    Console.WriteLine("File looks like valid");
                    Console.WriteLine("10 last records");
                    var nodes = lst.Skip((int)(header.count - 10)).Take(10);
                    foreach (var node in nodes)
                    {
                        var record = Helper.ReadFromBinaryFile<SelfOrganizeFileNode>(baseFile, node.offset);
                        Console.WriteLine(node.offset + " - " + record.text);
                    }
                }
                else
                {
                    Console.WriteLine("Number of records missmatch!");
                }
            }
        }
    }
}