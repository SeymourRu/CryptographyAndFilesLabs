using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;


namespace CoreDefinitions.Tasks.Cryptography
{
    class Crypto_Task2_1 : ITask<Crypto_Task2_1>, IBaseTask
    {
        TaskAppType _subSystemType;
        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public Crypto_Task2_1()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }
    }
}
