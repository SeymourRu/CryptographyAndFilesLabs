using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CoreDefinitions.Helpers.ConsoleHelper
{
    public class ConsoleHandler
    {
        const uint MF_BYCOMMAND = 0x00000000;
        const uint MF_GRAYED = 0x00000001;
        const uint SC_CLOSE = 0xF060;
        const uint MF_DISABLED = 0x00000002;
        private static Action onExitAction;
        private static bool busy;

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT:
                    Console.WriteLine("CTRL+C received!");
                    break;
                case CtrlTypes.CTRL_BREAK_EVENT:
                    Console.WriteLine("CTRL+BREAK received!");
                    break;
                case CtrlTypes.CTRL_CLOSE_EVENT:
                    Console.WriteLine("Program being closed!");
                    break;
                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    Console.WriteLine("User is logging off!");
                    break;
            }

            onExitAction();
            ConsoleImports.FreeConsole();
            return true;
        }

        public ConsoleHandler(Action onExit)
        {
            onExitAction = onExit;
        }

        public void OpenConsole(Action<bool> switchOn)
        {
            var opened = ConsoleImports.AllocConsole();
            while (!opened)
            {
                ConsoleImports.FreeConsole();
                opened = ConsoleImports.AllocConsole();
            }
            IntPtr hwnd = ConsoleImports.GetConsoleWindow();
            IntPtr hmenu = ConsoleImports.GetSystemMenu(hwnd, false);
            uint hWindow = ConsoleImports.EnableMenuItem(hmenu, SC_CLOSE, MF_BYCOMMAND | MF_DISABLED | MF_GRAYED);
            ConsoleImports.DeleteMenu(hmenu, SC_CLOSE, MF_BYCOMMAND | MF_DISABLED | MF_GRAYED);

            switchOn(false);
        }

        public void SetHandler()
        {
            var res = ConsoleImports.SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            Console.CancelKeyPress += HandleConsole_CancelKeyPress;
        }

        public void RemoveHandler()
        {
            var res = ConsoleImports.SetConsoleCtrlHandler(null, false);
        }

        public void Busy()
        {
            busy = true;
            var res = ConsoleImports.SetConsoleCtrlHandler(null, false);
        }

        public void Free()
        {
            busy = false;
            var res = ConsoleImports.SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
        }

        private static void HandleConsole_CancelKeyPress(object sender, ConsoleCancelEventArgs consoleCancelEventArgs)
        {
            if (busy)
            {
                Console.WriteLine("Busy. Can can not exit right now. Hotkey will be ignored. Wait until the end of the operation (it surely will!)");
                consoleCancelEventArgs.Cancel = true;
            }
        }
    }
}