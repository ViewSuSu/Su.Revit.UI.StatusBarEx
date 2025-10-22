using System;
using System.Runtime.InteropServices;

namespace Su.Revit.UI.StatusBarEx.LowVersion
{
    internal static class DllImportHelper
    {
        internal const int DlgID = 0x000EC801;

        [DllImport("user32")]
        internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndParent);

        [DllImport("user32")]
        internal static extern IntPtr GetDlgItem(IntPtr hWndParent, int cid);

        [DllImport("user32")]
        internal static extern bool MoveWindow(
            IntPtr hWnd,
            int x,
            int y,
            int width,
            int height,
            bool repaint
        );

        [DllImport("user32")]
        internal static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);

        internal static Tuple<double, double> GetWH(IntPtr hWnd)
        {
            var result = default(Rect);

            DllImportHelper.GetClientRect(hWnd, out result);
            return new Tuple<double, double>(
                result.right - result.left,
                result.bottom - result.top
            );
        }

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindowEx(
            IntPtr hwndParent,
            IntPtr hwndChildAfter,
            string lpszClass,
            string lpszWindow
        );

        internal struct Rect
        {
            public int left;

            public int top;

            public int right;

            public int bottom;
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int SetWindowText(IntPtr hWnd, string lpString);

        internal static void SetStatusText(IntPtr hWnd, string text)
        {
            IntPtr statusBar = FindWindowEx(hWnd, IntPtr.Zero, "msctls_statusbar32", "");
            if (statusBar != IntPtr.Zero)
            {
                SetWindowText(statusBar, text);
            }
        }

        internal const int SW_HIDE = 0;
        private static readonly IntPtr HWND_BOTTOM = (IntPtr)1;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags
        );

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        // 现有的常量和方法...

        public const int SW_SHOW = 5;
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_CHILD = 0x40000000;
        public const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);
    }
}
