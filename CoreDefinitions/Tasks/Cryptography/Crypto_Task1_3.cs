using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;

namespace CoreDefinitions.Tasks
{
    public class Crypto_Task1_3 : ITask<Crypto_Task1_3>, IBaseTask
    {
        TaskAppType _subSystemType;
        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }


        DataGridView dgv;
        ListBox logLstBox;
        List<int> multiplicity = new List<int>();
        List<int> newMultiplicity = new List<int>();

        public Crypto_Task1_3()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 3";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));
            form.Controls.Add(BeautyfyForms.AddButton("Открыть файл множества", new Point(0, 10), (o, k) =>
            {
                Helper.LoadFile("Файл множества", "mul", multiplicity);
                dgv.ColumnCount = multiplicity.Count;
                for (int i = 0; i < multiplicity.Count; i++)
                {
                    dgv.Columns[i].HeaderCell.Value = (i + 1).ToString();
                    dgv.Columns[i].ReadOnly = true;

                    dgv.Rows[0].Cells[i].Value = multiplicity.ElementAt(i);
                }
                logLstBox.Items.Add(DateTime.Now.ToString() + ": Заполнили DGV множеством");
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(250, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 3. Теорема: любое множество можно представить с помощью разложения в произведения транспозиций. Попробуем найти циклическую перестановку и построить транспозицию.");
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Открыть файл нового множества", new Point(0, 40), (o, k) =>
            {
                Helper.LoadFile("Файл нового множества", "mul", newMultiplicity);
                dgv.ColumnCount = newMultiplicity.Count;
                for (int i = 0; i < newMultiplicity.Count; i++)
                {
                    dgv.Rows[1].Cells[i].Value = newMultiplicity.ElementAt(i);
                }
                logLstBox.Items.Add(DateTime.Now.ToString() + ": Заполнили DGV множеством *");
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Найти транспозиции", new Point(0, 70), (o, k) =>
            {
                ProcessTranspos();
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Построить циклическую перестановку", new Point(150, 70), (o, k) =>
            {
                ProcessCycles();
            }));

            dgv = BeautyfyForms.AddDataGridViewHeadersOnLeft(new Point(0, 100), form.Size, 3, new string[] { "Множество", "Множество *" });
            form.Controls.Add(dgv);

            logLstBox = BeautyfyForms.AddListBox(new Point(0, 100 + dgv.Height + 5), form.Size);
            form.Controls.Add(logLstBox);
        }

        void ProcessTranspos()
        {
            var diff = multiplicity.Where(n => !newMultiplicity.Select(n1 => n1).Contains(n));
            if (diff.Any())
            {
                MessageBox.Show("Множества не эквивалентны");
                return;
            }

            logLstBox.Items.Add(DateTime.Now.ToString() + ": Составляем карту перемещений от элемента к элементу");
            var pathDic = new Dictionary<int, int>();
            for (int i = 0; i < multiplicity.Count; i++)
            {
                if (!pathDic.ContainsKey(multiplicity.ElementAt(i)))
                {
                    pathDic.Add(multiplicity.ElementAt(i), newMultiplicity.ElementAt(i));
                }
            }

            logLstBox.Items.Add(DateTime.Now.ToString() + ": Находим транспозиции");

            int position = 0; //Current location
            var chain = new List<int>();//Current chain
            var usedElemnts = new HashSet<int>();//Current chain
            var result = new List<List<int>>();//List of chains
            var multiplicityCopy = new List<int>(multiplicity);//Copy of elements
            while (multiplicityCopy.Count > 0)
            {
                var key = multiplicityCopy.ElementAt(position);
                var val = pathDic[multiplicityCopy.ElementAt(position)];

                chain.Add(key);
                chain.Add(val);

                usedElemnts.Add(key);

                multiplicityCopy.RemoveAt(position);
                pathDic.Remove(key);

                if (usedElemnts.Contains(val))
                {
                    position = 0;
                }
                else
                {
                    position = multiplicityCopy.FindIndex(0, multiplicityCopy.Count, x => x == val);
                }

                result.Add(chain);
                chain = new List<int>();
            }


            logLstBox.Items.Add(DateTime.Now.ToString() + ": Транспозиции найдены");

            var strBuild = new StringBuilder();
            foreach (var lst in result)
            {
                var template = "(";
                foreach (var item in lst)
                {
                    template += item;
                }
                template += ")";
                strBuild.Append(template);
            }

            logLstBox.Items.Add(DateTime.Now.ToString() + ": " + strBuild.ToString());
            logLstBox.Items.Add(DateTime.Now.ToString() + ": Готово");
        }

        void ProcessCycles()
        {
            var diff = multiplicity.Where(n => !newMultiplicity.Select(n1 => n1).Contains(n));
            if (diff.Any())
            {
                MessageBox.Show("Множества не эквивалентны");
                return;
            }

            logLstBox.Items.Add(DateTime.Now.ToString() + ": Составляем карту перемещений от элемента к элементу");
            var pathDic = new Dictionary<int, int>();
            for (int i = 0; i < multiplicity.Count; i++)
            {
                if (!pathDic.ContainsKey(multiplicity.ElementAt(i)))
                {
                    pathDic.Add(multiplicity.ElementAt(i), newMultiplicity.ElementAt(i));
                }
            }

            logLstBox.Items.Add(DateTime.Now.ToString() + ": Ищем цикл");

            int position = 0; //Current location
            var chain = new List<int>();//Current chain
            var toDel = new List<int>();//List of current index for removing
            var result = new List<List<int>>();//List of chains
            var multiplicityCopy = new List<int>(multiplicity);//Copy of elements
            while (multiplicityCopy.Count > 0)
            {

                /*
                var key = multiplicityCopy.ElementAt(position);
                var val = pathDic[multiplicityCopy.ElementAt(position)];

                chain.Add(key);
                usedElemnts.Add(key);
                multiplicityCopy.RemoveAt(position);
                pathDic.Remove(key);

                if (usedElemnts.Contains(val))
                {
                    preResult.Add(chain);
                    chain = new List<int>();
                    position = 0;
                }
                else
                {
                    position = multiplicityCopy.FindIndex(0, multiplicityCopy.Count, x => x == val);
                }
                */


                var key = multiplicityCopy.ElementAt(position);
                var val = pathDic[multiplicityCopy.ElementAt(position)];

                if (!chain.Contains(key))
                {
                    chain.Add(key);
                }

                toDel.Add(position);

                if (chain.Contains(val))
                {
                    result.Add(chain);
                    chain = new List<int>();
                    toDel = toDel.OrderByDescending(x => x).ToList();
                    foreach( var id in toDel)
                    {
                        multiplicityCopy.RemoveAt(id);
                    }
                    toDel.Clear();
                    position = 0;
                }
                else
                {
                    chain.Add(val);
                    pathDic.Remove(key);
                    position = multiplicityCopy.FindIndex(0, multiplicityCopy.Count, x => x == val);
                }
            }

            logLstBox.Items.Add(DateTime.Now.ToString() + ": Цикл найден");

            var strBuild = new StringBuilder();
            foreach (var lst in result)
            {
                var template = "(";
                foreach(var item in lst)
                {
                    template += "|" + item + "|";
                }
                template += ")";
                strBuild.Append(template);
            }

            logLstBox.Items.Add(DateTime.Now.ToString() + ": " + strBuild.ToString());
            logLstBox.Items.Add(DateTime.Now.ToString() + ": Готово");
        }
    }
}
