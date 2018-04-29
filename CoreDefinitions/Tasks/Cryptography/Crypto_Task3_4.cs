using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;
using System.Windows.Forms;
using System.Drawing;
using System.Numerics;

namespace CoreDefinitions.Tasks
{
    public class Crypto_Task3_4 : ITask<Crypto_Task3_4>, IBaseTask
    {
        TaskAppType _subSystemType;

        TextBox _inputH;
        TextBox _inputG;
        TextBox _inputP;
        CheckBox _keepLog;
        ListBox logLstBox;
        Random rnd = new Random();

        List<Tuple<long, long, long>> equations = new List<Tuple<long, long, long>>();

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Crypto_Task3_4()
        {
            _subSystemType = Helpers.TaskAppType.GUI;

            #region LoadDefaultEquations
            equations.Add(new Tuple<long, long, long>(18, 2, 29));
            equations.Add(new Tuple<long, long, long>(166, 7, 433));
            equations.Add(new Tuple<long, long, long>(7531, 6, 8101));
            equations.Add(new Tuple<long, long, long>(525, 3, 809));
            equations.Add(new Tuple<long, long, long>(12, 7, 41));
            equations.Add(new Tuple<long, long, long>(70, 2, 131));
            equations.Add(new Tuple<long, long, long>(525, 2, 809));
            equations.Add(new Tuple<long, long, long>(525, -2, 131));
            #endregion LoadDefaultEquations
        }

        private Tuple<long, long, long> SelectRandomEquation()
        {
            return equations[rnd.Next() % equations.Count];
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 4";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));
            form.Controls.Add(BeautyfyForms.AddButton("Случайное уравнение", new Point(0, 10), (o, k) =>
            {
                var tuple = SelectRandomEquation();
                _inputH.Text = tuple.Item1.ToString();
                _inputG.Text = tuple.Item2.ToString();
                _inputP.Text = tuple.Item3.ToString();

                Log("Инициализовали параметры заготовленными значениями");
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Очистить лог", new Point(0, 43), (o, k) =>
            {
                logLstBox.Items.Clear();
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(170, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 4 Алгоритм Полига-Хеллмана \r\n Найти такой х, что H = G^x (mod P) ");
            }));

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(310, 10), "H", true, 10));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(410, 10), "G", true, 25));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(520, 10), "P", true, 30));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(705, 100), "Очищать лог", true, 100));

            _inputH = BeautyfyForms.CreateTextBox(new Point(270, 33), false);
            _inputH.Text = "0";
            form.Controls.Add(_inputH);

            _inputG = BeautyfyForms.CreateTextBox(new Point(380, 33), false);
            _inputG.Text = "0";
            form.Controls.Add(_inputG);

            _inputP = BeautyfyForms.CreateTextBox(new Point(490, 33), false);
            _inputP.Text = "0";
            form.Controls.Add(_inputP);

            _keepLog = BeautyfyForms.CreateCheckBox(new Point(740, 115), false);
            form.Controls.Add(_keepLog);

            logLstBox = BeautyfyForms.AddListBox(new Point(0, 150 + 5), form.Size);
            logLstBox.MouseDoubleClick += (x, y) =>
            {
                if (logLstBox.SelectedItem != null)
                {
                    MessageBox.Show(logLstBox.SelectedItem.ToString());
                }
            };
            form.Controls.Add(logLstBox);

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(380, 63), "H = G^x (mod P)", true, 100));

            form.Controls.Add(BeautyfyForms.AddButton(" Поехали ", new Point(380, 93), (o, k) =>
            {
                ParseAndRunPohligHellman();
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

        public void PrintFormated(object arg1, object arg2, object arg3, object arg4, object arg5)
        {
            Log("---------------------------------------------------------");
            Log(String.Format(" {0} | {1} | {2} | {3} | {4}", arg1, arg2, arg3, arg4, arg5));
            Log("---------------------------------------------------------");
        }

        void ParseAndRunPohligHellman()
        {
            if (string.IsNullOrEmpty(_inputG.Text))
            {
                MessageBox.Show("Введите number");
                return;
            }

            if (string.IsNullOrEmpty(_inputH.Text))
            {
                MessageBox.Show("Введите H");
                return;
            }

            if (string.IsNullOrEmpty(_inputG.Text))
            {
                MessageBox.Show("Введите G");
                return;
            }

            if (string.IsNullOrEmpty(_inputP.Text))
            {
                MessageBox.Show("Введите P");
                return;
            }

            long h = 0, g = 0, p = 0;
            try
            {
                h = long.Parse(_inputH.Text);
                g = long.Parse(_inputG.Text);
                p = long.Parse(_inputP.Text);
            }
            catch
            {
                MessageBox.Show("Какое-то значение не валидно");
                return;
            }

            if (_keepLog.Checked)
            {
                logLstBox.Items.Clear();
            }

            try
            {
                PohlingHellman(h, g, p);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                Log(String.Format(" Для уравнения {0} = {1}^x (mod {2}) не существует решения ", h, g, p));
                Log("----------------------------------------------");  
            }
            Log("\n");
        }

        public static Dictionary<long, int> CountOccurences(List<long> primeFactors)
        {
            return primeFactors.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        }

        public void PohlingHellman(long h, long g, long p)
        {
            var CountOccurencesList = CountOccurences(CryptographyMath.Factorization(p - 1)).ToList();
            var CongruenceList = new List<KeyValuePair<long, long>>();
            Log("---------------------------------------------------------");
            Log(String.Format(" Решаем уравнение {0} = ({1})^x (mod {2})", h, g, p));
            Log("---------------------------------------------------------");
            PrintFormated("q", "e", "g^((p-1)/q^e)", "h^((p-1)/q^e)", "Уравнение (g^((p-1)/q^e))^x = h^((p-1)/q^e) для x");
            foreach (var i in Enumerable.Range(0, CountOccurencesList.Count))
            {
                var e1 = CryptographyMath.FixModulo((long)BigInteger.ModPow(g, ((p - 1) / BigInteger.Pow(CountOccurencesList[i].Key, CountOccurencesList[i].Value)), p), p);
                var e2 = CryptographyMath.FixModulo((long)BigInteger.ModPow(h, ((p - 1) / BigInteger.Pow(CountOccurencesList[i].Key, CountOccurencesList[i].Value)), p), p);

                CongruenceList.Add(CryptographyMath.CongruencePair(h, g, p, CountOccurencesList[i].Key, (long)CountOccurencesList[i].Value, e1, e2));
                
                var e3 = CongruenceList[CongruenceList.Count - 1].Key % CongruenceList[CongruenceList.Count - 1].Value;
                var e4 = CongruenceList[CongruenceList.Count - 1].Value;
                PrintFormated(CountOccurencesList[i].Key, CountOccurencesList[i].Value, e1, e2, String.Format("x = {0} (mod {1})", e3, e4));
            }
            Log(String.Format(" Решаем систему уравнений с помощью КТО:"));
            Log("---------------------------------------------------------");
            Log(String.Format(" x = {0}", CryptographyMath.ChineseRemainderAlgorithm(CongruenceList)));
            Log("---------------------------------------------------------");
        }
    }
}