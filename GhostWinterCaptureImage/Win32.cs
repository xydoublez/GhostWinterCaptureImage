using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace GhostWinterCaptureImage
{
    
    public class Win32
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]//获取桌面的句柄
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]//在桌面找寻子窗体
        public static extern IntPtr ChildWindowFromPointEx(IntPtr pHwnd, LPPOINT pt, uint uFlgs);
        public const int CWP_SKIPDISABLED = 0x2;   //忽略不可用窗体
        public const int CWP_SKIPINVISIBL = 0x1;   //忽略隐藏的窗体
        public const int CWP_All = 0x0;            //一个都不忽略
        
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        public const uint WM_LBUTTONUP = 0x202;

        [DllImport("user32.dll")]//进行坐标转换 （再窗体内部进行查找）
        public static extern bool ScreenToClient(IntPtr hWnd, out LPPOINT lpPoint);

        /// <summary>
        /// 注册全局热键
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="id"></param>
        /// <param name="fsModifiers"></param>
        /// <param name="vk"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        /// <summary>
        /// //卸载全局热键
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        public struct LPPOINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]//获得句柄对象的位置
        public static extern bool GetWindowRect(IntPtr hWnd, out LPRECT lpRect);

        public struct LPRECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorInfo(out PCURSORINFO pci);

        public struct PCURSORINFO
        {
            public int cbSize;
            public int flag;
            public IntPtr hCursor;
            public LPPOINT ptScreenPos;
        }
    }
}
