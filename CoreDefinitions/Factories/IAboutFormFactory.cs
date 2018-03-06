using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDefinitions.Views;

namespace CoreDefinitions.Factories
{
    public interface IAboutFormFactory
    {
        IAboutView CreateInstance();
        IAboutView GetInstance();
        void Reinit();
    }
}
