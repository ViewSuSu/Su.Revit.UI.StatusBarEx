#if RVT_26||RVT_25||RVT_24||RVT_23||RVT_22||RVT_21||RVT_20||RVT_19||RVT_18||RVT_17||RVT_16||RVT_15||RVT_14||RVT_13||RVT_12||RVT_11

#else
using Autodesk.Revit.UI;
using ricaun.Revit.UI;
using Su.Revit.UI.StatusBarEx.Test;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace Su.Revit.UI.StatusBarEx
{
    internal class App : IExternalApplication
    {
        private static RibbonPanel ribbonPanel;

        public Result OnStartup(UIControlledApplication application)
        {
            ribbonPanel = application.CreatePanel("StatusBarEx");
            ribbonPanel.RowStackedItems(
                ribbonPanel.CreatePushButton<TestCommand>(typeof(TestCommand).Name),
                ribbonPanel.CreatePushButton<TestCommand2>(typeof(TestCommand2).Name)
            );
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            ribbonPanel?.Remove();
            return Result.Succeeded;
        }
    }
}
#endif
