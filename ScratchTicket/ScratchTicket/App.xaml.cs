using Autofac;
using Autofac.Core;
using NLog;
using ScratchTicket.Helpers;
using ScratchTicket.ORM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ScratchTicket
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static IContainer Container { private set; get; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();

            // 注册窗口和服务
            builder.Register(c=>LogManager.GetCurrentClassLogger()).As<ILogger>();
            builder.RegisterType<Login>().SingleInstance(); //注册登录窗口，每次加载实例时都是返回同一个实例
            builder.RegisterType<LoginViewModel>().SingleInstance();
            builder.RegisterType<MainWindow>().SingleInstance();
            builder.RegisterType<MainWindowViewModel>().SingleInstance();
            builder.RegisterType<Test>().SingleInstance();

            Container = builder.Build();

            // 显示登录窗口
            var login = Container.Resolve<Test>();
            login.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Container?.Dispose();
            base.OnExit(e); 
        }
    }
}
