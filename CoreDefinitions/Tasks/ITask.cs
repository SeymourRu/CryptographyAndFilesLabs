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
    public interface ITask<T> where T : class
    {
        TaskAppType SubSystemType { get; }
        void LocateControls(Form form,ConsoleHandler console);
    }
}
