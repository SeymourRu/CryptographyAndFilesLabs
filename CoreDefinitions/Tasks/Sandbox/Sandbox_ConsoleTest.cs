using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;

namespace CoreDefinitions.Tasks
{
    public class Sandbox_ConsoleTest : ITask<Sandbox_ConsoleTest>, IBaseTask
    {
        TaskAppType _subSystemType;
        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Sandbox_ConsoleTest()
        {
            _subSystemType = Helpers.TaskAppType.Console;
        }


        public void LocateControls(Form form, ConsoleHandler console)
        {
            Console.WriteLine("To exit press ctrl + c");

            try
            {
                MailLoop();
            }
            catch (Exception ex)
            {
                if (!ex.StackTrace.Contains("System.IO"))
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void MailLoop()
        {
            while (true)
            {
                var input = Console.ReadLine();
                Console.WriteLine(input);
            }
        }

        void CleanSpaces()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var lines = System.IO.File.ReadAllLines(ofd.FileName);
                var reslst = new List<string>();
                foreach (var item in lines)
                {
                    reslst.Add(item.TrimStart());
                }
                System.IO.File.WriteAllLines(ofd.FileName + ".fixed", reslst);
            }
        }

        void CheckUniqness()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var lines = System.IO.File.ReadAllLines(ofd.FileName);
                var data = new HashSet<string>();
                foreach (var item in lines)
                {
                    data.Add(item);
                }

                System.IO.File.WriteAllLines(ofd.FileName + ".fixed", data);
            }
        }
    }
}