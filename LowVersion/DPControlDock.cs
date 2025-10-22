using System;
using System.Windows.Forms;
using Su.Revit.UI.StatusBarEx.LowVersion.Forms;

namespace Su.Revit.UI.StatusBarEx.LowVersion
{
    internal class DPControlDock
    {
        private UserControl _control;
        public DPControl _dpControl;

        public DPControlDock(UserControl userControl)
        {
            _control = userControl;
        }

        public void SetDock(IntPtr hWnd)
        {
            if (hWnd == null)
                throw new Exception();
            var dpInstance = new DPControl();
            dpInstance.Controls.Clear();
            _control.BackColor = System.Drawing.Color.Transparent;
            _control.Dock = DockStyle.Fill;
            _control.Parent = dpInstance;
            var wh = DllImportHelper.GetWH(hWnd);
            DllImportHelper.SetParent(dpInstance.Handle, hWnd);
            DllImportHelper.MoveWindow(dpInstance.Handle, 0, 0, (int)wh.Item1, 30, true);
            _dpControl = dpInstance;
        }
    }
}
