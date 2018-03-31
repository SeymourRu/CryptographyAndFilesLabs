using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Configuration;

namespace CoreDefinitions.Tasks
{
    public class Files_Task1_1 : ITask<Files_Task1_1>, IBaseTask
    {
        TaskAppType _subSystemType;

        TextBox _geometric;
        TextBox _binominal;
        TextBox _wedgeShaped;
        ListBox logGeometric;
        ListBox logBinominal;
        ListBox logWedgeShaped;

        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Files_Task1_1()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }

        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Задание № 1";
            form.SetDefaultVals(new System.Drawing.Size(800, 500));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(370, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 1. Игра с Геометрическим, Биномиальным и Клиновидным распределениями!");
            }));

            logGeometric = BeautyfyForms.AddListBox(new Point(10, 100), new Size(200, 450));
            form.Controls.Add(logGeometric);

            _geometric = BeautyfyForms.CreateTextBox(new Point(40, 420), true);
            form.Controls.Add(_geometric);

            logBinominal = BeautyfyForms.AddListBox(new Point(300, 100), new Size(200, 450));
            form.Controls.Add(logBinominal);

            _binominal = BeautyfyForms.CreateTextBox(new Point(340, 420), true);
            form.Controls.Add(_binominal);

            logWedgeShaped = BeautyfyForms.AddListBox(new Point(590, 100), new Size(200, 450));
            form.Controls.Add(logWedgeShaped);

            _wedgeShaped = BeautyfyForms.CreateTextBox(new Point(625, 420), true);
            form.Controls.Add(_wedgeShaped);

            form.Controls.Add(BeautyfyForms.AddButton("Геометрическое", new Point(30, 70), (o, k) =>
            {
                logGeometric.Items.Clear();
                Task.Run(() =>
                    {
                        var listOfElements = new List<KeyValueItem>();
                        var geom = new MathNet.Numerics.Distributions.Geometric(0.6);
                        for (int i = 0; i <= 1000; i++)
                        {
                            listOfElements.Add(new KeyValueItem(i, geom.Probability(i)));
                        }
                        var sum = (from element in listOfElements.AsParallel() select element.Probability).Sum();
                        listOfElements = (from element in listOfElements.AsParallel() orderby element.Probability descending select element).ToList();
                        foreach (var item in listOfElements)
                        {
                            logGeometric.BeginInvoke(new MethodInvoker(() => logGeometric.Items.Add(item)));
                        }

                        double average = 0;
                        for (int i = 0; i < 100000; i++)
                        {
                            var search = geom.Sample();
                            for (int j = 0; j < listOfElements.Count; j++)
                            {
                                if (listOfElements[j].Key == search)
                                {
                                    average += j;
                                }
                            }
                        }

                        average /= 100000;
                        _geometric.BeginInvoke(new MethodInvoker(() => _geometric.Text = Math.Round(average).ToString()));
                    });
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Биноминальное", new Point(335, 70), (o, k) =>
            {
                logBinominal.Items.Clear();
                Task.Run(() =>
                    {
                        var listOfElements = new List<KeyValueItem>();
                        var binom = new MathNet.Numerics.Distributions.Binomial(0.4, 1000);
                        for (int i = 0; i <= 1000; i++)
                        {
                            listOfElements.Add(new KeyValueItem(i, binom.Probability(i)));
                        }
                        listOfElements = (from element in listOfElements.AsParallel() orderby element.Probability descending select element).ToList();
                        foreach (var item in listOfElements)
                        {
                            logBinominal.BeginInvoke(new MethodInvoker(() => logBinominal.Items.Add(item)));
                        }

                        long average = 0;

                        for (int i = 0; i < 100000; i++)
                        {
                            var search = binom.Sample();
                            for (int j = 0; j < listOfElements.Count; j++)
                            {
                                if (listOfElements[j].Key == search)
                                {
                                    average += j;
                                }
                            }
                        }

                        average /= 100000;
                        _binominal.BeginInvoke(new MethodInvoker(() => _binominal.Text = Math.Round((decimal)average).ToString()));
                    });
            }));

            form.Controls.Add(BeautyfyForms.AddButton("Клиновидное", new Point(630, 70), (o, k) =>
            {
                logWedgeShaped.Items.Clear();
                Task.Run(() =>
                    {
                        var listOfElements = new List<KeyValueItem>();
                        Random random = new Random((int)DateTime.Now.ToBinary());
                        int N = 1000;
                        for (int i = 0; i < 1000; i++)
                        {
                            listOfElements.Add(new KeyValueItem(i, (N - i) * (2.0 / (N * (N + 1)))));
                        }
                        var sum = (from element in listOfElements.AsParallel() select element.Probability).Sum();
                        listOfElements = (from element in listOfElements.AsParallel() orderby element.Probability descending select element).ToList();
                        foreach (var item in listOfElements)
                        {
                            logWedgeShaped.BeginInvoke(new MethodInvoker(() => logWedgeShaped.Items.Add(item)));
                        }

                        double average = 0;

                        for (int i = 0; i < 100000; i++)
                        {
                            var p = (N - random.Next(0, 1000)) * (2.0 / (N * (N + 1)));
                            var search = -(1 / 2.0) * N * (N * p + p - 2);
                            for (int j = 0; j < listOfElements.Count; j++)
                            {
                                if (listOfElements[j].Key == search)
                                {
                                    average += j;
                                }
                            }
                        }
                        average /= 100000;
                        _wedgeShaped.BeginInvoke(new MethodInvoker(() => _wedgeShaped.Text = Math.Round(average).ToString()));
                    });
            }));
        }
    }
}
