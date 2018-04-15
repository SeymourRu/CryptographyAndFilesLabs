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
    public class Files_Task2_1 : ITask<Files_Task2_1>, IBaseTask
    {
        TaskAppType _subSystemType;
        BinaryTree<int?, SelfOrganizeIndexNode> _tree;
        Random random;

        List<Button> _buttonsToHide = new List<Button>();
        Timer _timerProgress;
        ProgressBar _progress;
        TextBox _singleInput;
        TextBox _randomInput;
        TextBox _randomMin;
        TextBox _randomMax;
        TextBox treeViewer;
        CheckBox _addXToEnd;

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Files_Task2_1()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
            _tree = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
            random = new Random();
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 1";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));
            var tmpbutt = BeautyfyForms.AddButton("Очистить дерево", new Point(0, 10), (o, k) =>
            {
                _tree = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
                treeViewer.Clear();
            });

            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(250, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 1. Параграф 6.2.2, алгоритм Т (поиск по дереву со вставкой)");
            }));

            tmpbutt = BeautyfyForms.AddButton("Добавить значение", new Point(0, 40), (o, k) =>
            {
                if (string.IsNullOrEmpty(_singleInput.Text))
                {
                    MessageBox.Show("Нет текста для поиска");
                    return;
                }

                var text = _singleInput.Text;
                int res;

                if (int.TryParse(text, out res))
                {
                    //AddNewValue(res);
                    AddNewValueRewritten(res, default(SelfOrganizeIndexNode));
                    treeViewer.Text = (_tree != null) ? _tree.getTreeView(_addXToEnd.Checked) : "";
                }
                else
                {
                    MessageBox.Show("Это было не число, да?..");
                    return;
                }

            });
            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            tmpbutt = BeautyfyForms.AddButton("Сгенерировать дерево (N элементное)", new Point(0, 70), (o, k) =>
            {
                if (string.IsNullOrEmpty(_randomInput.Text))
                {
                    MessageBox.Show("Введите кол-во элементов для добавления");
                    return;
                }

                if (string.IsNullOrEmpty(_randomMin.Text))
                {
                    MessageBox.Show("Введите минимальное число");
                    return;
                }

                if (string.IsNullOrEmpty(_randomMax.Text))
                {
                    MessageBox.Show("Введите максимальное число");
                    return;
                }

                int min = 0, max = 0;
                try
                {
                    min = int.Parse(_randomMin.Text);
                    max = int.Parse(_randomMax.Text);
                }
                catch
                {
                    MessageBox.Show("Пределы не валидны");
                    return;
                }

                var text = _randomInput.Text;
                int res;

                if (int.TryParse(text, out res))
                {
                    if (Math.Abs(min - max) < res)
                    {
                        MessageBox.Show("Cлющай, став нармалные пределы, окда?");
                        return;
                    }

                    _buttonsToHide.ForEach(x => x.Enabled = false);
                    _timerProgress.Start();
                    treeViewer.Clear();

                    Task.Run(() =>
                        {   
                            _tree = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
                            GenerateRandomTree(res, min, max);
                        }).ContinueWith(result =>
                        {
                            treeViewer.BeginInvoke(new MethodInvoker(() => treeViewer.Text = (_tree != null) ? _tree.getTreeView(_addXToEnd.Checked) : ""));
                        }); ;

                    _buttonsToHide.ForEach(x => x.Enabled = true);
                    _timerProgress.Stop();
                    _progress.Value = _progress.Maximum;
                    MessageBox.Show("Сгенерили!");
                }
                else
                {
                    MessageBox.Show("Это было не число, да?..");
                    return;
                }
            });

            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            tmpbutt = BeautyfyForms.AddButton("Отобразить деревце", new Point(0, 100), (o, k) =>
            {
                treeViewer.Text = (_tree != null) ? _tree.getTreeView(_addXToEnd.Checked) : "";
            });

            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            tmpbutt = BeautyfyForms.AddButton(" Импорт ", new Point(0, 140), (o, k) =>
            {
                var keys = new List<int>();
                Helper.LoadFile("Список ключей", "keylst", keys);

                foreach (var key in keys)
                {
                    AddNewValueRewritten(key, default(SelfOrganizeIndexNode));
                }

                treeViewer.Text = (_tree != null) ? _tree.getTreeView(_addXToEnd.Checked) : "";
            });
            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            tmpbutt = BeautyfyForms.AddButton(" Экспорт ", new Point(80, 140), (o, k) =>
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = string.Format("{0} (*.{1})|*.{1}", "Бинарное деревце", "btree");
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

            tmpbutt = BeautyfyForms.AddButton(" Импорт .index ", new Point(160, 140), async (o, k) =>
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

            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            _progress = BeautyfyForms.AddProgressBar(new Point(300, 140), form.Size, 0, 10);
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

            _singleInput = BeautyfyForms.CreateTextBox(new Point(150, 43), false);
            form.Controls.Add(_singleInput);

            _randomInput = BeautyfyForms.CreateTextBox(new Point(290, 73), false);
            _randomInput.Text = "20";
            form.Controls.Add(_randomInput);

            _randomMin = BeautyfyForms.CreateTextBox(new Point(400, 73), false);
            _randomMin.Text = "1";
            form.Controls.Add(_randomMin);

            _randomMax = BeautyfyForms.CreateTextBox(new Point(510, 73), false);
            _randomMax.Text = "100";
            form.Controls.Add(_randomMax);

            treeViewer = BeautyfyForms.CreateMLTextBox(new Point(5, 250 + 5), 780, 200);
            form.Controls.Add(treeViewer);

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(150, 105), "Добавлять Х в качестве null-ветвей", true, 190));

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(330, 50), "N", true, 10));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(430, 50), "Min", true, 25));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(540, 50), "Max", true, 30));

            _addXToEnd = BeautyfyForms.CreateCheckBox(new Point(340, 100), false);
            form.Controls.Add(_addXToEnd);

        }

        private void AddNewValue(int key, SelfOrganizeIndexNode val)
        {
            //T1
            var p = _tree;
            //T1.5
            if (p.Key == null)
            {
                p.Key = key;
                p.Value = val;
                return;
            }

            //T2
        T2:
            if (key < p.Key())
            {
                goto T3;
            }

            if (key > p.Key())
            {
                goto T4;
            }

            if (key == p.Key())
            {
                //success

                return;
            }

            //T3
        T3:
            if (p.LLink() != null)
            {
                p = p.LLink();
                goto T2;
            }
            else
            {
                goto T5;
            }

            //T4
        T4:
            if (p.RLink() != null)
            {
                p = p.RLink();
                goto T2;
            }
            else
            {
                goto T5;
            }

            //T5
        T5:
            //Set key in constructor
            var q = _tree.NewNode(key, val);

            //Will be set automatically
            //LLink(q) = null;
            //RLink(q) = null;

            if (key < p.Key())
            {
                p.Left = q;
            }
            else
            {
                p.Right = q;
            }

            p = q;
            return;
        }

        private void AddNewValueRewritten(int key, SelfOrganizeIndexNode val)
        {
            var p = _tree;
            if (p.Key == null)
            {
                p.Key = key;
                p.Value = val;
                return;
            }

            while (true)
            {
                if (key < p.Key())
                {
                    if (p.LLink() != null)
                    {
                        p = p.LLink();
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                if (key > p.Key())
                {
                    if (p.RLink() != null)
                    {
                        p = p.RLink();
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                if (key == p.Key())
                {
                    //success, but we already have this key,so...
                    return;
                }
            }

            var q = _tree.NewNode(key, val);
            if (key < p.Key())
            {
                p.Left = q;
            }
            else
            {
                p.Right = q;
            }

            p = q;
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
                    //AddNewValue(newVal);
                    AddNewValueRewritten(newVal, default(SelfOrganizeIndexNode));
                }
            }
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
                AddNewValueRewritten((int)val.offset, val);
            }
            return true;
        }
    }
}