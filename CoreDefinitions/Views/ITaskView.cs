using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreDefinitions.Views
{
    public interface ITaskView<T>
    {
        void Init();
        DialogResult ShowDialog();
        void Show();
        void SetVisibility(bool vis);
    }
}
