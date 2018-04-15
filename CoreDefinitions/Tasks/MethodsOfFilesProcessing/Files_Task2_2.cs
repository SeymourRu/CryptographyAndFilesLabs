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
    public class Files_Task2_2 : ITask<Files_Task2_2>, IBaseTask
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

        public Files_Task2_2()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
            _tree = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
            random = new Random();
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 2";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));
            var tmpbutt = BeautyfyForms.AddButton("Очистить дерево", new Point(0, 10), (o, k) =>
            {
                _tree = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
                treeViewer.Clear();
            });

            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(200, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 2. Параграф 6.2.2, алгоритм D (удаление узла дерева)");
            }));

            tmpbutt = BeautyfyForms.AddButton("Сгенерировать дерево (N элементное)", new Point(0, 40), (o, k) =>
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
                        });
					
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

            tmpbutt = BeautyfyForms.AddButton("Удалить значение", new Point(0, 70), (o, k) =>
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
                    //RemoveNode(res);
                    RemoveNodeRewritten(res);

                    if (_tree == null)
                    {
                        _tree = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
                    }

                    //funny but let`s skip this~
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
                    AddNewValue(key, default(SelfOrganizeIndexNode));
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
            _randomInput = BeautyfyForms.CreateTextBox(new Point(290, 43), false);
            _randomInput.Text = "20";
            form.Controls.Add(_randomInput);

            _randomMin = BeautyfyForms.CreateTextBox(new Point(400, 43), false);
            _randomMin.Text = "1";
            form.Controls.Add(_randomMin);

            _randomMax = BeautyfyForms.CreateTextBox(new Point(510, 43), false);
            _randomMax.Text = "100";
            form.Controls.Add(_randomMax);

            _singleInput = BeautyfyForms.CreateTextBox(new Point(150, 73), false);
            form.Controls.Add(_singleInput);

            treeViewer = BeautyfyForms.CreateMLTextBox(new Point(5, 250 + 5), 780, 200);
            form.Controls.Add(treeViewer);

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(150, 105), "Добавлять Х в качестве null-ветвей", true, 190));

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(330, 20), "N", true, 10));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(430, 20), "Min", true, 25));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(540, 20), "Max", true, 30));

            _addXToEnd = BeautyfyForms.CreateCheckBox(new Point(340, 100), false);
            form.Controls.Add(_addXToEnd);
        }

        private void AddNewValue(int key, SelfOrganizeIndexNode val)
        {
            var p = _tree;
            if (p.Key == null)
            {
                p.Key = key;
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

        private BinaryTree<int?, SelfOrganizeIndexNode> SearchValue(int key)
        {
            var p = _tree;
            if (p.Key == null)
            {
                throw new Exception("Деревце-то пустое!");
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
                    return p;
                }
            }

            throw new Exception("Значение " + key + " не найдено!");
        }

        private Branch SearchValue(int key, out BinaryTree<int?, SelfOrganizeIndexNode> upper, out BinaryTree<int?, SelfOrganizeIndexNode> value)
        {
            var p = _tree;
            BinaryTree<int?, SelfOrganizeIndexNode> parent = null;
            var res = Branch.Root;

            if (p.Key == null)
            {
                throw new Exception("Деревце-то пустое!");
            }

            while (true)
            {
                if (key < p.Key())
                {
                    if (p.LLink() != null)
                    {
                        parent = p;
                        p = p.LLink();
                        res = Branch.Left;
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
                        parent = p;
                        p = p.RLink();
                        res = Branch.Right;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                if (key == p.Key())
                {
                    upper = parent;
                    value = p;
                    return res;
                }
            }

            throw new Exception("Значение " + key + " не найдено!");
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
                    AddNewValue(newVal, default(SelfOrganizeIndexNode));
                }
            }
        }

        private void RemoveNode(int key)
        {
            try
            {
                BinaryTree<int?, SelfOrganizeIndexNode> upper, q;
                var pos = SearchValue(key, out upper, out q);
            D1:
                var t = q;
                if (t.RLink() == null)
                {
                    q = t.LLink();
                    goto D4;
                }
            D2:
                var r = t.RLink();
                if (r.LLink() == null)
                {
                    r.Left = t.LLink();
                    q = r;
                    goto D4;
                }
            D3:
                var s = r.LLink();
                while (s.LLink() != null)
                {
                    r = s;
                    s = r.LLink();
                }
                s.Left = t.LLink();
                r.Left = s.RLink();
                s.Right = t.RLink();
                q = s;
            D4:
                t = null;
                if (pos == Branch.Left)
                {
                    upper.Left = q;
                }
                else if (pos == Branch.Right)
                {
                    upper.Right = q;
                }
                else if (pos == Branch.Root)
                {
                    _tree = q;
                }
                else
                {
                    throw new Exception("WTF??!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveNodeRewritten(int key)
        {
            try
            {
                BinaryTree<int?, SelfOrganizeIndexNode> upper, q;
                var pos = SearchValue(key, out upper, out q);
                var t = q;

                while (true)
                {
                    if (t.RLink() == null)
                    {
                        q = t.LLink();
                        break;
                    }
                    var r = t.RLink();
                    if (r.LLink() == null)
                    {
                        r.Left = t.LLink();
                        q = r;
                        break;
                    }
                    var s = r.LLink();
                    while (s.LLink() != null)
                    {
                        r = s;
                        s = r.LLink();
                    }
                    s.Left = t.LLink();
                    r.Left = s.RLink();
                    s.Right = t.RLink();
                    q = s;
                    break;
                }

                t = null;
                if (pos == Branch.Left)
                {
                    upper.Left = q;
                }
                else if (pos == Branch.Right)
                {
                    upper.Right = q;
                }
                else if (pos == Branch.Root)
                {
                    _tree = q;
                }
                else
                {
                    throw new Exception("WTF??!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                AddNewValue((int)val.offset, val);
            }
            return true;
        }

    }
}