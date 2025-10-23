using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Su.Revit.UI.StatusBarEx.HighVersion.StatusBar.Controls
{
    public partial class ProgressBarStackPanel
    {
        /// <summary>
        /// ProgressBarData
        /// </summary>
        public ProgressBarData Data { get; } = new ProgressBarData();

        /// <summary>
        /// ProgressBarStackPanel
        /// </summary>
        /// <param name="hasCancelButton"></param>
        public ProgressBarStackPanel(
            bool hasCancelButton,
            ProgressBarExOptions progressBarExOptions
        )
        {
            Data.HasCancelButton = hasCancelButton;
            Data.CancelButtonText = progressBarExOptions.CancelButtonText;
            Data.CurrentOperation = progressBarExOptions.Title;
            Initialize();
        }

        private void Initialize()
        {
            DataContext = Data;
            InitializeComponent();
            //  this.AddResourceThemes();
        }

        public class ProgressBarData : INotifyPropertyChanged
        {
            private bool _isIndeterminate = false;
            private string _currentOperation = "Loading";
            private double _currentValue = 0;
            private double _minimumValue = 0;
            private double _maximumValue = 100;
            private ICommand _commandCancel;
            private bool _hasCancelButton = false;
            private string cancelButtonText = "取消";

            /// <summary>
            /// IsIndeterminate
            /// </summary>
            public bool IsIndeterminate
            {
                get => _isIndeterminate;
                set
                {
                    if (_isIndeterminate != value)
                    {
                        _isIndeterminate = value;
                        OnPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// CurrentOperation
            /// </summary>
            public string CurrentOperation
            {
                get => _currentOperation;
                set
                {
                    if (_currentOperation != value)
                    {
                        _currentOperation = value;
                        OnPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// 取消
            /// </summary>
            public string CancelButtonText
            {
                get { return cancelButtonText; }
                set
                {
                    cancelButtonText = value;
                    OnPropertyChanged();
                }
            }

            /// <summary>
            /// CurrentValue
            /// </summary>
            public double CurrentValue
            {
                get => _currentValue;
                set
                {
                    if (_currentValue != value)
                    {
                        _currentValue = value;
                        OnPropertyChanged();
                        OnPropertyChanged(nameof(DisplayValue));
                    }
                }
            }

            /// <summary>
            /// MinimumValue
            /// </summary>
            public double MinimumValue
            {
                get => _minimumValue;
                set
                {
                    if (_minimumValue != value)
                    {
                        _minimumValue = value;
                        OnPropertyChanged();
                        OnPropertyChanged(nameof(DisplayValue));
                    }
                }
            }

            /// <summary>
            /// MaximumValue
            /// </summary>
            public double MaximumValue
            {
                get => _maximumValue;
                set
                {
                    if (_maximumValue != value)
                    {
                        _maximumValue = value;
                        OnPropertyChanged();
                        OnPropertyChanged(nameof(DisplayValue));
                    }
                }
            }

            /// <summary>
            /// DisplayValue (read-only computed property)
            /// </summary>
            public double DisplayValue =>
                ((int)MaximumValue == (int)MinimumValue)
                    ? 100
                    : 100.0 * (CurrentValue - MinimumValue) / (MaximumValue - MinimumValue);

            /// <summary>
            /// CommandCancel
            /// </summary>
            public ICommand CommandCancel
            {
                get => _commandCancel;
                set
                {
                    if (_commandCancel != value)
                    {
                        _commandCancel = value;
                        OnPropertyChanged();
                    }
                }
            }

            /// <summary>
            /// HasCancelButton
            /// </summary>
            public bool HasCancelButton
            {
                get => _hasCancelButton;
                set
                {
                    if (_hasCancelButton != value)
                    {
                        _hasCancelButton = value;
                        OnPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
