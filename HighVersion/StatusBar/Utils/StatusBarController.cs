using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Su.Revit.UI.StatusBarEx.HighVersion.StatusBar.Extensions;
using UIFramework;
using Grid = System.Windows.Controls.Grid;
using Visibility = System.Windows.Visibility;

namespace Su.Revit.UI.StatusBarEx.HighVersion.StatusBar.Utils
{
    /// <summary>
    /// StatusBarController
    /// </summary>
    internal class StatusBarController
    {
        private static Grid RootGrid;
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D

#else
        private static DialogBarControl InternalControl;

        /// <summary>
        /// Default StatusBar is visible
        /// </summary>
        public static bool IsVisible => InternalControl.Visibility == Visibility.Visible;
#endif
        private static ContentPresenter _controlPresenter;

        static StatusBarController()
        {
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D

#else
            RootGrid = VisualTreeHelperUtils.FindChild<Grid>(MainWindow.getMainWnd(), "rootGrid");
            if (RootGrid is null)
                throw new InvalidOperationException("Cannot find root grid in Revit UI");
            // InternalControl = VisualTreeHelperUtils.FindChild<DialogBarControl>(RootGrid, "statusBar");
            var dialogBarControls = VisualTreeHelperUtils.FindChildren<DialogBarControl>(RootGrid);
            InternalControl = dialogBarControls.FirstOrDefault(x => x.Name != "statusBar");
            if (InternalControl is null)
                throw new InvalidOperationException("Cannot find internal control in Revit UI");
#endif
        }

        /// <summary>
        /// Show
        /// </summary>
        /// <param name="content"></param>
        public static void Show(FrameworkElement content)
        {
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D

#else
            InternalControl.Visibility = Visibility.Hidden;
            if (_controlPresenter is null)
            {
                _controlPresenter = new ContentPresenter();
                RootGrid.Children.Add(_controlPresenter);
            }
            _controlPresenter.Content = content;
            Grid.SetRow(_controlPresenter, Grid.GetRow(InternalControl));
#endif
        }

        /// <summary>
        /// Hide
        /// </summary>
        public static void Hide()
        {
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D

#else
            InternalControl.Visibility = Visibility.Visible;
            if (_controlPresenter is null)
                return;

            RootGrid.Children.Remove(_controlPresenter);
            _controlPresenter = null;
#endif
        }
    }
}
