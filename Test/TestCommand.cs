using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Su.Revit.UI.StatusBarEx.Test
{
    [Transaction(TransactionMode.Manual)]
    internal class TestCommand : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elementSet
        )
        {
            UIApplication uiapp = commandData.Application;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ProgressBarExUtils.Run(
                100,
                i =>
                {
                    System.Threading.Thread.Sleep(i);
                }
            );
            Console.WriteLine($"Stopwatch: {stopwatch.ElapsedMilliseconds}");
            return Result.Succeeded;
        }
    }
}
