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
    public class Crypto_Task3_2 : ITask<Crypto_Task3_2>, IBaseTask
    {
        TaskAppType _subSystemType;
        TextBox _inputNumber;
        Random rnd = new Random();

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Crypto_Task3_2()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 2";
            form.SetDefaultVals(new System.Drawing.Size(500, 200));
            form.Controls.Add(BeautyfyForms.AddButton("Инициализация", new Point(0, 45), (o, k) =>
            {
                _inputNumber.Text = rnd.Next(1, 200).ToString();
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(240, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 2. Тест числа на простоту ( Рабин-Миллер )");
            }));

            _inputNumber = BeautyfyForms.CreateTextBox(new Point(200, 45), false);
            _inputNumber.Text = "0";
            form.Controls.Add(_inputNumber);

            form.Controls.Add(BeautyfyForms.AddButton(" Поехали ", new Point(230, 75), (o, k) =>
            {
                ParseAndRunRabinMillerTest();
            }));
        }

        void ParseAndRunRabinMillerTest()
        {
            if (string.IsNullOrEmpty(_inputNumber.Text))
            {
                MessageBox.Show("Введите число");
                return;
            }

            long number = 0;
            try
            {
                number = long.Parse(_inputNumber.Text);
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

            Task.Factory.StartNew(() => RabinMillerEntry(number));
        }

        void RabinMillerEntry(long number)
        {
            try
            {
                long baseNum = (long)rnd.Next(2, (int)number - 2);
                long s = 0; // степень двойки
                long t = 0; // нечетное число, n-1 = 2^s*t
                bool searchRequired = true;
                long primeSearchCounter = 0;

                // если число n чётное или НОД(baseNum, n)!=1, то оно составное
                if (number % 2 == 0 || CryptographyMath.ExtendedGCD(baseNum, number)[0] != 1)
                {
                    MessageBox.Show("Число " + number.ToString() + " скорее всего составное");
                }
                else
                {
                    t = (number - 1);
                    //представляем число n - 1 в таком виде: n-1 = 2^s*t,  находим s и t
                    while (searchRequired)
                    {
                        if (t % 2 == 0)
                        {
                            s++;
                            t = (number - 1) / (long)Math.Pow(2, s);
                        }
                        else
                        {
                            searchRequired = false;
                        }
                    }

                    //если baseNum^t = 1 (mod n), или baseNum^((2^r)*t) = -1 (mod n) при 0<=r<s , то n псевдопростое по основанию baseNum
                    for (int i = 0; i < s; i++)
                    {
                        long atn = CryptographyMath.PowerSearch(baseNum, t, number); // возводим а в степень t по модулю n

                        if ((Math.Abs(atn) == 1) || (CryptographyMath.PowerSearch(atn, (long)Math.Pow(2, i), number) == number - 1)) // вместо того, чтобы писать Stepen(atn, (long) Math.Pow(2, i), n) == -1
                        {
                            MessageBox.Show("Число " + number.ToString() + " вероятно простое");
                            break;
                        }
                        else
                        {
                            primeSearchCounter += 1;
                        }
                    }
                    //если ни при одном r (0<=r<s) не выполняется baseNum^((2^r)*t) = -1 (mod n), то n составное
                    if (primeSearchCounter == s)
                    {
                        MessageBox.Show(" Число " + number.ToString() + " составное");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка:" + ex.Message);
            }
        }
    }
}