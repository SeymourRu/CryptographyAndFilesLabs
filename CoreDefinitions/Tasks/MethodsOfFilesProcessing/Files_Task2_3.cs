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
    public class Files_Task2_3 : ITask<Files_Task2_3>, IBaseTask
    {
        TaskAppType _subSystemType;
        BinaryTree<int?, SelfOrganizeIndexNode> _head;
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

        public Files_Task2_3()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
            _tree = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
            _head = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
            _head.Right = _tree;
            _head.Left = new BinaryTree<int?, SelfOrganizeIndexNode>(0, default(SelfOrganizeIndexNode));
            random = new Random();
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 3";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));
            var tmpbutt = BeautyfyForms.AddButton("Очистить дерево", new Point(0, 10), (o, k) =>
            {
                _tree = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
                _head = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
                _head.Right = _tree;
                _head.Left = new BinaryTree<int?, SelfOrganizeIndexNode>(0, default(SelfOrganizeIndexNode));
                treeViewer.Clear();
            });

            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(200, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 3. Параграф 6.2.3, алгоритм A (поиск со вставкой по сбалансированному дереву)");
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
                        _head = new BinaryTree<int?, SelfOrganizeIndexNode>(null, default(SelfOrganizeIndexNode));
                        _head.Right = _tree;
                        _head.Left = new BinaryTree<int?, SelfOrganizeIndexNode>(0, default(SelfOrganizeIndexNode));
                        GenerateRandomTree(res, min, max);
                    }).ContinueWith(result =>
                    {
                        treeViewer.BeginInvoke(new MethodInvoker(() => treeViewer.Text = (_tree != null) ? _tree.getTreeViewPlus(_addXToEnd.Checked) : ""));
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

            tmpbutt = BeautyfyForms.AddButton("Добавить значение", new Point(0, 70), (o, k) =>
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
                    AddNewValueAndBalance(res, default(SelfOrganizeIndexNode));
                    _tree = _head.Right;
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
                treeViewer.Text = (_tree != null) ? _tree.getTreeViewPlus(_addXToEnd.Checked) : "";
            });
            _buttonsToHide.Add(tmpbutt);
            form.Controls.Add(tmpbutt);

            tmpbutt = BeautyfyForms.AddButton(" Импорт ", new Point(0, 140), (o, k) =>
            {
                var keys = new List<int>();
                Helper.LoadFile("Список ключей", "keylst", keys);

                foreach (var key in keys)
                {
                    AddNewValueAndBalance(key, default(SelfOrganizeIndexNode));
                }

                _tree = _head.Right;
                treeViewer.Text = (_tree != null) ? _tree.getTreeViewPlus(_addXToEnd.Checked) : "";
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
                        System.IO.File.WriteAllText(saveFile.FileName, _tree.getTreeViewPlus(_addXToEnd.Checked, false));
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

        private void AddNewValueAndBalance(int key, SelfOrganizeIndexNode val)
        {
            //Work with 1st insert
            var init = _tree;
            if (init.Key == null)
            {
                init.Key = key;
                init.Value = val;
                init.Balance = 0;
                _head.Left.Key = _head.Left.Key + 1;//!!
                return;
            }

            var a = 0xFF;//Why not then?
        A1:
            var T = _head;
            var P = _head.RLink();
            var S = P;
        A2:
            if (key < P.Key)
            {
                goto A3;
            }
            if (key > P.Key)
            {
                goto A4;
            }
            if (key == P.Key)
            {
                MessageBox.Show("Ключ " + key + " уже присутствует в дереве");
                return;
            }
        A3:
            var Q = P.LLink();
            if (Q == null)
            {
                Q = _tree.NewNode(null, default(SelfOrganizeIndexNode));
                P.Left = Q;
                goto A5;
            }
            else
            {
                if (Q.Balance != 0)
                {
                    T = P;
                    S = Q;
                }
                P = Q;
                goto A2;
            }

        A4:
            Q = P.RLink();
            if (Q == null)
            {
                Q = _tree.NewNode(null, default(SelfOrganizeIndexNode));
                P.Right = Q;
                goto A5;
            }
            else
            {
                if (Q.Balance != 0)
                {
                    T = P;
                    S = Q;
                }
                P = Q;
                goto A2;
            }
        A5:
            Q.Key = key;
            Q.Value = val;//custom
            Q.Left = Q.Right = null;
            Q.Balance = 0;
        A6:
            if (key < S.Key)
            {
                a = -1;
            }
            else
            {
                a = 1;
            }

            var R = P = S.GetByA(a);

            while (P.Key != Q.Key)
            {
                if (key < P.Key)
                {
                    P.Balance = -1;
                    P = P.LLink();
                }
                else if (key > P.Key)
                {
                    P.Balance = 1;
                    P = P.RLink();
                }
                else //(key == P.Key)
                {
                    P = Q;
                    break;
                }
            }
        A7:
            //i
            if (S.Balance == 0)
            {
                S.Balance = a;
                _head.Left.Key = _head.Left.Key + 1;//!!
                return;
            }
            //ii
            if (S.Balance == -a)
            {
                S.Balance = 0;
                return;
            }
            //iii
            if (S.Balance == a)
            {
                if (R.Balance == a)
                {
                    goto A8;
                }
                if (R.Balance == -a)
                {
                    goto A9;
                }
                throw new Exception("?!?!?!");
            }
            return;
        A8:
            P = R;
            S.SetByA(a, R.GetByA(-a));
            R.SetByA(-a, S);
            S.Balance = R.Balance = 0;
            goto A10;
        A9:
            P = R.GetByA(-a);
            R.SetByA(-a, P.GetByA(a));
            P.SetByA(a, R);
            S.SetByA(a, P.GetByA(-a));
            P.SetByA(-a, S);

            if (P.Balance == a)
            {
                S.Balance = -a;
                R.Balance = 0;
            }
            else if (P.Balance == 0)
            {
                S.Balance = 0;
                R.Balance = 0;
            }
            else if (P.Balance == -a)
            {
                S.Balance = 0;
                R.Balance = a;
            }
            else
            {
                throw new Exception("?!?!?!?!?!?!?");
            }

            P.Balance = 0;
        A10:
            if (S == T.RLink()) // S.Key ..
            {
                T.Right = P;
            }
            else
            {
                T.Left = P;
            }
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

            _tree = _head.Right;
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

            _tree = _head.Right;
            return true;
        }
    }
}