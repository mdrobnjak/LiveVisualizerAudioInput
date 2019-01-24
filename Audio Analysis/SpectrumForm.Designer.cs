﻿namespace AudioAnalyzer
{
    partial class SpectrumForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlSpectrum = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.msSpectrumSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.msActiveRangeOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.msScale = new System.Windows.Forms.ToolStripMenuItem();
            this.txtmsiScale = new System.Windows.Forms.ToolStripTextBox();
            this.trkbrMax = new System.Windows.Forms.TrackBar();
            this.trkbrMin = new System.Windows.Forms.TrackBar();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrMin)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSpectrum
            // 
            this.pnlSpectrum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSpectrum.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSpectrum.Location = new System.Drawing.Point(0, 0);
            this.pnlSpectrum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlSpectrum.Name = "pnlSpectrum";
            this.pnlSpectrum.Size = new System.Drawing.Size(1095, 366);
            this.pnlSpectrum.TabIndex = 19;
            this.pnlSpectrum.SizeChanged += new System.EventHandler(this.pnlSpectrum_SizeChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msSpectrumSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1095, 28);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // msSpectrumSettings
            // 
            this.msSpectrumSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msActiveRangeOnly,
            this.msScale});
            this.msSpectrumSettings.Name = "msSpectrumSettings";
            this.msSpectrumSettings.Size = new System.Drawing.Size(84, 24);
            this.msSpectrumSettings.Text = "Spectrum";
            // 
            // msActiveRangeOnly
            // 
            this.msActiveRangeOnly.CheckOnClick = true;
            this.msActiveRangeOnly.Name = "msActiveRangeOnly";
            this.msActiveRangeOnly.Size = new System.Drawing.Size(216, 26);
            this.msActiveRangeOnly.Text = "Active Range Only";
            this.msActiveRangeOnly.CheckStateChanged += new System.EventHandler(this.msActiveRangeOnly_CheckStateChanged);
            // 
            // msScale
            // 
            this.msScale.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtmsiScale});
            this.msScale.Name = "msScale";
            this.msScale.Size = new System.Drawing.Size(216, 26);
            this.msScale.Text = "Scale";
            // 
            // txtmsiScale
            // 
            this.txtmsiScale.Name = "txtmsiScale";
            this.txtmsiScale.Size = new System.Drawing.Size(100, 27);
            this.txtmsiScale.TextChanged += new System.EventHandler(this.txtmsScale_TextChanged);
            // 
            // trkbrMax
            // 
            this.trkbrMax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkbrMax.BackColor = System.Drawing.SystemColors.Control;
            this.trkbrMax.Location = new System.Drawing.Point(-17, 396);
            this.trkbrMax.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trkbrMax.Name = "trkbrMax";
            this.trkbrMax.Size = new System.Drawing.Size(1128, 56);
            this.trkbrMax.TabIndex = 24;
            this.trkbrMax.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trkbrMax.ValueChanged += new System.EventHandler(this.trkbrMax_ValueChanged);
            // 
            // trkbrMin
            // 
            this.trkbrMin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trkbrMin.BackColor = System.Drawing.SystemColors.Control;
            this.trkbrMin.Location = new System.Drawing.Point(-17, 361);
            this.trkbrMin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trkbrMin.Name = "trkbrMin";
            this.trkbrMin.Size = new System.Drawing.Size(1128, 56);
            this.trkbrMin.TabIndex = 23;
            this.trkbrMin.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trkbrMin.ValueChanged += new System.EventHandler(this.trkbrMin_ValueChanged);
            // 
            // SpectrumForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 431);
            this.Controls.Add(this.trkbrMax);
            this.Controls.Add(this.trkbrMin);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pnlSpectrum);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SpectrumForm";
            this.Text = "Spectrum";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrMin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlSpectrum;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem msSpectrumSettings;
        private System.Windows.Forms.ToolStripMenuItem msActiveRangeOnly;
        private System.Windows.Forms.TrackBar trkbrMax;
        private System.Windows.Forms.TrackBar trkbrMin;
        private System.Windows.Forms.ToolStripMenuItem msScale;
        private System.Windows.Forms.ToolStripTextBox txtmsiScale;
    }
}