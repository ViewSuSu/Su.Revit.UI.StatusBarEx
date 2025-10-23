using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Su.Revit.UI.StatusBarEx.HighVersion.StatusBar;
using Su.Revit.UI.StatusBarEx.LowVersion;
using Su.Revit.UI.StatusBarEx.LowVersion.Forms;

namespace Su.Revit.UI.StatusBarEx
{
    /// <summary>
    /// 进度条设置
    /// </summary>
    public class ProgressBarExOptions
    {
        /// <summary>
        /// 进度条标题
        /// </summary>
        public string Title { get; set; } = "title";

        /// <summary>
        /// 进度条运行中Ribbon是否可用
        /// </summary>
        public bool IsRunningEnableRibbon { get; set; } = false;

        /// <summary>
        /// 取消按钮文字
        /// </summary>
        public string CancelButtonText { get; set; } = "取消";
    }

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
        /// <param name="progressBarExOptions">进度条选项</param>
        public static void Run<T>(
            IEnumerable<T> elements,
            Action<T> loopAction,
            ProgressBarExOptions progressBarExOptions = null
        )
        {
            var options = progressBarExOptions ?? new ProgressBarExOptions();
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D || RVT_15 || RVT_15_D || RVT_14 || RVT_14_D || RVT_13_D || RVT_13 || RVT_12_D || RVT_12 || RVT_11_D || RVT_11
            ProgressBarInetnalUtils.Run(elements, loopAction, options);
#else
            using var revitProgressBar = new RevitProgressBar(false, options);
            revitProgressBar.Run(
                options.Title,
                elements,
                (element) =>
                {
                    loopAction?.Invoke(element);
                },
                options.IsRunningEnableRibbon
            );
#endif
        }

        /// <summary>
        /// 开启进度条
        /// </summary>
        /// <param name="count">迭代次数</param>
        /// <param name="loopAction">迭代的操作</param>
        /// <param name="progressBarExOptions">进度条选项</param>
        public static void Run(
            int count,
            Action<int> loopAction,
            ProgressBarExOptions progressBarExOptions = null
        )
        {
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D||RVT_15||RVT_15_D||RVT_14||RVT_14_D||RVT_13_D||RVT_13||RVT_12_D||RVT_12||RVT_11_D||RVT_11
            ProgressBarInetnalUtils.Run(
                Enumerable.Range(0, count),
                loopAction,
                progressBarExOptions ?? new ProgressBarExOptions()
            );
#else
            Run(
                Enumerable.Range(0, count),
                loopAction,
                progressBarExOptions ?? new ProgressBarExOptions()
            );
#endif
        }

        /// <summary>
        /// 开启可取消的进度条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction">事务</param>
        /// <param name="sources">需要被迭代的元素集合</param>
        /// <param name="loopAction">迭代的操作</param>
        /// <param name="progressBarExOptions">进度条选项</param>
        public static void RunCancelable<T>(
            Transaction transaction,
            IEnumerable<T> sources,
            Action<T> loopAction,
            ProgressBarExOptions progressBarExOptions = null
        )
        {
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D || RVT_15 || RVT_15_D || RVT_14 || RVT_14_D || RVT_13_D || RVT_13 || RVT_12_D || RVT_12 || RVT_11_D || RVT_11
            ProgressBarInetnalUtils.RunCancelable(
                transaction,
                sources,
                loopAction,
                progressBarExOptions ?? new ProgressBarExOptions()
            );
#else
            var options = progressBarExOptions ?? new ProgressBarExOptions();
            using var revitProgressBar = new RevitProgressBar(true, options);
            revitProgressBar.Run(
                options.Title,
                sources,
                loopAction.Invoke,
                options.IsRunningEnableRibbon
            );
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
        /// <param name="progressBarExOptions">进度条选项</param>
        public static void RunCancelable<T>(
            TransactionGroup transactionGroup,
            IEnumerable<T> sources,
            Action<T> loopAction,
            ProgressBarExOptions progressBarExOptions = null
        )
        {
            var options = progressBarExOptions ?? new ProgressBarExOptions();
#if RVT_18 || RVT_18_D || RVT_17 || RVT_17_D || RVT_16 || RVT_16_D||RVT_15||RVT_15_D||RVT_14||RVT_14_D||RVT_13_D||RVT_13||RVT_12_D||RVT_12||RVT_11_D||RVT_11
            ProgressBarInetnalUtils.RunCancelable(transactionGroup, sources, loopAction, options);
#else
            using var revitProgressBar = new RevitProgressBar(true, options);
            revitProgressBar.Run(
                options.Title,
                sources,
                loopAction.Invoke,
                options.IsRunningEnableRibbon
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
            ProgressBarExOptions progressBarExOptions
        )
        {
            IEnumerable<object> objectSources = sources.Cast<object>();
            using var panel = new ControlPanel(
                objectSources,
                o =>
                {
                    DPCreator.TopMost();
                    (
                        (Action<object>)(
                            (obj) =>
                            {
                                if (obj is T typedObj)
                                {
                                    loopAction(typedObj);
                                }
                            }
                        )
                    )?.Invoke(o);
                },
                DPCreator.Close,
                false,
                progressBarExOptions
            );
            DPCreator.Create(panel);
            DPCreator.Show();
            panel.StartProgress();
        }

        public static void RunCancelable<T>(
            Transaction transaction,
            IEnumerable<T> sources,
            Action<T> loopAction,
            ProgressBarExOptions progressBarExOptions
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
                true,
                progressBarExOptions
            );
            DPCreator.Create(panel);
            DPCreator.Show();
            panel.StartProgress();
        }

        public static void RunCancelable<T>(
            TransactionGroup transactionGroup,
            IEnumerable<T> sources,
            Action<T> loopAction,
            ProgressBarExOptions progressBarExOptions
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
                true,
                progressBarExOptions
            );
            DPCreator.Create(panel);
            DPCreator.Show();
            panel.StartProgress();
        }
    }
}
