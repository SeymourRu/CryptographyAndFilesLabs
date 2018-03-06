using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDefinitions.Views;
using System.Windows.Forms;

namespace CoreDefinitions.Factories
{
    public class FormFactory<T> : IFormFactory<T> where T : new()
    {
        T instance = default(T);

        public T CreateInstance()
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }

        public void Reinit()
        {
            instance = default(T);
            CreateInstance();
        }

        public T GetInstance()
        {
            return instance;
        }
    }
}
