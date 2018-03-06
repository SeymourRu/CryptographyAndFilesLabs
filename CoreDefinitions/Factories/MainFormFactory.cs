using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CoreDefinitions.Views;
using CoreDefinitions.Forms;

namespace CoreDefinitions.Factories
{
    public class MainFormFactory : IMainFormFactory
    {
        Func<IContainer> _container;
        IMainView instance;

        public MainFormFactory(Func<IContainer> container)
        {
            _container = container;
        }

        public IMainView CreateInstance()
        {
            if (instance == null)
            {
                instance = new MainForm(_container());
            }
            return instance;
        }

        public void Reinit()
        {
            instance = null;
            CreateInstance();
        }

        public IMainView GetInstance()
        {
            return instance;
        }
    }
}
