using ScratchTicket.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScratchTicket.Controls
{
    /// <summary>
    /// CardHolder.xaml 的交互逻辑
    /// </summary>
    public partial class CardHolder : UserControl
    {
        [TypeConverter(typeof(LengthConverter))]
        public double Price
        {
            get { return (double)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Price.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PriceProperty =
            DependencyProperty.Register("Price", typeof(double), typeof(CardHolder), new PropertyMetadata(0.00));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(CardHolder), new PropertyMetadata(null));


        [TypeConverter(typeof(Visibility2BoolConverter))]
        public bool HidePrice
        {
            get { return (bool)GetValue(HidePriceProperty); }
            set { SetValue(HidePriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HidePrice.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HidePriceProperty =
            DependencyProperty.Register("HidePrice", typeof(bool), typeof(CardHolder), new PropertyMetadata(false));

        // 定义一个 RoutedEvent
        public static readonly RoutedEvent CardClickEvent = EventManager.RegisterRoutedEvent(
            "CardClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CardHolder));

        // 提供添加和移除事件处理程序的方法
        public event RoutedEventHandler CardClick
        {
            add { AddHandler(CardClickEvent, value); }
            remove { RemoveHandler(CardClickEvent, value); }
        }

        public CardHolder()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(CardClickEvent);
            RaiseEvent(args);
        }
    }
}
