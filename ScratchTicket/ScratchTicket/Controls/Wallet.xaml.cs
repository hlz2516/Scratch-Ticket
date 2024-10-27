using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ScratchTicket.Controls
{
    /// <summary>
    /// Wallet.xaml 的交互逻辑
    /// </summary>
    public partial class Wallet : UserControl
    {
        private Storyboard storyboard1;

        public double Money
        {
            get { return (double)GetValue(MoneyProperty); }
            set { SetValue(MoneyProperty, value); }
        }
        public Wallet()
        {
            InitializeComponent();
            container.DataContext = this;
            AssetChangeAnimationInit();
        }

        private void AssetChangeAnimationInit()
        {
            storyboard1 = new Storyboard();
            var duration = TimeSpan.FromMilliseconds(250);
            DoubleAnimation doubleAnimation1 = new DoubleAnimation();
            doubleAnimation1.Duration = new Duration(duration);
            storyboard1.Children.Add(doubleAnimation1);

            DoubleAnimation doubleAnimation2 = new DoubleAnimation();
            storyboard1.Children.Add(doubleAnimation2);
            doubleAnimation2.BeginTime = duration;
            doubleAnimation2.Duration = duration;

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

        public void StartAssetChangeAnimation(double variable)
        {
            DoubleAnimation doubleAnimation1 = storyboard1.Children[0] as DoubleAnimation;
            DoubleAnimation doubleAnimation2 = storyboard1.Children[1] as DoubleAnimation;

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

        // Using a DependencyProperty as the backing store for Money.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoneyProperty =
            DependencyProperty.Register("Money", typeof(double), typeof(Wallet), new PropertyMetadata(0.00,new PropertyChangedCallback(MoneyChanged)));

        private static bool isFirst = true;
        private static void MoneyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (isFirst)
            {
                isFirst = false;
                return;
            }
            double diff = (double)e.NewValue - (double)e.OldValue;
            Wallet inst = d as Wallet;
            if (inst != null)
            {
                inst.txtVar.Text = diff >= 0 ? "+" + diff.ToString() : diff.ToString();
                inst.StartAssetChangeAnimation(diff);
            }
        }
    }
}
