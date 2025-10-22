using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace Su.Revit.UI.StatusBarEx.LowVersion.Forms
{
    internal delegate void ButtonClickHandler(double parameter);

    internal partial class ControlPanel : UserControl, IDisposable
    {
        private readonly Transaction transaction;
        private readonly TransactionGroup transactionGroup;
        private readonly IEnumerable<object> objects;
        private readonly Action<object> action;
        private readonly Action stopAction;

        // 添加HasCancelButton字段
        private bool _hasCancelButton = false;

        private readonly bool IsRunningEnableRibbon;

        public ControlPanel(
            IEnumerable<Object> objects,
            Action<Object> action,
            Action stopAction,
            string title,
            bool hasCancelButton = true,
            bool IsRunningEnableRibbon = false
        )
        {
            _hasCancelButton = hasCancelButton;
            this.IsRunningEnableRibbon = IsRunningEnableRibbon;
            InitializeComponent();
            InitializeProgressBar();
            this.text.Text = title;
            this.objects = objects;
            this.action = action;
            this.stopAction = stopAction;

            // 根据参数设置按钮可见性
            UpdateButtonVisibility();
            this.Disposed += ControlPanel_Disposed;
        }

        private void ControlPanel_Disposed(object sender, EventArgs e)
        {
            UIFramework.RevitRibbonControl.RibbonControl.IsEnabled = true;
        }

        public ControlPanel(
            Transaction transaction,
            IEnumerable<object> objects,
            Action<object> action,
            Action stopAction,
            string title,
            bool hasCancelButton = true
        )
            : this(objects, action, stopAction, title, hasCancelButton)
        {
            this.transaction = transaction;
        }

        public ControlPanel(
            TransactionGroup transactionGroup,
            IEnumerable<object> objects,
            Action<object> action,
            Action stopAction,
            string title,
            bool hasCancelButton = true
        )
            : this(objects, action, stopAction, title, hasCancelButton)
        {
            this.transactionGroup = transactionGroup;
        }

        // 更新按钮可见性和布局的方法
        private void UpdateButtonVisibility()
        {
            button1.Visible = _hasCancelButton;

            // 根据按钮可见性调整进度条位置
            if (_hasCancelButton)
            {
                progressBar1.Location = new System.Drawing.Point(62, 4);
                progressBar1.Size = new System.Drawing.Size(240, 16);
            }
            else
            {
                progressBar1.Location = new System.Drawing.Point(4, 4);
                progressBar1.Size = new System.Drawing.Size(298, 16);
            }
        }

        // 添加公共属性以便外部修改
        public bool HasCancelButton
        {
            get => _hasCancelButton;
            set
            {
                if (_hasCancelButton != value)
                {
                    _hasCancelButton = value;
                    UpdateButtonVisibility();
                }
            }
        }

        // 其他代码保持不变...
        private void InitializeProgressBar()
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            progressBar1.Style = ProgressBarStyle.Continuous;
        }

        internal void StartProgress()
        {
            IsRunning = true;
            ProcessItemsSynchronously();
        }

        private bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                if (_isRunning)
                {
                    if (IsRunningEnableRibbon)
                        UIFramework.RevitRibbonControl.RibbonControl.IsEnabled = true;
                    else
                        UIFramework.RevitRibbonControl.RibbonControl.IsEnabled = false;
                }
                else
                {
                    UIFramework.RevitRibbonControl.RibbonControl.IsEnabled = true;
                }
            }
        }

        private void ProcessItemsSynchronously()
        {
            var totalItems = objects?.Count() ?? 0;

            if (totalItems == 0)
            {
                for (int i = 0; i <= 100; i++)
                {
                    if (!IsRunning)
                    {
                        break;
                    }
                    progressBar1.Value = i;
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(50);
                    this.DisplayValue.Text = $"{i}%";
                }
            }
            else
            {
                int current = 0;
                foreach (var obj in objects)
                {
                    if (!IsRunning)
                    {
                        break;
                    }
                    // 执行实际操作
                    action?.Invoke(obj);
                    current++;
                    int progress = (int)((double)current / totalItems * 100);
                    progressBar1.Value = progress;
                    this.DisplayValue.Text = $"{progress}%";
                    Application.DoEvents();
                }
            }

            IsRunning = false;
            progressBar1.Value = 100;
            stopAction?.Invoke();
        }

        public event ButtonClickHandler ButtonClick;

        private void button1_Click(object sender, EventArgs e)
        {
            IsRunning = false;
            if (transaction != null)
            {
                if (transaction.GetStatus() == TransactionStatus.Started)
                    transaction.RollBack();
            }
            else if (transactionGroup != null)
            {
                if (transaction.GetStatus() == TransactionStatus.Started)
                    transactionGroup.RollBack();
            }
            stopAction?.Invoke();
        }
    }
}
