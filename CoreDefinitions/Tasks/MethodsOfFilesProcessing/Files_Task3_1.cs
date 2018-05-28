using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;
using System.Windows.Forms;

namespace CoreDefinitions.Tasks
{
    public class Files_Task3_1 : ITask<Files_Task3_1>, IBaseTask
    {
        TaskAppType _subSystemType;
        //BinaryTree<int?, SelfOrganizeIndexNode> _head;
        //BinaryTree<int?, SelfOrganizeIndexNode> _tree;
        Random random;

        Trie _tree;

        List<Button> _buttonsToHide = new List<Button>();
        Timer _timerProgress;
        ProgressBar _progress;
        TextBox _addInput;
        TextBox _deleteInput;
        TextBox _checkInput;
        ListBox _addedWords;
        TextBox treeViewer;
        CheckBox _addXToEnd;

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Files_Task3_1()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
            //_tree = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
            //_head = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
            //_head.Right = _tree;
            //_head.Left = new BinaryTree<int?, SelfOrganizeIndexNode>(0, default(SelfOrganizeIndexNode));
            //_tree = new BranchingTree(new List<char>() { 'a', 'b', 'c', 'd', 'e' }, 10);
            _tree = new Trie();

            ///*
            _tree.AddWord("Kura");
            _tree.AddWord("Kuraga");
            _tree.AddWord("Kuraja");
            _tree.AddWord("Kuraza");
            _tree.AddWord("Kuroko");
            _tree.AddWord("Aura");
            _tree.AddWord("Kurona");
            _tree.AddWord("Kurara");
            _tree.AddWord("Kuragada");
            //var test1 = _tree.WordExists("Kurenai");
            //var test2 = _tree.WordExists("Kura");

            //*/

