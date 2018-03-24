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

        int size = 5000, a = 106, b = 1283, c = 6075;

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
            form.Controls.Add(BeautyfyForms.AddButton("Инициализация", new Point(0, 10), (o, k) =>
            {

            }));

            form.Controls.Add(BeautyfyForms.AddButton(" Суть ", new Point(200, 10), (o, k) =>
            {
                MessageBox.Show("Задача № 1. Игра с Геометрическим, Биномиальным и Клиновидным распределениями!");
            }));


        }

        private double CogurentGeneration(int x)
        {
            return (a * x + b) % c;
        }

        private double Func(double x)
        {
            return Math.Exp(-x * x / 2) / Math.Sqrt(2 * Math.PI);
        }

        //численное интегрирование функции Лапласса
        private double SimpsonIntegr(double a, double b)
        {
            return ((b - a) / 6) * (Func(a) + 4 * Func((a + b) / 2) + Func(b));
        }

        private double LaplasFunction(double x)
        {
            double a = -x;
            double b = x;
            double tst, tst1, midlle;
            int c = 2;
            tst1 = SimpsonIntegr(a, b);
            do
            {
                tst = tst1;
                tst1 = 0;
                midlle = (b - a) / c;
                for (int i = 0; i < c; ++i)
                {
                    tst1 = tst1 + SimpsonIntegr(a + midlle * i, a + midlle * (i + 1));
                }
                c += 1;
            } while (Math.Abs(tst - tst1) >= 0.0000001);
            return tst1;
        }

    }
}
