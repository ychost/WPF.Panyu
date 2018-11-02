namespace HuaWeiCtls
{
    partial class FoxitPdfReader
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FoxitPdfReader));
            this.axFoxitCtl1 = new AxFOXITREADERLib.AxFoxitCtl();
            ((System.ComponentModel.ISupportInitialize)(this.axFoxitCtl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axFoxitCtl1
            // 
            this.axFoxitCtl1.Enabled = true;
            this.axFoxitCtl1.Location = new System.Drawing.Point(28, 18);
            this.axFoxitCtl1.Name = "axFoxitCtl1";
            this.axFoxitCtl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axFoxitCtl1.OcxState")));
            this.axFoxitCtl1.Size = new System.Drawing.Size(997, 777);
            this.axFoxitCtl1.TabIndex = 0;
            // 
            // FoxitPdfReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axFoxitCtl1);
            this.Name = "FoxitPdfReader";
            this.Size = new System.Drawing.Size(1042, 854);
            this.Load += new System.EventHandler(this.FoxitPdfReader_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axFoxitCtl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AxFOXITREADERLib.AxFoxitCtl axFoxitCtl1;
    }
}
