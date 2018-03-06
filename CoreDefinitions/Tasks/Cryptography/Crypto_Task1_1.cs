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
    public class Crypto_Task1_1 : ITask<Crypto_Task1_1>, IBaseTask
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
        List<Node> multiplicity = new List<Node>();
        List<int> reshuffle = new List<int>();

        public Crypto_Task1_1()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 1";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));
            form.Controls.Add(BeautyfyForms.AddButton("Открыть файл множества", new Point(0, 10), (o, k) =>
            {
                Helper.LoadFileNode("Файл множества", "mul", multiplicity);
                dgv.ColumnCount = multiplicity.Count;
                for (int i = 0; i < multiplicity.Count; i++)
                {
                    dgv.Columns[i].HeaderCell.Value = (i + 1).ToString();
                    dgv.Columns[i].ReadOnly = true;

                    dgv.Rows[0].Cells[i].Value = multiplicity.ElementAt(i).value;
                }
                logLstBox.Items.Add(DateTime.Now.ToString() + ": Заполнили DGV множеством");
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(250, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 1. Задано множество, задана перестановка, вывести множество используя перестановку. ");
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Открыть файл перестановок", new Point(0, 40), (o, k) =>
            {
                Helper.LoadFile("Файл перестановок", "shuff", reshuffle);
                dgv.ColumnCount = reshuffle.Count;
                for (int i = 0; i < reshuffle.Count; i++)
                {
                    dgv.Rows[1].Cells[i].Value = reshuffle.ElementAt(i);
                }
                logLstBox.Items.Add(DateTime.Now.ToString() + ": Заполнили DGV перестановкой");
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Обработать", new Point(0, 70), (o, k) =>
            {
                Process();
            }));

            dgv = BeautyfyForms.AddDataGridView(new Point(0, 100), form.Size, 5, new string[] { "Множество", "Перестановка", "Δ", "Множество *" });
            form.Controls.Add(dgv);

            logLstBox = BeautyfyForms.AddListBox(new Point(0, 100 + dgv.Height + 5), form.Size);
            form.Controls.Add(logLstBox);
        }

        void Process()
        {
            if (multiplicity.Count != reshuffle.Count)
            {
                MessageBox.Show("Не совпадает кол-во элементов в заданном множестве и перестановке");
                return;
            }

            if (multiplicity.Count != reshuffle.Max())
            {
                MessageBox.Show("Не верная перестановка: максимальное значение не совпадает с кол-вом элементов");
                return;
            }

            if (reshuffle.Min() <= 0)
            {
                MessageBox.Show("В перестановке не могут содержаться отрицательные элементы и 0 (начинаем с 1)");
                return;
            }

            var same = reshuffle.GroupBy(x => x).Where(g => g.Count() > 1);
            if (same.Any())
            {
                MessageBox.Show("В перестановке содержатся повторные элементы, это недопустимо");
                return;
            }

            logLstBox.Items.Add(DateTime.Now.ToString() + ": Вычисляем дельты");

            var deltas = Node.CalculateDelta(reshuffle);

            logLstBox.Items.Add(DateTime.Now.ToString() + ": Готово");
            for (int i = 0; i < deltas.Count; i++)
            {
                dgv.Rows[2].Cells[i].Value = deltas.ElementAt(i);
            }
            logLstBox.Items.Add(DateTime.Now.ToString() + ": Заполнили DGV дельтами");

            for (int i = 0; i < multiplicity.Count; i++)
            {
                dgv.Rows[3].Cells[i].Value = multiplicity.ElementAt(i + deltas.ElementAt(i)).value;
            }
            logLstBox.Items.Add(DateTime.Now.ToString() + ": Заполнили DGV искомым множеством");
            logLstBox.Items.Add(DateTime.Now.ToString() + ": Готово");
        }
    }
}