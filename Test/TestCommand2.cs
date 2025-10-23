using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Su.Revit.UI.StatusBarEx.Test
{
    [Transaction(TransactionMode.Manual)]
    internal class TestCommand2 : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elementSet
        )
        {
            UIApplication uiapp = commandData.Application;
            Stopwatch stopwatch = new Stopwatch();
            Document doc = uiapp.ActiveUIDocument.Document;
            stopwatch.Start();
            var walls = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .WhereElementIsNotElementType()
                .Cast<Wall>();
            using (var tx = new Transaction(doc, "批量修改"))
            {
                tx.Start();
                ProgressBarExUtils.RunCancelable(
                    transaction: tx,
                    sources: walls,
                    loopAction: wall =>
                    {
                        Thread.Sleep(100);
                        wall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
                            ?.Set("批量处理完成");
                    }
                );
                if (tx.GetStatus() == TransactionStatus.Started)
                {
                    tx.Commit();
                }
            }
            Console.WriteLine($"Stopwatch: {stopwatch.ElapsedMilliseconds}");
            return Result.Succeeded;
        }
    }
}
