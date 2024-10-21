using Autofac;
using NLog;
using System;
using System.Windows;

namespace ScratchTicket
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger logger;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(ILogger _logger):this()
        {
            logger = _logger;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
