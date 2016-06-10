namespace ExcelAnalysisTools.WfHosts
{
    partial class HostToolsPane
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Host = new System.Windows.Forms.Integration.ElementHost();
            this.SuspendLayout();
            // 
            // Host
            // 
            this.Host.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Host.Location = new System.Drawing.Point(0, 0);
            this.Host.Name = "Host";
            this.Host.Size = new System.Drawing.Size(410, 336);
            this.Host.TabIndex = 0;
            this.Host.Text = "elementHost1";
            this.Host.Child = null;
            // 
            // HostToolsPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Host);
            this.Name = "HostToolsPane";
            this.Size = new System.Drawing.Size(410, 336);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Integration.ElementHost Host;
    }
}
