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
    public class Crypto_Task3_3 : ITask<Crypto_Task3_3>, IBaseTask
    {
        TaskAppType _subSystemType;
        TextBox _inputNumber;
        TextBox _inputFactor;
        Random rnd = new Random();

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Crypto_Task3_3()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 3";
            form.SetDefaultVals(new System.Drawing.Size(500, 200));
            form.Controls.Add(BeautyfyForms.AddButton("Инициализация", new Point(0, 45), (o, k) =>
            {
                _inputNumber.Text = rnd.Next(1, 200).ToString();
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(240, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 3. Факторизация числа ( Поллард ρ-1 )");
            }));

            _inputNumber = BeautyfyForms.CreateTextBox(new Point(150, 70), false);
            _inputNumber.Text = "0";
            form.Controls.Add(_inputNumber);

            _inputFactor = BeautyfyForms.CreateTextBox(new Point(270, 70), false);
            _inputFactor.Text = "0";
            form.Controls.Add(_inputFactor);

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(150, 55), "  Число                               Значение B", false, 5)); //meh~

            form.Controls.Add(BeautyfyForms.AddButton(" Поехали ", new Point(230, 95), (o, k) =>
            {
                ParseAndRunPollardPMinusOne();
            }));
        }

        void ParseAndRunPollardPMinusOne()
        {
            if (string.IsNullOrEmpty(_inputNumber.Text))
            {
                MessageBox.Show("Введите число");
                return;
            }

            long number = 0, b = 0;
            try
            {
                number = long.Parse(_inputNumber.Text);
                b = long.Parse(_inputFactor.Text);
                if (number == 0)
                {
                    throw new Exception();
                }
            }
            catch
            {
                MessageBox.Show("Какое-то значение не валидно");
                return;
            }

            Task.Factory.StartNew(() => PollardPMinusOneEntry(number, b));
        }

        void PollardPMinusOneEntry(long number, long b)
        {
            try
            {
                long a = 2, p;
                for (int j = 2; j <= b; j++)
                {
                    a = CryptographyMath.PowerSearch(a, j, number);
                }

                p = CryptographyMath.ExtendedGCD(a - 1, number)[0];
                if (p != 1 || p != number)
                {
                    MessageBox.Show(p.ToString() + " - делитель " + number.ToString());
                }
                else
                {
                    MessageBox.Show("Увы, попробуй ещё раз");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка:" + ex.Message);
            }
        }
    }
}