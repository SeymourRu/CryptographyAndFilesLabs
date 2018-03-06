using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDefinitions.Views;
using CoreDefinitions.Forms;
using CoreDefinitions.Tasks;

namespace CoreDefinitions.Factories
{
    public class TaskFormFactory<T> : ITaskFormFactory<T> where T : class
    {
        public static ITaskView<T> CreateInstance(ITask<T> task)
        {
            Type taskType = typeof(T);
            Type formType = typeof(TaskForm<>);
            var genericType = formType.MakeGenericType(taskType);
            var instance = Activator.CreateInstance(genericType, task);
            return (ITaskView<T>)instance;
        }
    }
}