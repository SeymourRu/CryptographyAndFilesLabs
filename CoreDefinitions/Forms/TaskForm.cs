using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using CoreDefinitions.Factories;
using CoreDefinitions.Views;
using CoreDefinitions.Tasks;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;

namespace CoreDefinitions.Forms
{
    public partial class TaskForm<T> : Form, ITaskView<T> where T : class
    {
        ITask<T> _task;
        ConsoleHandler console;
        delegate void Visibility(bool state);

        public TaskForm(ITask<T> task)
        {
            InitializeComponent();
            _task = task;
            Init();
        }

        public void Init()
        {
            if (_task.SubSystemType == Helpers.TaskAppType.GUI)
            {
                _task.LocateControls(this,null);
            }
            else
            {
                console = new ConsoleHandler(() =>
                {
                    SetVisibility(true);
                });
                console.OpenConsole(SetVisibility);
            }
        }

        new public DialogResult ShowDialog()
        {
            if (_task.SubSystemType == Helpers.TaskAppType.GUI)
            {
                return base.ShowDialog();
            }
            else
            {
                console.SetHandler();
                _task.LocateControls(null, console);
                return DialogResult.Ignore;
            }
        }

        new public void Show()
        {
            if (_task.SubSystemType == Helpers.TaskAppType.GUI)
            {
                base.Show();
            }
        }

        public void SetVisibility(bool vis)
        {
            if (this.InvokeRequired)
            {
                Visibility d = new Visibility(SetVisibility);
                this.Invoke(d, new object[] { vis });
            }
            else
            {
                this.Visible = vis;
            }

            if (vis)
            {
                this.Close();
            }
        }
    }
}