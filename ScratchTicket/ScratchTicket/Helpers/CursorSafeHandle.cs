using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScratchTicket.Helpers
{
    public class CursorSafeHandle : SafeHandle
    {
        public CursorSafeHandle(IntPtr preexistingHandle, bool ownsHandle)
            : base(IntPtr.Zero, ownsHandle)
        {
            handle = preexistingHandle;
        }
        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
        }
        protected override bool ReleaseHandle()
        {
            return true;
        }

        public static Bitmap CreateCustomRegion()
        {
            var backBitmap = new Bitmap(20, 20);
            using (Graphics graphics = Graphics.FromImage(backBitmap))
            {
                // 设置背景色
                graphics.Clear(System.Drawing.Color.Transparent);
                // 绘制圆
                Brush greenBrush = new SolidBrush(Color.Green);
                graphics.FillRectangle(greenBrush, 0, 0, backBitmap.Width, backBitmap.Height);
                graphics.Dispose();
            }
            return backBitmap;
        }
    }

}
