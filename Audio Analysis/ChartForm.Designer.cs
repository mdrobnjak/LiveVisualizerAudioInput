﻿namespace AudioAnalyzer
{
    partial class ChartForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartAmplitude = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txtThreshold = new System.Windows.Forms.TextBox();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.trkbrThreshold = new System.Windows.Forms.TrackBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.msChartSettings = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chartAmplitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrThreshold)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartAmplitude
            // 
            this.chartAmplitude.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.chartAmplitude.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartAmplitude.Legends.Add(legend1);
            this.chartAmplitude.Location = new System.Drawing.Point(51, 1);
            this.chartAmplitude.Name = "chartAmplitude";
            series1.ChartArea = "ChartArea1";
            series1.Color = System.Drawing.Color.Black;
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartAmplitude.Series.Add(series1);
            this.chartAmplitude.Size = new System.Drawing.Size(830, 443);
            this.chartAmplitude.TabIndex = 17;
            // 
            // txtThreshold
            // 
            this.txtThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtThreshold.BackColor = System.Drawing.Color.Black;
            this.txtThreshold.ForeColor = System.Drawing.Color.White;
            this.txtThreshold.Location = new System.Drawing.Point(3, 407);
            this.txtThreshold.Name = "txtThreshold";
            this.txtThreshold.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtThreshold.Size = new System.Drawing.Size(42, 20);
            this.txtThreshold.TabIndex = 28;
            this.txtThreshold.Text = "0";
            // 
            // lblThreshold
            // 
            this.lblThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(-3, 430);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(54, 13);
            this.lblThreshold.TabIndex = 30;
            this.lblThreshold.Text = "Threshold";
            // 
            // trkbrThreshold
            // 
            this.trkbrThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.trkbrThreshold.BackColor = System.Drawing.SystemColors.Control;
            this.trkbrThreshold.Location = new System.Drawing.Point(0, -13);
            this.trkbrThreshold.Maximum = 100;
            this.trkbrThreshold.Name = "trkbrThreshold";
            this.trkbrThreshold.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trkbrThreshold.Size = new System.Drawing.Size(45, 414);
            this.trkbrThreshold.TabIndex = 29;
            this.trkbrThreshold.TickFrequency = 10;
            this.trkbrThreshold.ValueChanged += new System.EventHandler(this.trkbrThreshold_ValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msChartSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(881, 24);
            this.menuStrip1.TabIndex = 31;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // msChartSettings
            // 
            this.msChartSettings.Name = "msChartSettings";
            this.msChartSettings.Size = new System.Drawing.Size(48, 20);
            this.msChartSettings.Text = "Chart";
            // 
            // frmChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 444);
            this.Controls.Add(this.chartAmplitude);
            this.Controls.Add(this.txtThreshold);
            this.Controls.Add(this.trkbrThreshold);
            this.Controls.Add(this.lblThreshold);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmChart";
            this.Text = "Chart";
            ((System.ComponentModel.ISupportInitialize)(this.chartAmplitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkbrThreshold)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartAmplitude;
        private System.Windows.Forms.TextBox txtThreshold;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.TrackBar trkbrThreshold;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem msChartSettings;
    }
}