using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HYBIM.Revit.FrameworkBase.Utils.ProgressBar.HighVersion.StatusBar
{
    /// <summary>
    /// RevitProgressBar
    /// </summary>
    internal class RevitProgressBar : IDisposable
    {
        private readonly Stopwatch stopwatch;
        private readonly Su.Revit.UI.StatusBarEx.HighVersion.StatusBar.Controls.ProgressBarStackPanel progressBarStackPanel;

        /// <summary>
        /// RevitProgressBar
        /// </summary>
        /// <param name="hasCancelButton"></param>
        public RevitProgressBar(bool hasCancelButton = false)
        {
            stopwatch = Stopwatch.StartNew();
            progressBarStackPanel =
                new Su.Revit.UI.StatusBarEx.HighVersion.StatusBar.Controls.ProgressBarStackPanel(
                    hasCancelButton
                );
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D||RVT_15||RVT_15_D||RVT_14||RVT_14_D||RVT_13_D||RVT_13||RVT_12_D||RVT_12||RVT_11_D||RVT_11

#else
            //this.ForceToRefresh = StatusBarController.IsVisible;
#endif
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="currentOperation"></param>
        /// <param name="count"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public RevitProgressBar Run(
            string currentOperation,
            int count,
            Action<int> action,
            bool isRunningEnableRibbon
        )
        {
            return SetCurrentOperation(currentOperation).Run(count, action, isRunningEnableRibbon);
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="count"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public RevitProgressBar Run(int count, Action<int> action, bool isRunningEnableRibbon)
        {
            return Run(Enumerable.Range(0, count), action, isRunningEnableRibbon);
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentOperation"></param>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public RevitProgressBar Run<T>(
            string currentOperation,
            IEnumerable<T> collection,
            Action<T> action,
            bool isRunningEnableRibbon
        )
        {
            return SetCurrentOperation(currentOperation)
                .Run(collection, action, isRunningEnableRibbon);
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public RevitProgressBar Run<T>(
            IEnumerable<T> collection,
            Action<T> action,
            bool isRunningEnableRibbon
        )
        {
            progressBarStackPanel.Data.CurrentValue = 0;
            progressBarStackPanel.Data.MinimumValue = 0;
            progressBarStackPanel.Data.MaximumValue = collection.Count();
            foreach (var item in collection)
            {
                var current = progressBarStackPanel.Data.CurrentValue;
                action?.Invoke(item);
                progressBarStackPanel.Data.CurrentValue = current + 1;
                //ApplicationUtils.DoEvents();
                var progressBar = RefreshStopwatchBackground(isRunningEnableRibbon);
                if (progressBar.IsCancelling())
                    break;
            }
            return this;
        }

        /// <summary>
        /// SetCurrentOperation
        /// </summary>
        /// <param name="currentOperation"></param>
        /// <returns></returns>
        public RevitProgressBar SetCurrentOperation(string currentOperation)
        {
            progressBarStackPanel.Data.CurrentOperation = currentOperation;
            return this;
        }

        /// <summary>
        /// SetCurrentValue
        /// </summary>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public RevitProgressBar SetCurrentValue(double currentValue)
        {
            progressBarStackPanel.Data.CurrentValue = currentValue;
            return this;
        }

        /// <summary>
        /// SetMinimumValue
        /// </summary>
        /// <param name="minimumValue"></param>
        /// <returns></returns>
        public RevitProgressBar SetMinimumValue(double minimumValue)
        {
            progressBarStackPanel.Data.MinimumValue = minimumValue;
            return this;
        }

        /// <summary>
        /// SetMaximumValue
        /// </summary>
        /// <param name="maximumValue"></param>
        /// <returns></returns>
        public RevitProgressBar SetMaximumValue(double maximumValue)
        {
            progressBarStackPanel.Data.MaximumValue = maximumValue;
            return this;
        }

        /// <summary>
        /// SetIsIndeterminate
        /// </summary>
        /// <param name="isIndeterminate"></param>
        /// <returns></returns>
        public RevitProgressBar SetIsIndeterminate(bool isIndeterminate)
        {
            progressBarStackPanel.Data.IsIndeterminate = isIndeterminate;
            return this;
        }

        /// <summary>
        /// SetHasCancelButton
        /// </summary>
        /// <param name="hasCancelButton"></param>
        /// <returns></returns>
        public RevitProgressBar SetHasCancelButton(bool hasCancelButton)
        {
            progressBarStackPanel.Data.HasCancelButton = hasCancelButton;
            return this;
        }

        /// <summary>
        /// Increment
        /// </summary>
        /// <param name="incrementCurrentValue"></param>
        /// <returns></returns>
        public RevitProgressBar Increment(int incrementCurrentValue = 1)
        {
            progressBarStackPanel.Data.CurrentValue += incrementCurrentValue;
            RefreshStopwatchBackground();
            return this;
        }

        private bool cancelPressed { get; set; } = false;

        /// <summary>
        /// IsCancelling
        /// </summary>
        /// <returns></returns>
        public bool IsCancelling()
        {
            if (progressBarStackPanel.Data.CommandCancel is null)
            {
                progressBarStackPanel.Data.CommandCancel =
                    new Su.Revit.UI.StatusBarEx.HighVersion.StatusBar.Utils.RelayCommand(Cancel);
            }
            return cancelPressed;
        }

        /// <summary>
        /// Cancel
        /// </summary>
        public void Cancel()
        {
            cancelPressed = true;
        }

        private bool ForceToRefresh;

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Su.Revit.UI.StatusBarEx.HighVersion.StatusBar.Utils.StatusBarController.Hide();
            stopwatch.Stop();
            RefreshBackground(true);
        }

        /// <summary>
        /// Refresh Milliseconds (default: 50)
        /// </summary>
        public int RefreshMilliseconds { get; set; } = 50;

        /// <summary>
        /// Initialize Milliseconds (default: 250)
        /// </summary>
        public int InitializeMilliseconds { get; set; } = 250;

        private RevitProgressBar RefreshStopwatchBackground(bool isRunningEnableRibbon = false)
        {
            if (
                InitializeMilliseconds > 0
                && stopwatch.ElapsedMilliseconds < InitializeMilliseconds
            )
            {
                return this;
            }
            InitializeMilliseconds = 0;
            if (stopwatch.ElapsedMilliseconds > RefreshMilliseconds)
            {
                Su.Revit.UI.StatusBarEx.HighVersion.StatusBar.Utils.StatusBarController.Show(
                    progressBarStackPanel
                );
                RefreshBackground(isRunningEnableRibbon);
                stopwatch.Restart();
            }
            return this;
        }

        private void RefreshBackground(bool enable = false)
        {
            System.Windows.Forms.Application.DoEvents();
            if (enable)
            {
                Su.Revit.UI.StatusBarEx.HighVersion.StatusBar.Utils.RevitRibbonController.Enable();
            }
            else
            {
                Su.Revit.UI.StatusBarEx.HighVersion.StatusBar.Utils.RevitRibbonController.Disable();
            }
        }
    }
}
