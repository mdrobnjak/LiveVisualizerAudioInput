﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioAnalyzer
{
    public partial class AudioAnalyzerMDIForm : Form
    {
        private int childFormNumber = 0;
        MdiLayout DefaultLayout = MdiLayout.TileHorizontal;

        SpectrumForm frmSpectrum;
        ChartForm[] frmChart = new ChartForm[Range.Count];
        GateForm frmGate;
        ArduinoForm frmArduino;
        SettingsForm frmSettings;

        public AudioAnalyzerMDIForm()
        {
            InitializeComponent();

            using (System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess())
                p.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;

            Range.Init();
            Brushes.Init();
            ML.InitPaths();

            //Spectrum.SyncBandsAndFreqs();
            
            SoundCapture.Init();
            lblPreset.Text = FileIO.InitPathAndGetPreset();
            Arduino.InitPort();

            LoadChildForms();

            InitControls();

            this.SizeChanged += new System.EventHandler(this.frmAudioAnalyzerMDI_SizeChanged);
        }

        private void frmAudioAnalyzerMDI_SizeChanged(object sender, EventArgs e)
        {
            CustomMDILayout();
        }

        void InitControls()
        {
            for (int i = 0; i < Range.Count; i++)
            {
                cboRange.Items.Add("Range " + (i + 1));
                cboSubtractor.Items.Add(i + 1);
                cboSubtractFrom.Items.Add(i + 1);
            }
            cboSubtractFrom.SelectedIndex = 0;
            cboSubtractor.SelectedIndex = 1;
            MakeActive(0);

            lblStatus.Text = "";

            //btnDynamicThreshold.Checked = true;
            //btnDynamicThreshold_CheckedChanged(null, null);
        }

        void LoadChildForms()
        {
            for (int i = 0; i < Range.Count; i++)
            {
                frmChart[i] = new ChartForm(i);
                frmChart[i].MdiParent = this;
                childFormNumber++;
                frmChart[i].Show();
            }

            frmSpectrum = new SpectrumForm();
            frmSpectrum.MdiParent = this;
            childFormNumber++;
            frmSpectrum.Show();

            frmGate = new GateForm();
            frmGate.MdiParent = this;
            childFormNumber++;
            frmGate.Show();
        }

        DateTime BeforeFFT;

        public void timerFFT_Tick(object sender, EventArgs e)
        {
            //BeforeFFT = DateTime.Now;

            FFT.transformedData = FFT.LogScale(SoundCapture.Update());

            for (int r = 0; r < Range.Count; r++)
            {
                if (r == 2) Gate.FilterForMax(r);
                else Gate.FilterRejectMax(r);

                Gate.ApplySubtraction(r);

                Range.Ranges[r].AutoSettings.ApplyAutoSettings();

                //lblDelay.Text = "Delays: Gate-" + (DateTime.Now - BeforeFFT).TotalMilliseconds + "ms";

                if (Gate.Pass(r))
                {
                    if (r == 0) Visuals.Preset.Trigger1();
                    else if (r == 1) Visuals.Preset.Trigger2();
                    else if (r == 2) Visuals.Preset.Trigger3(Range.Ranges[r].Audio);

                    Arduino.Trigger(r);

                    frmGate.Pass[r] = true;
                }
                else
                {
                    frmGate.Pass[r] = false;
                }
            }

            Task.Run(() => frmGate.Draw());

            foreach (ChartForm chart in frmChart) Task.Run(() => chart.Draw());

            Task.Run(() => frmSpectrum.Draw());

            if (AutoSettings.Ranging)
            {
                AutoSettings.CollectFFTData(FFT.transformedData);
                progressBar.PerformStep();
                lblStatus.Text = "Auto Ranging...";
            }
            else if (AutoSettings.ReadyToProcess)
            {
                AutoSettings.ReadyToProcess = false;

                //Range1
                //PrintBandAnalysis(Ranges[0].AutoSettings.DoBandAnalysis());
                Range.Ranges[0].AutoSettings.KickSelector();

                //Range2
                Range.Ranges[1].AutoSettings.SnareSelector();

                //Range3
                Range.Ranges[2].AutoSettings.HatSelector();

                AutoSettings.Reset();

                progressBar.Value = 0;
                lblStatus.Text = "";
            }
        }


        #region Visual Studio Generated Code

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = FileIO.Path;

            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;

                FileIO.ReadConfig(FileName);

                lblPreset.Text = FileName.Split('\\').Last().Split('.')[0];
            }

        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                string FileName = lblPreset.Text;

                FileIO.WriteConfig(FileIO.Path + FileName + ".txt");

                MessageBox.Show("'" + lblPreset.Text + "' saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while saving: " + ex.ToString());
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = FileIO.Path;

            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;

                FileIO.WriteConfig(FileName);

                lblPreset.Text = FileName.Split('\\').Last().Split('.')[0];
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMain.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        #endregion

        private void frmAudioAnalyzerMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            SoundCapture.OnApplicationQuit();
            if (Arduino.Port.IsOpen) Arduino.Port.Close();
        }

        private void cboRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeActive(cboRange.SelectedIndex);
        }

        public void MakeActive(int i)
        {
            cboRange.SelectedIndex = i;

            Range.MakeActive(i);
            frmSpectrum.InitRectanglesAndBackground();

            cboRange.BackColor = Range.Active.Color;
            Brushes.InitGateBrushes(i);
        }

        private void arduinoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmArduino = new ArduinoForm();
            frmArduino.MdiParent = this;
            childFormNumber++;
            frmArduino.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings = new SettingsForm();
            frmSettings.MdiParent = this;
            childFormNumber++;
            frmSettings.Show();
        }

        private void incrementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSpectrum.IncrementRange();
        }

        private void decrementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSpectrum.DecrementRange();
        }

        private void msAutoRange_Click(object sender, EventArgs e)
        {
            AutoSettings.BeginRanging();
        }

        private void btnSubtract_CheckStateChanged(object sender, EventArgs e)
        {
            Gate.subtractFrom = Convert.ToInt32(cboSubtractFrom.Text) - 1;
            Gate.subtractor = Convert.ToInt32(cboSubtractor.Text) - 1;
            Gate.Subtract = btnSubtract.Checked;
        }

        private void msThreshold_Click(object sender, EventArgs e)
        {
            frmChart[cboRange.SelectedIndex].AutoThreshold();
        }

        //This needs to call a frmChart Method.
        private void btnDynamicThreshold_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Range.Count; i++)
            {
                Range.Ranges[i].AutoSettings.DynamicThreshold = btnDynamicThreshold.Checked;
            }
        }

        private void btnAutoSetThreshold_Click(object sender, EventArgs e)
        {
            frmChart[cboRange.SelectedIndex].AutoThreshold();
        }

        private void btnAutoRange_Click(object sender, EventArgs e)
        {
            AutoSettings.BeginRanging();
        }

        private async void initializeMachineLearningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Initializing Predictor...";

            await Task.Run(() => ML.InitPredictor());

            lblStatus.Text = "";

            MessageBox.Show("Predictor Initialized.");
        }

        private void trainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BandAnalysis.CompleteAndSaveTraining();
        }

        public void SetTimerInterval(int intervalMs)
        {
            timerFFT.Interval = intervalMs;
        }

        public int GetTimerInterval()
        {
            return timerFFT.Interval;
        }

        void CustomMDILayout()
        {
            int offset = menuStrip.Height + toolStripMain.Height + toolStripProcessing.Height + statusStrip.Height + 4;
            int h = ClientSize.Height - offset;

            frmSpectrum.Height = h / 2;
            frmSpectrum.Width = ClientSize.Width - 4;
            frmSpectrum.Location = new Point(0, h - frmSpectrum.Height);

            frmGate.Height = h / 2;
            frmGate.Width = (ClientSize.Width - 4) / (Range.Count + 1);
            frmGate.Location = new Point(0, h - frmGate.Height - frmSpectrum.Height);

            for (int i = 0; i < Range.Count; i++)
            {
                frmChart[i].Height = h / 2;
                frmChart[i].Width = (ClientSize.Width - 4) / (Range.Count + 1);
                frmChart[i].Location = new Point((i + 1) * frmChart[i].Width, h - frmChart[i].Height - frmSpectrum.Height);
            }
        }

        private void performanceModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (btnPerformanceMode.Checked)
            {
                for (int i = 0; i < Range.Count; i++)
                {
                    frmChart[i].WindowState = FormWindowState.Minimized;
                }

                frmSpectrum.WindowState = FormWindowState.Minimized;

                frmGate.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frmGate.WindowState = FormWindowState.Normal;

                for (int i = 0; i < Range.Count; i++)
                {
                    frmChart[i].WindowState = FormWindowState.Normal;
                }

                frmSpectrum.WindowState = FormWindowState.Normal;

                CustomMDILayout();
            }
        }
    }
}
