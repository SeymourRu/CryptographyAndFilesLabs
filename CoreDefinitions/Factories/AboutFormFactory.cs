using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreDefinitions.Views;
using CoreDefinitions.Forms;

namespace CoreDefinitions.Factories
{
    public class AboutFormFactory : IAboutFormFactory
    {
        private IAboutView instance;

        public IAboutView CreateInstance()
        {
            if (instance == null)
            {
                instance = new AboutForm();
            }
            return instance;
        }

        public void Reinit()
        {
            instance = null;
            CreateInstance();
        }

        public IAboutView GetInstance()
        {
            return instance;
        }
    }
}
