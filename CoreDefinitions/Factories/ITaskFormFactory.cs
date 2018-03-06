using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDefinitions.Views;
using CoreDefinitions.Tasks;

namespace CoreDefinitions.Factories
{
    public interface ITaskFormFactory<T> where T:class
    {
        //ITaskView<T> CreateInstance(ITask<T> task);
    }
}
