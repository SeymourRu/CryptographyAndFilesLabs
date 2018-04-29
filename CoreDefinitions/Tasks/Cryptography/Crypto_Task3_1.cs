using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;
using System.Windows.Forms;
using System.Drawing;

namespace CoreDefinitions.Tasks
{
    public class Crypto_Task3_1 : ITask<Crypto_Task3_1>, IBaseTask
    {
        TaskAppType _subSystemType;

        TextBox _inputNumber;
        TextBox _inputCycles;
        Random rnd = new Random();

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Crypto_Task3_1()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 1";
            form.SetDefaultVals(new System.Drawing.Size(500, 200));
            form.Controls.Add(BeautyfyForms.AddButton("Инициализация", new Point(0, 45), (o, k) =>
            {
                _inputNumber.Text = rnd.Next(1, 200).ToString();
                _inputCycles.Text = rnd.Next(10, 30).ToString();
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(240, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 1. Тест числа на простоту ( Ферма )");
            }));

            _inputNumber = BeautyfyForms.CreateTextBox(new Point(150, 70), false);
            _inputNumber.Text = "0";
            form.Controls.Add(_inputNumber);

            _inputCycles = BeautyfyForms.CreateTextBox(new Point(270, 70), false);
            _inputCycles.Text = "0";
            form.Controls.Add(_inputCycles);

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(150, 55), "    Число                             Кол-во итераций", false, 5)); //meh~

            form.Controls.Add(BeautyfyForms.AddButton(" Поехали ", new Point(230, 95), (o, k) =>
            {
                ParseAndRunFermaTest();
            }));
        }

        void ParseAndRunFermaTest()
        {
            if (string.IsNullOrEmpty(_inputNumber.Text))
            {
                MessageBox.Show("Введите число");
                return;
            }

            if (string.IsNullOrEmpty(_inputCycles.Text))
            {
                MessageBox.Show("Введите кол-во итераций");
                return;
            }


            long number = 0, cycles = 0;
            try
            {
                number = long.Parse(_inputNumber.Text);
                cycles = long.Parse(_inputCycles.Text);

                if (number ==0 || cycles == 0)
                {
                    throw new Exception();
                }
            }
            catch
            {
                MessageBox.Show("Какое-то значение не валидно");
                return;
            }

            Task.Factory.StartNew(() => FermaEntry(number, cycles));
        }

        void FermaEntry(long number, long cycles)
        {
            try
            {
                long a, b;
                bool prime = true;
                for (int i = 0; i < cycles; i++)
                {
                    a = (long)rnd.Next(2, (int)number - 1);
                    b = CryptographyMath.PowerSearch(a, number - 1, number);

                    if (b != 1)
                    {
                        MessageBox.Show("Число  " + number.ToString() + " скорее всего составное");
                        prime = false;
                        break;
                    }
                }

                if (prime)
                {
                    MessageBox.Show("Число  " + number.ToString() + " вероятно простое");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка:" + ex.Message);
            }
        }
    }
}