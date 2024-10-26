using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchTicket.Helpers
{
    public class Common
    {
        public static int[] GetIntersection(int[] A, int[] B)
        {
            // 确保数组 A 和 B 都有两个元素
            if (A.Length != 2 || B.Length != 2)
                throw new ArgumentException("Both input arrays must contain exactly two elements.");

            // 找到交集的起始点和结束点
            int start = Math.Max(A[0], B[0]);
            int end = Math.Min(A[1], B[1]);

            // 检查是否有交集
            if (start <= end)
            {
                return new int[] { start, end };
            }
            else
            {
                return null; // 返回空数组表示没有交集
            }
        }
        /// <summary>
        /// 判断两个int指针的连续numBytes个字节的数据是否相同
        /// </summary>
        /// <param name="ptr1"></param>
        /// <param name="ptr2"></param>
        /// <param name="numBytes"></param>
        /// <returns></returns>
        public static unsafe bool CompareIntPtrData(IntPtr ptr1, IntPtr ptr2, int numBytes)
        {
            // 使用 unsafe 代码块进行比较
            byte* p1 = (byte*)ptr1;
            byte* p2 = (byte*)ptr2;

            for (int i = 0; i < numBytes; i++)
            {
                if (p1[i] != p2[i])
                {
                    return false; // 一旦发现不同，返回 false
                }
            }

            return true; // 所有字节都相等
        }
    }
}
