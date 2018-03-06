using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using CoreDefinitions.Factories;
using CoreDefinitions.Views;
using CoreDefinitions.Tasks;
using CoreDefinitions.Helpers;

namespace CoreDefinitions.Forms
{
    public partial class MainForm : Form, IMainView
    {
        IAboutFormFactory _about;
        IContainer _container;
        IEnumerable<Type> _tasks;

        public MainForm(IContainer container)
        {
            InitializeComponent();
            _container = container;
            _about = _container.Resolve<IAboutFormFactory>();

            Init(); //Load all implemented tasks
        }

        public void Init()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                _tasks = ImplementationSearcher.GetImplementingTypes<IBaseTask>(scope);

                foreach (var task in _tasks)
                {
                    comboBox1.Items.Add(task.Name);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ((Form)_about.CreateInstance()).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                //We invoke TaskFormFactory<T> where T: Task1_1,Task1_2,Task1_3,etc

                var type = _tasks.ElementAt(comboBox1.SelectedIndex); //Get Type of selected task
                var task = _container.Resolve(typeof(ITask<>).MakeGenericType(type)); // Resolve exact implementation of selected Type
                var genericType = typeof(TaskFormFactory<>).MakeGenericType(type);//Build TaskFormFactory<FoundTaskImplementation> type
                var instance = Activator.CreateInstance(genericType);//Create instance of such object
                var method = instance.GetType().GetRuntimeMethods().Where(x => x.Name == "CreateInstance").ElementAt(0);//Find ptr to CreateInstance
                dynamic result = method.Invoke(null, new object[] { task });//Get instance of TaskForm<FoundTaskImplementation>
                result.ShowDialog();//Show window or console
            }
        }
    }
}
