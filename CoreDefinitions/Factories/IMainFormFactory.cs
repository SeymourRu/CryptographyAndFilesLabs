using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDefinitions.Views;

namespace CoreDefinitions.Factories
{
    public interface IMainFormFactory
    {
        IMainView CreateInstance();
        IMainView GetInstance();
        void Reinit();
    }
}
