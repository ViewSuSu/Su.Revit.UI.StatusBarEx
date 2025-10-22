using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Su.Revit.UI.StatusBarEx.LowVersion;
using Su.Revit.UI.StatusBarEx.LowVersion.Forms;

namespace Su.Revit.UI.StatusBarEx
{
    /// <summary>
    /// This class is used to create a Revit progress bar.
    /// </summary>
    public static class ProgressBarExUtils
    {
        /// <summary>
        /// 开启进度条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements">需要被迭代的元素集合</param>
        /// <param name="loopAction">迭代的操作</param>
        /// <param name="title">进度条标题</param>
        /// <param name="isRunningEnableRibbon">进度条运行中Ribbon是否可用</param>
        public static void Run<T>(
            IEnumerable<T> elements,
            Action<T> loopAction,
            string title = "title",
            bool isRunningEnableRibbon = false
        )
        {
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D
            ProgressBarInetnalUtils.Run(elements, loopAction, title, isRunningEnableRibbon);
#else
            using var revitProgressBar =
                new HYBIM.Revit.FrameworkBase.Utils.ProgressBar.HighVersion.StatusBar.RevitProgressBar();
            revitProgressBar.Run(
                title,
                elements,
                (element) =>
                {
                    //  Task.Delay(new Random().Next(50, maxValue: 200));
                    loopAction?.Invoke(element);
                },
                isRunningEnableRibbon
            );
#endif
        }

        /// <summary>
        /// 开启进度条
        /// </summary>
        /// <param name="count">迭代次数</param>
        /// <param name="loopAction">迭代的操作</param>
        /// <param name="title">进度条标题</param>
        /// <param name="isRunningEnableRibbon">进度条运行中Ribbon是否可用</param>
        public static void Run(
            int count,
            Action<int> loopAction,
            string title = "title",
            bool isRunningEnableRibbon = false
        )
        {
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D
            ProgressBarInetnalUtils.Run(
                Enumerable.Range(0, count),
                loopAction,
                title,
                isRunningEnableRibbon
            );
#else
            Run(Enumerable.Range(0, count), loopAction, title, isRunningEnableRibbon);
#endif
        }

        /// <summary>
        /// 开启可取消的进度条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">事务</param>
        /// <param name="sources">需要被迭代的元素集合</param>
        /// <param name="loopAction">迭代的操作</param>
        /// <param name="title">进度条标题</param>
        /// <param name="isRunningEnableRibbon">进度条运行中Ribbon是否可用</param>
        public static void RunCancelable<T>(
            Transaction transaction,
            IEnumerable<T> sources,
            Action<T> loopAction,
            string title = "title",
            bool isRunningEnableRibbon = false
        )
        {
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D
            ProgressBarInetnalUtils.RunCancelable(
                transaction,
                sources,
                loopAction,
                title,
                isRunningEnableRibbon
            );
#else
            using var revitProgressBar =
                new HYBIM.Revit.FrameworkBase.Utils.ProgressBar.HighVersion.StatusBar.RevitProgressBar(
                    true
                );
            revitProgressBar.Run(title, sources, loopAction.Invoke, isRunningEnableRibbon);
            if (
                revitProgressBar.IsCancelling()
                && transaction.GetStatus() == TransactionStatus.Started
            )
            {
                transaction.RollBack();
            }
#endif
        }

        /// <summary>
        ///  开启可取消的进度条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transactionGroup">事务组</param>
        /// <param name="sources">需要被迭代的元素集合</param>
        /// <param name="loopAction">迭代的操作</param>
        /// <param name="title">进度条标题</param>
        /// <param name="isRunningEnableRibbon">进度条运行中Ribbon是否可用</param>
        public static void RunCancelable<T>(
            TransactionGroup transactionGroup,
            IEnumerable<T> sources,
            Action<T> loopAction,
            string title = "title",
            bool isRunningEnableRibbon = false
        )
        {
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D
            ProgressBarInetnalUtils.RunCancelable(
                transactionGroup,
                sources,
                loopAction,
                title,
                isRunningEnableRibbon
            );
#else
            using var revitProgressBar =
                new HYBIM.Revit.FrameworkBase.Utils.ProgressBar.HighVersion.StatusBar.RevitProgressBar(
                    true
                );
            revitProgressBar.Run(
                title,
                sources,
                t =>
                {
                    //  Task.Delay(new Random().Next(50, maxValue: 200));
                    loopAction.Invoke(t);
                },
                isRunningEnableRibbon
            );
            if (revitProgressBar.IsCancelling())
            {
                transactionGroup.RollBack();
            }
#endif
        }
    }

    internal static class ProgressBarInetnalUtils
    {
        public static void Run<T>(
            IEnumerable<T> sources,
            Action<T> loopAction,
            string title = "title",
            bool isRunningEnableRibbon = false
        )
        {
            IEnumerable<object> objectSources = sources.Cast<object>();
            Action<object> action = (obj) =>
            {
                if (obj is T typedObj)
                {
                    loopAction(typedObj);
                }
            };
            using var panel = new ControlPanel(
                objectSources,
                o =>
                {
                    DPCreator.TopMost();
                    action?.Invoke(o);
                },
                DPCreator.Close,
                title,
                false
            );
            DPCreator.Create(panel);
            DPCreator.Show();
            panel.StartProgress();
        }

        public static void RunCancelable<T>(
            Transaction transaction,
            IEnumerable<T> sources,
            Action<T> loopAction,
            string title = "title",
            bool isRunningEnableRibbon = false
        )
        {
            IEnumerable<object> objectSources = sources.Cast<object>();
            Action<object> action = (obj) =>
            {
                if (obj is T typedObj)
                {
                    loopAction(typedObj);
                }
            };
            using var panel = new ControlPanel(
                transaction,
                objectSources,
                o =>
                {
                    DPCreator.TopMost();
                    action?.Invoke(o);
                },
                DPCreator.Close,
                title,
                true
            );
            DPCreator.Create(panel);
            DPCreator.Show();
            panel.StartProgress();
        }

        public static void RunCancelable<T>(
            TransactionGroup transactionGroup,
            IEnumerable<T> sources,
            Action<T> loopAction,
            string title = "title",
            bool isRunningEnableRibbon = false
        )
        {
            IEnumerable<object> objectSources = sources.Cast<object>();
            Action<object> action = (obj) =>
            {
                if (obj is T typedObj)
                {
                    loopAction(typedObj);
                }
            };
            using var panel = new ControlPanel(
                transactionGroup,
                objectSources,
                o =>
                {
                    DPCreator.TopMost();
                    action?.Invoke(o);
                },
                DPCreator.Close,
                title,
                true
            );
            DPCreator.Create(panel);
            DPCreator.Show();
            panel.StartProgress();
        }
    }
}
