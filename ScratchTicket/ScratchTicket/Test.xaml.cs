using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

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
            Image myImage = new Image();
            FormattedText text = new FormattedText("ABC",
                    new CultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface(this.FontFamily, FontStyles.Normal, FontWeights.Normal, new FontStretch()),
                    36,
                    this.Foreground);

            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawText(text, new Point(2, 2));
            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap(180, 180, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            RenderedImage.Source = bmp;
        }
    }
}
