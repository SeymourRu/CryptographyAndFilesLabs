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
        BinaryTree<int?> _tree;
        Random random;

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
            _tree = new BinaryTree<int?>(null);
            random = new Random();
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 2";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));
            form.Controls.Add(BeautyfyForms.AddButton("Очистить дерево", new Point(0, 10), (o, k) =>
            {
                _tree = new BinaryTree<int?>(null);
                treeViewer.Clear();
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(200, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 2. Параграф 6.2.2, алгоритм D (удаление узла дерева)");
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Сгенерировать дерево (N элементное)", new Point(0, 40), (o, k) =>
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
                    _tree = new BinaryTree<int?>(null);
                    treeViewer.Clear();
                    if (Math.Abs(min - max) < res)
                    {
                        MessageBox.Show("Cлющай, став нармалные пределы, окда?");
                        return;
                    }

                    GenerateRandomTree(res, min, max);
                    treeViewer.Text = (_tree != null) ? _tree.getTreeView(_addXToEnd.Checked) : "";
                }
                else
                {
                    MessageBox.Show("Это было не число, да?..");
                    return;
                }
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Удалить значение", new Point(0, 70), (o, k) =>
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
                        _tree = new BinaryTree<int?>(null);
                    }

                    //funny but let`si skip this~
                    treeViewer.Text = (_tree != null) ? _tree.getTreeView(_addXToEnd.Checked) : "";
                }
                else
                {
                    MessageBox.Show("Это было не число, да?..");
                    return;
                }

            }));

            form.Controls.Add(BeautyfyForms.AddButton("Отобразить деревце", new Point(0, 100), (o, k) =>
            {
                treeViewer.Text = (_tree != null) ? _tree.getTreeView(_addXToEnd.Checked) : "";
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Импорт ", new Point(0, 140), (o, k) =>
            {
                var keys = new List<int>();
                Helper.LoadFile("Список ключей", "keylst", keys);

                foreach (var key in keys)
                {
                    AddNewValue(key);
                }

                treeViewer.Text = (_tree != null) ? _tree.getTreeView(_addXToEnd.Checked) : "";
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Экспорт ", new Point(80, 140), (o, k) =>
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = string.Format("{0} (*.{1})|*.{1}", "Бинарное деревце", "btree");
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(saveFile.FileName, _tree.getTreeView(_addXToEnd.Checked));
                }
            }));

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

        private void AddNewValue(int key)
        {
            var p = _tree;
            if (p.Value == null)
            {
                p.Value = key;
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

            var q = _tree.NewNode(key);
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

        private BinaryTree<int?> SearchValue(int key)
        {
            var p = _tree;
            if (p.Value == null)
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

        private Branch SearchValue(int key, out BinaryTree<int?> upper, out BinaryTree<int?> value)
        {
            var p = _tree;
            BinaryTree<int?> parent = null;
            var res = Branch.Root;

            if (p.Value == null)
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
                    AddNewValue(newVal);
                }
            }
        }

        private void RemoveNode(int key)
        {
            try
            {
                BinaryTree<int?> upper, q;
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
                BinaryTree<int?> upper, q;
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
    }
}