using Autofac;
using NLog;
using ScratchTicket.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ScratchTicket
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger logger;
        private Storyboard storyboard1;
        public event Action<double> AssetChanged;

        public MainWindow()
        {
            InitializeComponent();
            AssetChangeAnimationInit();
            AssetChanged += MainWindow_AssetChanged;
        }

        private void MainWindow_AssetChanged(double obj)
        {
            double curAsset = double.Parse(tbAssets.Text);
            curAsset += obj;
            tbAssets.Text = curAsset.ToString("F2");
        }

        public MainWindow(ILogger _logger):this()
        {
            logger = _logger;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void AssetChangeAnimationInit()
        {
            storyboard1 = new Storyboard();
            var duration = TimeSpan.FromMilliseconds(250);
            DoubleAnimation doubleAnimation1 = new DoubleAnimation();
            doubleAnimation1.Duration = new Duration(duration); // 翻页动画持续时间
            storyboard1.Children.Add(doubleAnimation1);

            DoubleAnimation doubleAnimation2 = new DoubleAnimation();
            storyboard1.Children.Add(doubleAnimation2);
            doubleAnimation2.BeginTime = duration;
            doubleAnimation2.Duration = duration; // 翻页动画持续时间

            Storyboard.SetTarget(doubleAnimation1, txtVar);
            Storyboard.SetTargetProperty(doubleAnimation1, new PropertyPath("(Canvas.Top)"));
            Storyboard.SetTarget(doubleAnimation2, txtVar);
            Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath("(Canvas.Top)"));

            DoubleAnimation doubleAnimation3 = new DoubleAnimation();
            doubleAnimation3.From = 0; // 开始位置
            doubleAnimation3.To = 1; // 结束位置
            doubleAnimation3.BeginTime = TimeSpan.FromMilliseconds(0);
            doubleAnimation3.Duration = new Duration(duration);
            storyboard1.Children.Add(doubleAnimation3);

            DoubleAnimation doubleAnimation4 = new DoubleAnimation();
            doubleAnimation4.From = 1; // 开始位置
            doubleAnimation4.To = 0; // 结束位置
            doubleAnimation4.BeginTime = duration;
            doubleAnimation4.Duration = duration;
            storyboard1.Children.Add(doubleAnimation4);

            Storyboard.SetTarget(doubleAnimation3, cvs1);
            Storyboard.SetTargetProperty(doubleAnimation3, new PropertyPath("Opacity"));
            Storyboard.SetTarget(doubleAnimation4, cvs1);
            Storyboard.SetTargetProperty(doubleAnimation4, new PropertyPath("Opacity"));
        }

        private bool isSubscribed = false;
        private EventHandler eventHandler = null;
        public void StartAssetChangeAnimation(double variable)
        {
            DoubleAnimation doubleAnimation1 = storyboard1.Children[0] as DoubleAnimation;
            DoubleAnimation doubleAnimation2 = storyboard1.Children[1] as DoubleAnimation;

            if (eventHandler != null)
            {
                doubleAnimation1.Completed -= eventHandler;
            }
            eventHandler = (sender, eve) =>
            {
                AssetChanged?.Invoke(variable);
            };
            doubleAnimation1.Completed += eventHandler;

            if (variable > 0)
            {
                txtVar.Foreground = new SolidColorBrush(Colors.LightGreen);
                doubleAnimation1.From = cvs1.ActualHeight; // 开始位置
                doubleAnimation1.To = (cvs1.ActualHeight - txtVar.ActualHeight) / 2; // 结束位置
            }
            else if (variable < 0)
            {
                txtVar.Foreground = new SolidColorBrush(Colors.Red);
                doubleAnimation1.From = -txtVar.ActualHeight; // 开始位置
                doubleAnimation1.To = (cvs1.ActualHeight - txtVar.ActualHeight) / 2; // 结束位置
            }

            if (variable > 0)
            {
                doubleAnimation2.From = (cvs1.ActualHeight - txtVar.ActualHeight) / 2; // 开始位置
                doubleAnimation2.To = -txtVar.ActualHeight; // 结束位置
            }
            else if (variable < 0)
            {
                doubleAnimation2.From = (cvs1.ActualHeight - txtVar.ActualHeight) / 2; // 开始位置
                doubleAnimation2.To = cvs1.ActualHeight; // 结束位置
            }

            storyboard1.Begin();
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //StartAssetChangeAnimation(-100);
        }

        private void CardHolder_CardClick(object sender, RoutedEventArgs e)
        {
            var card = sender as CardHolder;
            if (card != null)
            {
                txtVar.Text = (-card.Price).ToString("F2");
                StartAssetChangeAnimation(-card.Price);
            }
        }
    }
}
