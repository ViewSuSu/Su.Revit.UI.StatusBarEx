using UIFramework;

namespace Su.Revit.UI.StatusBarEx.Utils
{
    /// <summary>
    /// RevitRibbonController
    /// </summary>
    internal class RevitRibbonController
    {
        /// <summary>
        /// RibbonControl
        /// </summary>
        public static RevitRibbonControl RibbonControl =>
            UIFramework.RevitRibbonControl.RibbonControl;

        /// <summary>
        /// Enable
        /// </summary>
        public static void Enable() => RibbonControl.IsEnabled = true;

        /// <summary>
        /// Disable
        /// </summary>
        public static void Disable() => RibbonControl.IsEnabled = false;
    }
}
