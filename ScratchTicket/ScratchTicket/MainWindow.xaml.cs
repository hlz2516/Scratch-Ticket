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

        public MainWindow(ILogger _logger)
        {
            logger = _logger;
        }
    }
}
