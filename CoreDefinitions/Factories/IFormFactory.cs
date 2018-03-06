using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreDefinitions.Factories
{
    public interface IFormFactory<T>
    {
        T CreateInstance();
        T GetInstance();
        void Reinit();
    }
}
