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

        public static Bitmap CreateRectRegion(int side)
        {
            var backBitmap = new Bitmap(side, side);
            var tmpBmp = new Bitmap(side*2, side*2);
            using (Graphics graphics = Graphics.FromImage(tmpBmp))
            {
                // 设置背景色
                graphics.Clear(System.Drawing.Color.Transparent);
                // 绘制矩形
                Brush greenBrush = new SolidBrush(Color.DarkGray);
                graphics.FillRectangle(greenBrush, backBitmap.Width, backBitmap.Height, backBitmap.Width, backBitmap.Height);
            }
            return tmpBmp;
        }
    }

}
