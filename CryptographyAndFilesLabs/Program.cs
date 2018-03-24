using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Autofac.Builder;
using CoreDefinitions.Views;
using CoreDefinitions.Factories;
using CoreDefinitions.Forms;
using CoreDefinitions.Tasks;

namespace CryptographyAndFilesLabs
{
    static class Program
    {

        public static IContainer _container;

        private static void LoadCryptography(ContainerBuilder builder)
        {
            builder.RegisterType<Crypto_Task1_1>().As<ITask<Crypto_Task1_1>>().As<IBaseTask>();
            builder.RegisterType<Crypto_Task1_2>().As<ITask<Crypto_Task1_2>>().As<IBaseTask>();
            builder.RegisterType<Crypto_Task1_3>().As<ITask<Crypto_Task1_3>>().As<IBaseTask>();

            builder.RegisterType<Crypto_Task2_1>().As<ITask<Crypto_Task2_1>>().As<IBaseTask>();
            //builder.RegisterType<Crypto_Task2_2>().As<ITask<Crypto_Task2_2>>().As<IBaseTask>();
        }

        private static void LoadFilesProcessing(ContainerBuilder builder)
        {
            builder.RegisterType<Files_Task1_1>().As<ITask<Files_Task1_1>>().As<IBaseTask>();
            builder.RegisterType<Files_Task1_2>().As<ITask<Files_Task1_2>>().As<IBaseTask>();
            builder.RegisterType<Files_Task1_3>().As<ITask<Files_Task1_3>>().As<IBaseTask>();

            builder.RegisterType<Files_Task2_1>().As<ITask<Files_Task2_1>>().As<IBaseTask>();
            builder.RegisterType<Files_Task2_2>().As<ITask<Files_Task2_2>>().As<IBaseTask>();
        }

        private static void LoadSandbox(ContainerBuilder builder)
        {
            builder.RegisterType<Sandbox_ConsoleTest>().As<ITask<Sandbox_ConsoleTest>>().As<IBaseTask>();
            builder.RegisterType<SandBox_WinFormsTest>().As<ITask<SandBox_WinFormsTest>>().As<IBaseTask>();
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainFormFactory>().As<IMainFormFactory>();
            builder.RegisterType<AboutFormFactory>().As<IAboutFormFactory>();
            //builder.RegisterGeneric(typeof(TaskFormFactory<>)).As(typeof(ITaskFormFactory<>));
            //builder.RegisterGeneric(typeof(FormFactory<>)).As(typeof(IFormFactory<>)).InstancePerDependency();

            LoadCryptography(builder);
            LoadFilesProcessing(builder);
            LoadSandbox(builder);

            Func<IContainer> factory = () => _container;
            builder.RegisterInstance(factory);

            return builder.Build();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _container = BuildContainer();

            var mainForm = _container.Resolve<IMainFormFactory>();
            Application.Run((Form)mainForm.CreateInstance());
        }
    }
}