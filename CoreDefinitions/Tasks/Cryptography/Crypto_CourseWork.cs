using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;
using System.Configuration;
using System.Numerics;

namespace CoreDefinitions.Tasks
{
    public class SmoothInfo
    {
        public long a;
        public long b;
        public List<long> aVec = new List<long>();
        public List<long> epsVec = new List<long>();
    }

    public enum DixonOperations
    {
        GenerateB = 0,
        GenerateA = 1,
        CheckSmooth = 2,
        CheckLinearEquation = 3,
        CheckXY = 4,
        CalculateGcd = 5,
    }

    public class Crypto_CourseWork : ITask<Crypto_CourseWork>, IBaseTask
    {
        TaskAppType _subSystemType;

        NumericUpDown _inputNumber;
        NumericUpDown _inputThreads;
        CheckBox _calculateOperations;
        ListBox logLstBox;
        Random random = new Random();

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Crypto_CourseWork()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Алгоритм Диксона - Курсовая работа";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(40, 10), "Число для факторизации (2 - 2147483647)", true, 250));
            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(70, 56), "Кол-во потоков (1-100)", true, 150));


            _inputNumber = BeautyfyForms.CreateNumericUpDown(new Point(70, 33), 2, 2147483647, false);
            _inputNumber.Value = 2;
            form.Controls.Add(_inputNumber);

            _inputThreads = BeautyfyForms.CreateNumericUpDown(new Point(70, 79), 1, 100, false);
            _inputThreads.Value = 1;
            form.Controls.Add(_inputThreads);

            form.Controls.Add(BeautyfyForms.AddButton("Инициализация", new Point(300, 20), (o, k) =>
            {
                _inputNumber.Value = 2;
                _inputThreads.Value = 1;
                Log("Инициализовали параметры стандартными значениями");
            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Факторизовать ", new Point(290, 55), (o, k) =>
            {
                ParseAndRunDixon();
            }));

            form.Controls.Add(BeautyfyForms.CreateLabel(new Point(280, 90), "Считать кол-во операций", true, 150));

            _calculateOperations = BeautyfyForms.CreateCheckBox(new Point(350, 105), false);
            form.Controls.Add(_calculateOperations);

            form.Controls.Add(BeautyfyForms.AddButton(" О программе ", new Point(545, 20), (o, k) =>
            {
                string buffer = "Курсовая работа по предмету `Математические основы защиты информации и информационная безопасность`" + '\n';
                buffer += "Алгоритм Диксона." + '\n';
                buffer += "Данный алгоритм предназначен для факторизации натуральных чисел" + '\n';
                buffer += "Выполнил: " + ConfigurationManager.AppSettings["Author"] + ", группа " + ConfigurationManager.AppSettings["Group"];
                MessageBox.Show(buffer);
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Очистить лог", new Point(550, 55), (o, k) =>
            {
                logLstBox.Items.Clear();
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Кол-во операций -> буфер обмена", new Point(470, 90), (o, k) =>
            {
                var researchData = new List<string>();
                foreach (string item in logLstBox.Items)
                {
                    if (item.Contains("Общее кол-во выполненых операций"))
                    {
                        researchData.Add(item);
                    }
                }
                if (researchData.Count > 0)
                {
                    System.Windows.Forms.Clipboard.SetText(string.Join("\r\n", researchData));
                    MessageBox.Show("Скопировано!");
                }
                else
                {
                    MessageBox.Show("Подходящие сообщения отсутствуют.");
                }
            }));

            logLstBox = BeautyfyForms.AddListBox(new Point(0, 150 + 5), form.Size);
            logLstBox.MouseDoubleClick += (x, y) =>
            {
                if (logLstBox.SelectedItem != null)
                {
                    MessageBox.Show(logLstBox.SelectedItem.ToString());
                }
            };
            form.Controls.Add(logLstBox);
        }

        private void Log(string text)
        {
            var taskID = GetTaskID();
            logLstBox.BeginInvoke(new MethodInvoker(() => logLstBox.Items.Add(DateTime.Now.ToString() + taskID + ": " + text)));
        }

        private void LogWithoutDate(string text)
        {
            logLstBox.Items.Add(text);
        }

        string GetTaskID()
        {
            return Task.CurrentId.HasValue ? " TaskID=" + Task.CurrentId.Value.ToString() : "";
        }

        public static double LofN(double N)
        {
            return Math.Exp(Math.Sqrt(Math.Log(N, Math.E) * Math.Log(Math.Log(N, Math.E), Math.E)));
        }


        void ParseAndRunDixon()
        {
            var N = (int)_inputNumber.Value;
            var instances = (int)_inputThreads.Value;

            var calculateOperations = _calculateOperations.Checked;

            if (CryptographyMath.IsPower(N))
            {
                Log("N = " + N + " - степень простого числа");
            }
            else
            {
                DixonAlgorithm(N, instances, calculateOperations);
            }
        }

        private void DixonAlgorithm(int N, int numOfInstances, bool calculateOperations)
        {
            Log("Начинаем подбор. Факторизуемое число - " + N + " .Кол-во инстансов - " + numOfInstances);

            var tasks = new List<Task>();

            for (int iter = 0; iter < numOfInstances; iter++)
            {
                tasks.Add(new Task(() =>
                {
                    try
                    {
                        var calculateOps = calculateOperations;
                        var primesUponN = CryptographyMath.SieveOfEratosthenes(N).ToList();
                        if (primesUponN.Contains(N))
                        {
                            Log("Число " + N + " - простое!");
                            return;
                        }

                        primesUponN.RemoveAt(0);//Remove 1 from list

                        var minN = (int)Math.Sqrt(N);
                        var M = Math.Pow(LofN(N), 0.5);
                        var primes = primesUponN.Where(x => x <= M).ToList();

                        if (primes.Count == 0)
                        {
                            Log("Слишком маленькое число !!");
                            return;
                        }

                        var B = new List<long>(primes.ConvertAll(i => (long)i));//build factor base
                        int h = B.Count + 1;

                        var smoothed = new List<SmoothInfo>();
                        var operationCounter = new List<DixonOperations>();
                        var answerNotFound = true;

                        while (answerNotFound)
                        {
                            int curFound = 0;
                            smoothed.Clear();
                            while (curFound < h)
                            {
                                var b = random.Next(minN + 1, N - 1);
                                var a = long.Parse(BigInteger.ModPow(new BigInteger(b), new BigInteger(2), new BigInteger(N)).ToString());

                                if (calculateOps)
                                {
                                    operationCounter.Add(DixonOperations.GenerateB);
                                    operationCounter.Add(DixonOperations.GenerateA);
                                }

                                if (a == 0)
                                {
                                    continue;
                                }

                                var factors = new List<long>();

                                if (calculateOps)
                                {
                                    operationCounter.Add(DixonOperations.CheckSmooth);
                                }

                                if (CryptographyMath.IsSmooth(a, B, out factors))
                                {
                                    var smooth = new SmoothInfo();
                                    smooth.a = a;
                                    smooth.b = b;
                                    smooth.aVec = factors;
                                    factors.ForEach(x => smooth.epsVec.Add(x % 2));
                                    smoothed.Add(smooth);
                                    curFound += 1;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            for (int first = 0; first < smoothed.Count && answerNotFound == true; first++)
                            {
                                for (int second = first; second < smoothed.Count && answerNotFound == true; second++)
                                {
                                    if (first != second)
                                    {
                                        if (calculateOps)
                                        {
                                            operationCounter.Add(DixonOperations.CheckLinearEquation);
                                        }

                                        var vecOne = smoothed[first];
                                        var vecTwo = smoothed[second];

                                        var deltaLeft = smoothed.Count - (smoothed.Count - first);
                                        var deltaMiddle = second - (first + 1);
                                        var currentAnswer = new List<double>();
                                        var matrix = new List<List<double>>();

                                        //create correct matrix here
                                        for (int i = 0; i < vecOne.aVec.Count; i++)
                                        {
                                            var vertex = new List<double>();
                                            vertex.AddRange(Enumerable.Repeat(0.0, deltaLeft));
                                            vertex.Add(vecOne.aVec[i]);
                                            vertex.AddRange(Enumerable.Repeat(0.0, deltaMiddle));
                                            vertex.Add(vecTwo.aVec[i]);
                                            vertex.Add(0);
                                            matrix.Add(vertex);
                                        }

                                        CryptographyMath.TheTruePowerfullGauss(matrix, currentAnswer);

                                        if (currentAnswer.Any(x => x != 0))
                                        {
                                            var probableX = (vecOne.b * vecTwo.b) % N;

                                            var probableY = 1.0;
                                            for (int i = 0; i < B.Count; i++)
                                            {
                                                var value = B[i];
                                                var step = (vecOne.aVec[i] + vecTwo.aVec[i]) / 2.0;
                                                probableY *= Math.Pow(value, step);
                                            }

                                            probableY = probableY % N;

                                            if (calculateOps)
                                            {
                                                operationCounter.Add(DixonOperations.CheckXY);
                                            }

                                            if ((probableX == probableY) || (probableX == -probableY))
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                //n = u * v
                                                //u = gcd(x+y,n)
                                                //v = gcd(x-y,n)

                                                var u = CryptographyMath.ExtendedGCD((int)(probableX + probableY), N)[0];
                                                var v = CryptographyMath.ExtendedGCD((int)(probableX - probableY), N)[0];

                                                if (calculateOps)
                                                {
                                                    operationCounter.Add(DixonOperations.CalculateGcd);
                                                    operationCounter.Add(DixonOperations.CalculateGcd);
                                                }

                                                if (((u * v) == N) && (u != 1) && (v != 1))
                                                {
                                                    Log("Разложение найдено - (" + u + ") * (" + v + ") = " + N);
                                                    answerNotFound = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                        }

                        if (calculateOps)
                        {
                            var finalResult = "\r\nОбщее кол-во выполненых операций:" + operationCounter.Count + "\r\n";
                            var countResults = operationCounter.GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
                            foreach (var info in countResults)
                            {
                                finalResult += info.Key + " - " + info.Value + "\r\n";
                            }

                            Log(finalResult);
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("OutOfMemoryException"))
                        {
                            Log("Недостаточно памяти для вычисления значения");
                        }
                        else
                        {
                            Log("Возникла ошибка:" + e.Message);
                        }
                        return;
                    }
                }));
            }

            tasks.ForEach(x => x.Start());
        }

    }
}
