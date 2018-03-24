using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using CoreDefinitions.Helpers;
using CoreDefinitions.Helpers.ConsoleHelper;
using System.Media;

namespace CoreDefinitions.Tasks
{
    public class SandBox_WinFormsTest : ITask<SandBox_WinFormsTest>, IBaseTask
    {
        TaskAppType _subSystemType;
        public TaskAppType SubSystemType
        {
            get
            {
                return _subSystemType;
            }
        }

        public SandBox_WinFormsTest()
        {
            _subSystemType = Helpers.TaskAppType.GUI;
        }


        public void LocateControls(Form form, ConsoleHandler console)
        {
            form.Text = "Пример приложения на WinForms";
            form.SetDefaultVals(new System.Drawing.Size(400, 200));
            form.Controls.Add(BeautyfyForms.AddButton("Нажми меня!", new Point(160, 80), (o, k) =>
            {
                MessageBox.Show("Sry, no cookies here!");
                SystemSounds.Exclamation.Play();
                PlayMusic();
                MessageBox.Show("That`si all folks!");
            }));
        }

        private void PlayMusic()
        {
            Console.Beep(480, 200);
            Console.Beep(1568, 200);
            Console.Beep(1568, 200);
            Console.Beep(1568, 200);
            Console.Beep(739, 200);
            Console.Beep(783, 200);
            Console.Beep(783, 200);
            Console.Beep(783, 200);

            Console.Beep(369, 200);
            Console.Beep(392, 200);
            Console.Beep(369, 200);
            Console.Beep(392, 200);
            Console.Beep(392, 400);
            Console.Beep(196, 400);

            Console.Beep(739, 200);
            Console.Beep(783, 200);
            Console.Beep(783, 200);
            Console.Beep(739, 200);
            Console.Beep(783, 200);
            Console.Beep(783, 200);
            Console.Beep(739, 200);
            Console.Beep(83, 200);
            Console.Beep(880, 200);
            Console.Beep(830, 200);
            Console.Beep(880, 200);
            Console.Beep(987, 400);
            Console.Beep(880, 200);
            Console.Beep(783, 200);
            Console.Beep(698, 200);
            Console.Beep(739, 200);
            Console.Beep(783, 200);
            Console.Beep(783, 200);
            Console.Beep(739, 200);
            Console.Beep(783, 200);
            Console.Beep(783, 200);
            Console.Beep(739, 200);
            Console.Beep(783, 200);
            Console.Beep(880, 200);
            Console.Beep(830, 200);
            Console.Beep(880, 200);
            Console.Beep(987, 400);
            System.Threading.Thread.Sleep(200);
            Console.Beep(1108, 10);
            Console.Beep(1174, 200);
            Console.Beep(1480, 10);
            Console.Beep(1568, 200);

            System.Threading.Thread.Sleep(200);
            Console.Beep(739, 200);
            Console.Beep(783, 200);
            Console.Beep(783, 200);
            Console.Beep(739, 200);
            Console.Beep(783, 200);
            Console.Beep(783, 200);
            Console.Beep(739, 200);
            Console.Beep(783, 200);
            Console.Beep(880, 200);
            Console.Beep(830, 200);
            Console.Beep(880, 200);
            Console.Beep(987, 400);

            Console.Beep(880, 200);
            Console.Beep(783, 200);
            Console.Beep(698, 200);

            Console.Beep(659, 200);
            Console.Beep(698, 200);
            Console.Beep(784, 200);
            Console.Beep(880, 400);
            Console.Beep(784, 200);
            Console.Beep(698, 200);
            Console.Beep(659, 200);

            Console.Beep(587, 200);
            Console.Beep(659, 200);
            Console.Beep(698, 200);
            Console.Beep(784, 400);
            Console.Beep(698, 200);
            Console.Beep(659, 200);
            Console.Beep(587, 200);

            Console.Beep(523, 200);
            Console.Beep(587, 200);
            Console.Beep(659, 200);
            Console.Beep(698, 400);
            Console.Beep(659, 200);
            Console.Beep(587, 200);
            Console.Beep(493, 200);
            Console.Beep(523, 200);

            System.Threading.Thread.Sleep(400);
            Console.Beep(349, 400);
            Console.Beep(392, 200);
            Console.Beep(329, 200);
            Console.Beep(523, 200);
            Console.Beep(493, 200);
            Console.Beep(466, 200);

            Console.Beep(440, 200);
            Console.Beep(493, 200);
            Console.Beep(523, 200);
            Console.Beep(880, 200);
            Console.Beep(493, 200);
            Console.Beep(880, 200);
            Console.Beep(1760, 200);
            Console.Beep(440, 200);

            Console.Beep(392, 200);
            Console.Beep(440, 200);
            Console.Beep(493, 200);
            Console.Beep(783, 200);
            Console.Beep(440, 200);
            Console.Beep(783, 200);
            Console.Beep(1568, 200);
            Console.Beep(392, 200);

            Console.Beep(349, 200);
            Console.Beep(392, 200);
            Console.Beep(440, 200);
            Console.Beep(698, 200);
            Console.Beep(415, 200);
            Console.Beep(698, 200);
            Console.Beep(1396, 200);
            Console.Beep(349, 200);

            Console.Beep(329, 200);
            Console.Beep(311, 200);
            Console.Beep(329, 200);
            Console.Beep(659, 200);
            Console.Beep(698, 400);
            Console.Beep(783, 400);

            Console.Beep(440, 200);
            Console.Beep(493, 200);
            Console.Beep(523, 200);
            Console.Beep(880, 200);
            Console.Beep(493, 200);
            Console.Beep(880, 200);
            Console.Beep(1760, 200);
            Console.Beep(440, 200);

            Console.Beep(392, 200);
            Console.Beep(440, 200);
            Console.Beep(493, 200);
            Console.Beep(783, 200);
            Console.Beep(440, 200);
            Console.Beep(783, 200);
            Console.Beep(1568, 200);
            Console.Beep(392, 200);

            Console.Beep(349, 200);
            Console.Beep(392, 200);
            Console.Beep(440, 200);
            Console.Beep(698, 200);
            Console.Beep(659, 200);
            Console.Beep(698, 200);
            Console.Beep(739, 200);
            Console.Beep(783, 200);
            Console.Beep(392, 200);
            Console.Beep(392, 200);
            Console.Beep(392, 200);
            Console.Beep(392, 200);
            Console.Beep(196, 200);
            Console.Beep(196, 200);
            Console.Beep(196, 200);

            Console.Beep(185, 200);
            Console.Beep(196, 200);
            Console.Beep(185, 200);
            Console.Beep(196, 200);
            Console.Beep(207, 200);
            Console.Beep(220, 200);
            Console.Beep(233, 200);
            Console.Beep(246, 200);
        }
    }
}
