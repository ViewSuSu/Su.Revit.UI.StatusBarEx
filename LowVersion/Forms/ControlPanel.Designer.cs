
namespace Su.Revit.UI.StatusBarEx.LowVersion.Forms
{
    internal partial class ControlPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.text = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.DisplayValue = new System.Windows.Forms.Label();
            this.separatorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 20);
            this.button1.TabIndex = 5;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(62, 4);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(240, 16);
            this.progressBar1.TabIndex = 6;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // DisplayValue
            // 
            this.DisplayValue.Location = new System.Drawing.Point(306, 4);
            this.DisplayValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DisplayValue.Name = "DisplayValue";
            this.DisplayValue.Size = new System.Drawing.Size(30, 16);
            this.DisplayValue.TabIndex = 7;
            this.DisplayValue.Text = "0%";
            this.DisplayValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // separatorLabel
            // 
            this.separatorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.separatorLabel.Location = new System.Drawing.Point(340, 2);
            this.separatorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.separatorLabel.Name = "separatorLabel";
            this.separatorLabel.Size = new System.Drawing.Size(1, 20);
            this.separatorLabel.TabIndex = 8;
            // 
            // text
            // 
            this.text.Location = new System.Drawing.Point(345, 4);
            this.text.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(200, 16);
            this.text.TabIndex = 3;
            this.text.Text = "Loading";
            this.text.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.separatorLabel);
            this.Controls.Add(this.DisplayValue);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.text);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ControlPanel";
            this.Size = new System.Drawing.Size(550, 24);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label text;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label DisplayValue;
        private System.Windows.Forms.Label separatorLabel;
    }
}
