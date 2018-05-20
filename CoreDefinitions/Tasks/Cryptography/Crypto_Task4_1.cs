using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;
using System.Windows.Forms;
using System.Drawing;
using System.Numerics;

namespace CoreDefinitions.Tasks
{
    public class Crypto_Task4_1 : ITask<Crypto_Task4_1>, IBaseTask
    {
        TaskAppType _subSystemType;

        TextBox _inputValue;
        TextBox _inputMod;
        Button _addNew;
        Button _acceptFunction;
        Button _selectNewFunction;
        TreeView _treeOfData;

        int curValue = 0;
        Dictionary<int, List<int>> chains = new Dictionary<int, List<int>>();

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Crypto_Task4_1()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 1";
            form.SetDefaultVals(new System.Drawing.Size(810, 500));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(300, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 1 Алгоритм разрешения коллизий в хэш-таблицах методом цепочек.");
            }));

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(370, 46), "Хеширующая функция", true, 125));

            _inputValue = BeautyfyForms.CreateTextBox(new Point(300, 66), false);
            form.Controls.Add(_inputValue);

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(420, 66), "mod", true, 35));

            _inputMod = BeautyfyForms.CreateTextBox(new Point(460, 66), false);
            form.Controls.Add(_inputMod);

            _acceptFunction = BeautyfyForms.AddButton(" Подтвердить выбор ", new Point(620, 33), (o, k) =>
            {
                var value = 0;
                if (int.TryParse(_inputMod.Text, out value))
                {
                    curValue = value;
                    _inputMod.ReadOnly = true;
                    _addNew.Visible = true;
                }
                else
                {
                    MessageBox.Show("Invalid mod value");
                }
            });
            form.Controls.Add(_acceptFunction);

            _selectNewFunction = BeautyfyForms.AddButton("Выбрать другую функцию", new Point(615, 66), (o, k) =>
            {
                _inputMod.ReadOnly = false;
                _addNew.Visible = false;
                _treeOfData.Nodes.Clear();
                chains.Clear();

            });
            form.Controls.Add(_selectNewFunction);

            _addNew = BeautyfyForms.AddButton(" Добавить ", new Point(380, 99), (o, k) =>
            {
                var value = 0;
                if (int.TryParse(_inputValue.Text, out value))
                {
                    var hash = value % curValue;
                    if (chains.ContainsKey(hash))
                    {
                        if (!(chains[hash].Contains(value)))
                        {
                            chains[hash].Add(value);
                            var node = _treeOfData.Nodes.Find("", true).FirstOrDefault(x => x.Text == hash.ToString() && x.Parent == null);
                            if (node != null)
                            {
                                node.Nodes.Add(new TreeNode(value.ToString()));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ключ найден - " + hash);
                        }
                    }
                    else
                    {
                        chains.Add(hash, new List<int>() { value });

                        var stringHash = hash.ToString();

                        var hashNode = new TreeNode(stringHash);

                        hashNode.Text = stringHash;
                        hashNode.ImageKey = stringHash;
                        hashNode.Tag = stringHash;

                        var valueNode = new TreeNode(value.ToString());
                        hashNode.Nodes.Add(valueNode);

                        _treeOfData.Nodes.Add(hashNode);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid x value");
                }
            });
            _addNew.Visible = false;
            form.Controls.Add(_addNew);

            _treeOfData = BeautyfyForms.AddTreeView(new Point(10, 10), new Size(200, 450));
            form.Controls.Add(_treeOfData);
        }
    }
}