            random = new Random();
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 1";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));
            var tmpbutt = BeautyfyForms.AddButton("Очистить дерево", new Point(0, 10), (o, k) =>
            {
                //_tree = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
                //_head = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
                //_head.Right = _tree;
                //_head.Left = new BinaryTree<int?, SelfOrganizeIndexNode>(0, default(SelfOrganizeIndexNode));
                _tree = new Trie();
                treeViewer.Clear();
            });

            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(200, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 1. Параграф 6.3, алгоритм T (лучевой поиск)");
            }));

            tmpbutt = BeautyfyForms.AddButton("Добавить значение", new Point(0, 40), (o, k) =>
            {
                if (string.IsNullOrEmpty(_addInput.Text))
                {
                    MessageBox.Show("Нет текста для поиска");
                    return;
                }

                var text = _addInput.Text;
                if (_tree.AddWord(text))
                {
                    if (!_addedWords.Items.Contains(text))
                    {
                        _addedWords.Items.Add(text);
                    }
                }
            });

            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            tmpbutt = BeautyfyForms.AddButton("Удалить значение", new Point(0, 70), (o, k) =>
            {
                if (string.IsNullOrEmpty(_deleteInput.Text))
                {
                    MessageBox.Show("Нет текста для поиска");
                    return;
                }

                var text = _deleteInput.Text;
                _tree.DeleteWord(text);
                _addedWords.Items.Remove(text);
            });

            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            tmpbutt = BeautyfyForms.AddButton("Проверить значение", new Point(0, 100), (o, k) =>
            {
                if (string.IsNullOrEmpty(_checkInput.Text))
                {
                    MessageBox.Show("Нет текста для поиска");
                    return;
                }

                var text = _checkInput.Text;
                if (_tree.WordExists(text))
                {
                    MessageBox.Show("Существует");
                }
                else
                {
                    MessageBox.Show("Не существует");
                }
            });

            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            tmpbutt = BeautyfyForms.AddButton("Отобразить деревце", new Point(0, 140), (o, k) =>
            {
                treeViewer.Text = (_tree != null) ? _tree.getTreeView(_addXToEnd.Checked) : "";
            });
            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            tmpbutt = BeautyfyForms.AddButton(" Импорт ", new Point(0, 180), (o, k) =>
            {
                var keys = new List<string>();
                Helper.LoadFile("Список слов", "wordlst", keys);

                foreach (var key in keys)
                {
                    if (_tree.AddWord(key))
                    {
                        if (!_addedWords.Items.Contains(key))
                        {
                            _addedWords.Items.Add(key);
                        }
                    }
                }

                treeViewer.Text = (_tree != null) ? _tree.getTreeView(_addXToEnd.Checked) : "";
            });
            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            tmpbutt = BeautyfyForms.AddButton(" Экспорт ", new Point(80, 180), (o, k) =>
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = string.Format("{0} (*.{1})|*.{1}", "Луч", "trieview");
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    _buttonsToHide.ForEach(x => x.Enabled = false);
                    _timerProgress.Start();

                    Task.Run(() =>
                    {
                        System.IO.File.WriteAllText(saveFile.FileName, _tree.getTreeView(_addXToEnd.Checked, false));
                    });
                    _buttonsToHide.ForEach(x => x.Enabled = true);
                    _timerProgress.Stop();
                    _progress.Value = _progress.Maximum;
                    MessageBox.Show("Сохранено!");
                }
            });
            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            tmpbutt = BeautyfyForms.AddButton(" Импорт .index ", new Point(160, 180), async (o, k) =>
            {
                OpenFileDialog openDic = new OpenFileDialog();
                openDic.Multiselect = false;
                openDic.Filter = string.Format("{0} (*.{1})|*.{1}", "Index файл", "index");
                if (openDic.ShowDialog() == DialogResult.OK)
                {
                    _buttonsToHide.ForEach(x => x.Enabled = false);
                    var openedFile = System.IO.Path.ChangeExtension(openDic.FileName, null);
                    var indexFile = openedFile + ".index";
                    var baseFile = openedFile + ".base";
                    _timerProgress.Start();
                    var state = await Task.Run(() => LoadIndexFile(indexFile, baseFile));

                    if (state != false)
                    {
                        MessageBox.Show("Готово! Дерево загружено.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка загрузки дерева.");
                        return;
                    }

                    _timerProgress.Stop();
                    _progress.Value = _progress.Maximum;
                    _buttonsToHide.ForEach(x => x.Enabled = true);
                }
            });

            //_buttonsToHide.Add(tmpbutt);
            //form.Controls.Add(tmpbutt);

            _progress = BeautyfyForms.AddProgressBar(new Point(300, 180), form.Size, 0, 10);
            form.Controls.Add(_progress);

            _timerProgress = BeautyfyForms.CreateTimer((sender, EventArgs) =>
            {
                if (_progress.Value >= _progress.Maximum)
                {
                    _progress.Value = 0;
                }
                else
                {
                    _progress.Value = _progress.Value + 1;
                }
            });

            _addInput = BeautyfyForms.CreateTextBox(new Point(150, 43), false);
            form.Controls.Add(_addInput);

            _deleteInput = BeautyfyForms.CreateTextBox(new Point(150, 73), false);
            form.Controls.Add(_deleteInput);

            _checkInput = BeautyfyForms.CreateTextBox(new Point(150, 103), false);
            form.Controls.Add(_checkInput);

            treeViewer = BeautyfyForms.CreateMLTextBox(new Point(5, 250 + 5), 780, 200);
            form.Controls.Add(treeViewer);

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(150, 145), "Добавлять Х в качестве null-ветвей", true, 190));

            _addXToEnd = BeautyfyForms.CreateCheckBox(new Point(340, 140), false);
            form.Controls.Add(_addXToEnd);

            _addedWords = BeautyfyForms.AddListBox(new Point(500, 20), new Size(200, 250));
            _addedWords.MouseDoubleClick += (x, y) =>
            {
                if (_addedWords.SelectedItem != null)
                {
                    MessageBox.Show(_addedWords.SelectedItem.ToString());
                }
            };
            form.Controls.Add(_addedWords);

        }

        private void AddNewValueAndBalance(int key, SelfOrganizeIndexNode val)
        {
            return;
        }

        private void GenerateRandomTree(int elementNum, int min, int max)
        {
            var uniqVals = new HashSet<int>();
            for (int i = 0; i < elementNum; i++)
            {
                var newVal = random.Next(min, max);
                if (uniqVals.Contains(newVal))
                {
                    i--;
                }
                else
                {
                    uniqVals.Add(newVal);
                    AddNewValueAndBalance(newVal, default(SelfOrganizeIndexNode));
                }
            }

            //_tree = _head.Right;
        }

        async Task<bool> LoadIndexFile(string indexFile, string baseFile)
        {
            var _header = Helper.ReadFromBinaryFile<SelfOrganizeIndexHeader>(indexFile, 0);
            if (_header.version == 0)
            {
                MessageBox.Show("Ошибка чтения заголовка");
                return false;
            }

            MessageBox.Show("Заголовок успешно прочитан. Загрузка index файла займёт некоторое время, пожалуйста, подождите");
            var List = Helper.ReadFromBinaryFileList<SelfOrganizeIndexNode>(indexFile, _header.nodesBeginOffset, 0, true);

            if (List != null)
            {
                MessageBox.Show("index файл загружен.Строим дерево");
            }
            else
            {
                MessageBox.Show("ошибка загрузки");
                return false;
            }

            //Mix it, so that tree will be at least semi-balanced
            List.Shuffle();

            foreach (var val in List)
            {
                AddNewValueAndBalance((int)val.offset, val);
            }

            //_tree = _head.Right;
            return true;
        }

    }
}
