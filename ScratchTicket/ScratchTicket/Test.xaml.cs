using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using ScratchTicket.Helpers;
using System.Windows.Interop;

namespace ScratchTicket
{
    /// <summary>
    /// Test.xaml 的交互逻辑
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
        }
        private void RenderButton_Click(object sender, RoutedEventArgs e)
        {
            //Image myImage = new Image();
            //FormattedText text = new FormattedText("ABC",
            //        new CultureInfo("en-us"),
            //        FlowDirection.LeftToRight,
            //        new Typeface(this.FontFamily, FontStyles.Normal, FontWeights.Normal, new FontStretch()),
            //        36,
            //        this.Foreground);

            //DrawingVisual drawingVisual = new DrawingVisual();
            //DrawingContext drawingContext = drawingVisual.RenderOpen();
            //drawingContext.DrawText(text, new Point(2, 2));
            //drawingContext.Close();

            //RenderTargetBitmap bmp = new RenderTargetBitmap(180, 180, 96, 96, PixelFormats.Pbgra32);
            //bmp.Render(drawingVisual);
            //RenderedImage.Source = bmp;

            //往位图中的指定位置写入指定像素
            //BitmapSource bitmapSource = RenderedImage.Source as BitmapSource;
            //int width = bitmapSource.PixelWidth;
            //int height = bitmapSource.PixelHeight;
            //System.Diagnostics.Debug.WriteLine(bitmapSource.Format);
            //// 创建一个WriteableBitmap对象
            //WriteableBitmap bitmap = new WriteableBitmap(bitmapSource);

            //// 锁定位图的像素数据
            //bitmap.Lock();

            //// 获取位图的像素数据的指针
            //IntPtr pixelData = bitmap.BackBuffer;

            //// 遍历位图的像素数据
            //for (int y = 30; y < 50; y++)
            //{
            //    for (int x = 30; x < 50; x++)
            //    {
            //        // 计算像素的位置
            //        int index = (y * width + x) * 4;

            //        // 设置像素的颜色
            //        Marshal.WriteByte(pixelData, index + 0, 0); // B
            //        Marshal.WriteByte(pixelData, index + 1, 0); // G
            //        Marshal.WriteByte(pixelData, index + 2, 0); // R
            //        Marshal.WriteByte(pixelData, index + 3, 255); // A
            //    }
            //}
            //// 解锁位图的像素数据
            //bitmap.Unlock();

            //// 将位图显示在Image控件上
            //RenderedImage.Source = bitmap;

            //自绘位图
            // 创建一个 DrawingVisual 对象
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                // 绘制白色背景
                drawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, 200, 100));

                // 绘制黑色边框
                drawingContext.DrawRectangle(null, new Pen(Brushes.Black, 2), new Rect(0, 0, 200, 100));

                // 设置字体和绘制文本
                FormattedText formattedText = new FormattedText(
                    "Hello World",
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    16, // 字体大小
                    Brushes.Black,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip); // 获取 DPI

                // 计算文本位置以使其居中
                double textX = (200 - formattedText.Width) / 2;
                double textY = (100 - formattedText.Height) / 2;

                // 绘制文本
                drawingContext.DrawText(formattedText, new Point(textX, textY));
            }

            // 将 DrawingVisual 渲染到 WriteableBitmap
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(200, 100, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(drawingVisual);
            var wbmp = new WriteableBitmap(renderTargetBitmap);
            RenderedImage.Source = wbmp;
        }

        private void ChgCursorButton_Click(object sender, RoutedEventArgs e)
        {
                
        }
    }
}
