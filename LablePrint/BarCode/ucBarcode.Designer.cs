﻿using LablePrint.Control;

namespace LablePrint.BarCode
{
    partial class ucBarcode
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
            this.documentSpace1 = new DocumentSpace();
            this.SuspendLayout();
            // 
            // documentSpace1
            // 
            this.documentSpace1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentSpace1.Location = new System.Drawing.Point(0, 0);
            this.documentSpace1.Name = "documentSpace1";
            this.documentSpace1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.documentSpace1.Size = new System.Drawing.Size(787, 503);
            this.documentSpace1.TabIndex = 0;
            // 
            // ucBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.documentSpace1);
            this.Name = "ucBarcode";
            this.Size = new System.Drawing.Size(787, 503);
            this.ResumeLayout(false);

        }

        #endregion

        private DocumentSpace documentSpace1;








    }
}
