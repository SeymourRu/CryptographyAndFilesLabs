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
    public class Crypto_Task1_2 : ITask<Crypto_Task1_2>, IBaseTask
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
        List<Node> newMultiplicity = new List<Node>();

        public Crypto_Task1_2()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 2";
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
                MessageBox.Show("Задача № 2. Задано множество, задано перемешанное множество, вывести перестановку. ");
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Открыть файл нового множества", new Point(0, 40), (o, k) =>
            {
                Helper.LoadFileNode("Файл нового множества", "mul", newMultiplicity);
                dgv.ColumnCount = newMultiplicity.Count;
                for (int i = 0; i < newMultiplicity.Count; i++)
                {
                    dgv.Rows[1].Cells[i].Value = newMultiplicity.ElementAt(i).value;
                }
                logLstBox.Items.Add(DateTime.Now.ToString() + ": Заполнили DGV множеством *");
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Обработать", new Point(0, 70), (o, k) =>
            {
                Process();
            }));

            dgv = BeautyfyForms.AddDataGridView(new Point(0, 100), form.Size, 4, new string[] { "Множество", "Множество *", "Перестановка" });
            form.Controls.Add(dgv);

            logLstBox = BeautyfyForms.AddListBox(new Point(0, 100 + dgv.Height + 5), form.Size);
            form.Controls.Add(logLstBox);
        }

        void Process()
        {
            var diff = multiplicity.Where(n => !newMultiplicity.Select(n1 => n1.value).Contains(n.value));
            if (diff.Any())
            {
                MessageBox.Show("Множества не эквивалентны");
                return;
            }

            var result = new List<int>();
            var firstDic = multiplicity.ToDictionary(x => x.value, y => y.position);
            for (int i = 0; i < multiplicity.Count; i++)
            {
                result.Add(firstDic[newMultiplicity.ElementAt(i).value] + 1);
            }
            logLstBox.Items.Add(DateTime.Now.ToString() + ": Вычислили искомую перестановку");

            for (int i = 0; i < multiplicity.Count; i++)
            {
                dgv.Rows[2].Cells[i].Value = result.ElementAt(i);
            }
            logLstBox.Items.Add(DateTime.Now.ToString() + ": Заполнили DGV искомой перестановкой");
            logLstBox.Items.Add(DateTime.Now.ToString() + ": Готово");
        }
    }
}