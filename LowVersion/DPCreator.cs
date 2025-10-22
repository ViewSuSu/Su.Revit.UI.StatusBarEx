using System;
using Autodesk.Windows;
using Su.Revit.UI.StatusBarEx.LowVersion.Forms;

namespace Su.Revit.UI.StatusBarEx.LowVersion
{
    internal class DPCreator
    {
        private static DPControl _dpControl;
        public static IntPtr hWnd;

        //public static IntPtr statusBar;
        private static DPControlDock dpDock;

        internal static void Create(System.Windows.Forms.UserControl userControl)
        {
            dpDock = new DPControlDock(userControl);
            hWnd = DllImportHelper.GetDlgItem(
                ComponentManager.ApplicationWindow,
                DllImportHelper.DlgID
            );
            if (hWnd == null)
                throw new Exception();
            dpDock.SetDock(hWnd);
            _dpControl = dpDock._dpControl;
        }

        internal static void Show()
        {
            _dpControl.TopMost = true;
            _dpControl.Show();
        }

        internal static void Close()
        {
            _dpControl.Close();
        }

        internal static void TopMost()
        {
            SetWindowTopMost();
        }

        /// <summary>
        /// 设置_dpControl始终位于hWnd窗口的最前面
        /// </summary>
        private static void SetWindowTopMost()
        {
            if (_dpControl == null || hWnd == IntPtr.Zero)
                return;
            _dpControl.TopMost = true;
            //   _dpControl.TopMost = true;
            // DllImportHelper.SetStatusText(ComponentManager.ApplicationWindow,null);
        }
    }
}
