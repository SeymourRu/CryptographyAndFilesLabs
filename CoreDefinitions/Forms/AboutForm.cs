using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoreDefinitions.Factories;
using CoreDefinitions.Views;
using System.Configuration;

namespace CoreDefinitions.Forms
{
    public partial class AboutForm : Form, IAboutView
    {
        private Point startLocationLbl1;
        private Point startLocationLbl2;
        private Point startLocationLbl3;
        public AboutForm()
        {
            InitializeComponent();
            Init();
            timer1.Start();
        }

        public void Init()
        {
            label1.Text = "Лабораторные работы по :\r\n";

            var labTypes = ConfigurationManager.AppSettings.AllKeys.Where(x => x.Contains("TaskType_"));

            foreach(var type in labTypes)
            {
                label1.Text += ConfigurationManager.AppSettings[type] + "\r\n";
            }

            label2.Text = "Выполнил: " + ConfigurationManager.AppSettings["Author"];
            label3.Text = "Группа: " + ConfigurationManager.AppSettings["Group"];
            startLocationLbl1 = label1.Location;
            startLocationLbl2 = label2.Location;
            startLocationLbl3 = label3.Location;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var newPointlb1 = new Point(label1.Location.X - 1, label1.Location.Y);
            var newPointlb2 = new Point(label2.Location.X - 1, label2.Location.Y);
            var newPointlb3 = new Point(label3.Location.X - 1, label3.Location.Y);

            if (newPointlb1.X + label1.Size.Width + 5 < 0)
            {
                newPointlb1 = startLocationLbl1;
            }

            if (newPointlb2.X + label2.Size.Width + 5 < 0)
            {
                newPointlb2 = startLocationLbl2;
            }

            if (newPointlb3.X + label3.Size.Width + 5 < 0)
            {
                newPointlb3 = startLocationLbl3;
            }

            label1.Location = newPointlb1;
            label2.Location = newPointlb2;
            label3.Location = newPointlb3;
        }
    }
}
