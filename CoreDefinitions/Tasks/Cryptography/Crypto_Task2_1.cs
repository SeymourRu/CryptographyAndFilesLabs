using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;
using System.Windows.Forms;
using System.Numerics;

namespace CoreDefinitions.Tasks
{
    public class Crypto_Task2_1 : ITask<Crypto_Task2_1>, IBaseTask
    {
        TaskAppType _subSystemType;

        TextBox _inputG;
        TextBox _inputA;
        TextBox _inputM;
        TextBox _inputN;
        CheckBox _autoFindN;
        ListBox logLstBox;

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Crypto_Task2_1()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 1 и 2";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));
            form.Controls.Add(BeautyfyForms.AddButton("Инициализация", new Point(0, 10), (o, k) =>
            {
                _inputG.Text = "64";//G
                _inputA.Text = "122";//H
                _inputM.Text = "607";//P
                if (!_autoFindN.Checked)
                {
                    _inputN.Text = "101";//N
                }

                Log("Инициализовали параметры стандартными значениями");
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Очистить лог", new Point(0, 43), (o, k) =>
            {
                logLstBox.Items.Clear();
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(150, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 1 и 2. ρ - метод Полларда (обычный и параллельный) \r\n Найти такой х, что g^x = a % m ");
            }));

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(310, 10), "g", true, 10));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(410, 10), "a", true, 25));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(520, 10), "m", true, 30));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(640, 10), "n", true, 30));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(705, 10), "Автоподбор n", true, 100));

            _inputG = BeautyfyForms.CreateTextBox(new Point(270, 33), false);
            _inputG.Text = "0";
            form.Controls.Add(_inputG);

            _inputA = BeautyfyForms.CreateTextBox(new Point(380, 33), false);
            _inputA.Text = "0";
            form.Controls.Add(_inputA);

            _inputM = BeautyfyForms.CreateTextBox(new Point(490, 33), false);
            _inputM.Text = "0";
            form.Controls.Add(_inputM);

            _inputN = BeautyfyForms.CreateTextBox(new Point(600, 33), false);
            _inputN.Text = "0";
            form.Controls.Add(_inputN);

            _autoFindN = BeautyfyForms.CreateCheckBox(new Point(740, 33), false);
            _autoFindN.CheckedChanged += (o, k) =>
            {
                if (_autoFindN.Checked)
                {
                    _inputN.ReadOnly = true;
                    _inputN.Text = "-1";
                }
                else
                {
                    _inputN.ReadOnly = false;
                    _inputN.Text = "0";
                }
            };
            form.Controls.Add(_autoFindN);

            logLstBox = BeautyfyForms.AddListBox(new Point(0, 150 + 5), form.Size);
            logLstBox.MouseDoubleClick += (x, y) =>
            {
                if (logLstBox.SelectedItem != null)
                {
                    MessageBox.Show(logLstBox.SelectedItem.ToString());
                }
            };
            form.Controls.Add(logLstBox);

            form.Controls.Add(BeautyfyForms.AddButton(" Поехали (обычный) ", new Point(410, 63), (o, k) =>
            {
                ParseAndRunPollard(false);
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Поехали (параллельный) ", new Point(410, 93), (o, k) =>
            {
                ParseAndRunPollard(true);
            }));

        }

        private void Log(string text)
        {
            logLstBox.Items.Add(DateTime.Now.ToString() + ": " + text);
        }

        private void LogWithoutDate(string text)
        {
            logLstBox.Items.Add(text);
        }

        void ParseAndRunPollard(bool parr)
        {
            if (string.IsNullOrEmpty(_inputG.Text))
            {
                MessageBox.Show("Введите g");
                return;
            }

            if (string.IsNullOrEmpty(_inputA.Text))
            {
                MessageBox.Show("Введите a");
                return;
            }

            if (string.IsNullOrEmpty(_inputM.Text))
            {
                MessageBox.Show("Введите m");
                return;
            }

            if (string.IsNullOrEmpty(_inputN.Text))
            {
                MessageBox.Show("Введите n");
                return;
            }

            long g = 0, a = 0, m = 0, n = 0;
            try
            {
                g = long.Parse(_inputG.Text);
                a = long.Parse(_inputA.Text);
                m = long.Parse(_inputM.Text);
                n = long.Parse(_inputN.Text);
            }
            catch
            {
                MessageBox.Show("Какое-то значение не валидно");
                return;
            }

            PollardEntry(g, a, m, n, parr, _autoFindN.Checked);
        }

        private void PollardEntry(long g, long a, long m, long n, bool parr = true, bool autofindn = false)
        {
            if (autofindn)
            {
                try
                {
                    Log(string.Format("Пробуем подобрать n"));
                    var value = AutoFindN(g, m);
                    if (value == -1)
                    {
                        return;
                    }
                    else
                    {
                        n = value;
                    }
                }
                catch
                {
                    Log("При такой конфигурации невозможно подобрать n.");
                    return;
                }
            }

            Log(string.Format("g^x = a % m; Попробуем найти x"));

            if (parr)
            {
                Log(string.Format("Используем параллельный метод"));
                try
                {
                    var answer = PollardParallel(g, a, m, n);
                    if (answer != null)
                    {
                        Log("Возможный ответ найден. х = " + answer);
                    }
                    else
                    {
                        Log("Не получилось :(");
                    }
                }
                catch
                {
                    Log("Не получилось :( . Проверьте значения g,a,m,n");
                }
            }
            else
            {
                Log(string.Format("Используем обычный метод"));
                var answer = PollardNormal(g, a, m, n);
                Log("Ответ найден. х = " + answer);
            }

            Log(string.Format("Готово!"));
        }

        private long PollardNormal(long _g, long _a, long _m, long _n)
        {
            int x1 = 1, a1 = 0, b1 = 0;
            int x2 = 1, a2 = 0, b2 = 0;

            if (_g == _a)
            {
                return 1;
            }

            LogWithoutDate("\ti\txi\tai\tbi\tx2i\ta2i\tb2i");

            int i = 0;
            do
            {
                Iteration(ref x1, ref a1, ref b1, (int)_g, (int)_a, (int)_m, (int)_n);
                Iteration(ref x2, ref a2, ref b2, (int)_g, (int)_a, (int)_m, (int)_n);
                Iteration(ref x2, ref a2, ref b2, (int)_g, (int)_a, (int)_m, (int)_n);
                LogWithoutDate("\t" + i + "\t" + x1 + "\t" + a1 + "\t" + b1 + "\t" + x2 + "\t" + a2 + "\t" + b2);
                i++;
            }
            while (x1 != x2);

            int x;
            int temp = 1;

            while (true)
            {
                if (((a1 + b1 * temp) % _n) == ((a2 + b2 * temp) % _n))
                {
                    x = temp;
                    break;
                }
                temp++;
            }
            return x;
        }

        public int? PollardParallel(long _g, long _a, long _m, long _n)
        {
            Random rand = new Random();
            int length = 100;

            var lpows = new List<int>();
            var rpows = new List<int>();
            var ai = new List<int>();
            var bi = new List<int>();
            var si = new List<int>();
            var ti = new List<int>();
            var result = new List<int>();

            for (int i = 0; i < length; i++)
            {
                ai.Add(rand.Next(0, (int)(_n - 1)));
                bi.Add(rand.Next(0, (int)(_n - 1)));
                lpows.Add((int)((CryptographyMath.FastPower(_g, ai[i], _n) * CryptographyMath.FastPower(_a, bi[i], _n)) % _n));
            }

            si.Add(rand.Next(0, (int)(_n - 1)));
            ti.Add(rand.Next(0, (int)(_n - 1)));
            rpows.Add((int)(CryptographyMath.FastPower(_g, si[0], _n) * CryptographyMath.FastPower(_a, ti[0], _n) % _n));

            LogWithoutDate("\ti\tgi\tai\tbi\tsi\tti");

            LogWithoutDate("\t" + 0 + "\t" + rpows[0] + "\t" + ai[0] + "\t" + bi[0] + "\t" + si[0] + "\t" + ti[0]);

            for (int i = 0; i < rpows.Count; i++)
            {
                int index = EasyCalculatableFunction((int)_m, rpows[i], rpows.Count) - 1;
                rpows.Add((int)((rpows[i] * lpows[index]) % _n));
                si.Add((int)((si[i] + ai[index]) % _n));
                ti.Add((int)((ti[i] + bi[index]) % _n));

                LogWithoutDate("\t" + (i + 1) + "\t" + rpows[i + 1] + "\t" + ai[index] + "\t" + bi[index] + "\t" + si[i + 1] + "\t" + ti[i + 1]);

                for (int j = 0; j < rpows.Count; j++)
                {
                    for (int k = 0; k < rpows.Count; k++)
                    {
                        if (rpows[k] == rpows[j] && j != k)
                        {
                            if (si[k] != si[j])
                            {
                                long x = Math.Abs(si[k] - si[j]) * CryptographyMath.FastPower(Math.Abs(ti[j] - ti[k]), CryptographyMath.Euler(_a) - 1, _a);
                                x = x % _m;
                                if (_a == CryptographyMath.FastPower(_g, x, _m))
                                {
                                    return (int)x;
                                }
                            }
                        }
                    }
                }

            }
            return null;
        }

        public int AutoFindN(long _g, long _m)
        {
            int i = 1;
            var lst = new List<long>();
            long res = CryptographyMath.FastPower(_g, i, _m);
            while (res != 1)
            {
                if (lst.Contains(res))
                {
                    Log("Не выйдет найти х - плохой диапазон значений. Попробуйте выбрать другие значения!");
                    return -1;
                }
                else
                {
                    lst.Add(res);
                    i++;
                    res = CryptographyMath.FastPower(_g, i, _m);
                }
            }
            return i;
        }

        private void Iteration(ref int x, ref int a, ref int b, int _g, int _a, int _m, int _n)
        {
            if (x <= _m / 3)
            {
                x = (_a * x) % _m;
                b = (b + 1) % _n;
            }
            else if ((x >= _m / 3) && (x <= 2 * _m / 3))
            {
                x = (x * x) % _m;
                a = (2 * a) % _n;
                b = (2 * b) % _n;
            }
            else
            {
                x = (_g * x) % _m;
                a = (a + 1) % _n;
            }
        }

        private int EasyCalculatableFunction(int m, int value, int setCount)
        {
            int parts = m / setCount;

            for (int i = 1; i <= setCount; i++)
            {
                if (value <= parts * i)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}