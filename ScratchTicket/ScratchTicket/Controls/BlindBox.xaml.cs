using ScratchTicket.Helpers;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScratchTicket.Controls
{
    /// <summary>
    /// BlindBox.xaml 的交互逻辑
    /// </summary>
    public partial class BlindBox : UserControl
    {
        private WriteableBitmap moneyVisual; //要覆盖的板
        private WriteableBitmap transparentBackground;  //被覆盖的板
        private readonly int range = 10;
        private int[] hRange;
        private int[] vRange;
        private bool dragging = false;
        public double Money
        {
            get { return (double)GetValue(MoneyProperty); }
            private set { SetValue(MoneyProperty, value); }
        }
        public double MoneyMaxLimit
        {
            get { return (double)GetValue(MoneyMaxLimitProperty); }
            set { SetValue(MoneyMaxLimitProperty, value); }
        }

        public BlindBox()
        {
            InitializeComponent();
            container.DataContext = this;
            //设置光标
            var bmp = CursorSafeHandle.CreateRectRegion(range);
            var handle = bmp.GetHicon();
            var c = new CursorSafeHandle(handle, true);
            var cursor = CursorInteropHelper.Create(c);
            this.Cursor = cursor;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            dragging = true;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging)
            {
                return;
            }
            var pos = e.GetPosition(img);
            int X = (int)pos.X;
            int Y = (int)pos.Y;
            //获取刷新区间，包括水平和竖直方向
            //计算点击时的预区间
            int[] hPreRange = new int[2] { X, X + range };
            int[] vPreRange = new int[2] { Y, Y + range };
            //取有效区间与预区间的交集，有效区间指的是[0,img.Width]和[0,img.Height]，注意这里是像素区间（没有*4）
            var hValidRange = Common.GetIntersection(hRange, hPreRange);
            var vValidRange = Common.GetIntersection(vRange, vPreRange);
            if (hValidRange == null || vValidRange == null)
            {
                return;
            }
            // 锁定位图的像素数据
            transparentBackground.Lock();
            // 获取位图的像素数据的指针
            IntPtr transData = transparentBackground.BackBuffer;
            IntPtr moneyData = moneyVisual.BackBuffer;
            int validVRange = vValidRange[1] - vValidRange[0];
            int validHRange = hValidRange[1] - hValidRange[0];
            for (int y = vValidRange[0]; y < vValidRange[1]; y++)
            {
                IntPtr tmpMoneyData = moneyData + y * moneyVisual.BackBufferStride + hValidRange[0] * 4;
                IntPtr tmpTransData = transData + y * moneyVisual.BackBufferStride + hValidRange[0] * 4;
                byte[] buffer = new byte[validHRange * 4];
                Marshal.Copy(tmpMoneyData, buffer, 0, validHRange * 4);
                Marshal.Copy(buffer, 0, tmpTransData, validHRange * 4);
            }
            //指定更改区域
            transparentBackground.AddDirtyRect(new Int32Rect(hValidRange[0], vValidRange[0], validHRange, validVRange));
            // 解锁位图的像素数据
            transparentBackground.Unlock();
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            dragging = false;
            //判定两个板的像素是否一致，如果一致则触发完成事件
            if (BitmapEqual(transparentBackground,moneyVisual))
            {
                RoutedEventArgs args = new RoutedEventArgs(DoneEvent);
                RaiseEvent(args);
            }
        }

        private bool BitmapEqual(WriteableBitmap map1,WriteableBitmap map2)
        {
            bool equal = true;
            //先校验宽高是否一致
            if (map1.PixelWidth != map2.PixelWidth || map1.PixelHeight != map2.PixelHeight)
            {
                return false;
            }
            //采用隔行校验法
            IntPtr data1 = map1.BackBuffer;
            IntPtr data2 = map2.BackBuffer;
            for (int i = 0;i < map1.Height; i += 2)
            {
                bool rowEqual = Common.CompareIntPtrData(data1, data2, map1.BackBufferStride);
                if (!rowEqual)
                {
                    equal = false;
                    break;
                }
                data1 += map1.BackBufferStride * 2;
                data2 += map2.BackBufferStride * 2;
            }
            return equal;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Money = new Random().NextDouble() * MoneyMaxLimit;

            hRange = new int[2] { 0, (int)Width };
            vRange = new int[2] {0, (int)Height };
            //生成一副透明图像作为背景
            int width = (int)Width;
            int height = (int)Height;
            transparentBackground = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
            int[] pixels = new int[width * height];

            // 将所有像素设置为透明
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = 0x00000000; // ARGB格式，0x00表示完全透明
            }

            // 更新 WriteableBitmap 的像素
            transparentBackground.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);

            // 将生成的透明图像设置为 Image 控件的 Source
            img.Source = transparentBackground;

            //根据金额生成对应图像数据存储在内存中
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                // 绘制白色背景
                drawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, Width, Height));
                // 绘制黑色边框
                drawingContext.DrawRectangle(null, new Pen(Brushes.Black, 2), new Rect(0, 0, Width, Height));

                // 设置字体和绘制文本
                FormattedText formattedText = new FormattedText(
                    Math.Round(Money, 1, MidpointRounding.AwayFromZero).ToString(),
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    14, // 字体大小
                    Brushes.Black,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip); // 获取 DPI

                // 计算文本位置以使其居中
                double textX = (Width - formattedText.Width) / 2;
                double textY = (Height - formattedText.Height) / 2;

                // 绘制文本
                drawingContext.DrawText(formattedText, new Point(textX, textY));
            }

            // 将 DrawingVisual 渲染到 WriteableBitmap
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)Width, (int)Height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(drawingVisual);
            moneyVisual = new WriteableBitmap(renderTargetBitmap);
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //直接揭晓数目
            img.Source = moneyVisual;
            transparentBackground = moneyVisual.CloneCurrentValue();
            RoutedEventArgs args = new RoutedEventArgs(DoneEvent);
            RaiseEvent(args);
        }
        // Using a DependencyProperty as the backing store for Money.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoneyProperty =
            DependencyProperty.Register("Money", typeof(double), typeof(BlindBox), new PropertyMetadata(0.00));

        // Using a DependencyProperty as the backing store for MoneyMaxLimit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoneyMaxLimitProperty =
            DependencyProperty.Register("MoneyMaxLimit", typeof(double), typeof(BlindBox), new PropertyMetadata(100.00));

        public static readonly RoutedEvent DoneEvent =
            EventManager.RegisterRoutedEvent(
                "Done",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(BlindBox));

        public event RoutedEventHandler Done
        {
            add { AddHandler(DoneEvent, value); }
            remove { RemoveHandler(DoneEvent, value); }
        }
    }
}
