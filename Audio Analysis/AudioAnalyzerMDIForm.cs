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
        ChartForm frmChart;
        ArduinoForm frmArduino;
        AutoSettingsForm frmAutoSettings;

        Range[] Ranges;

        public AudioAnalyzerMDIForm()
        {
            InitializeComponent();

            using (System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess())
                p.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;

            Range.Init(ref this.Ranges);
            Range.Init(ref FileIO.Ranges);
            Range.Init(ref AutoSettingsForm.Ranges);

            AudioIn.InitSoundCapture();
            Spectrum.SyncBandsAndFreqs();
            lblPreset.Text = FileIO.InitPathAndGetPreset();
            Arduino.InitPort();

            LoadChildForms();

            InitControls();
        }

        private void frmAudioAnalyzerMDI_Load(object sender, EventArgs e)
        {
            LayoutMdi(DefaultLayout);
        }

        void InitControls()
        {
            for (int i = 0; i < Range.Count; i++)
            {
                cboRange.Items.Add("Range " + (i + 1));
                cboSubtractor.Items.Add(i+1);
                cboSubtractFrom.Items.Add(i+1);
            }
            cboSubtractFrom.SelectedIndex = 0;
            cboSubtractor.SelectedIndex = 1;
            cboRange.SelectedIndex = 0;
        }

        void LoadChildForms()
        {
            frmSpectrum = new SpectrumForm();
            frmSpectrum.MdiParent = this;
            childFormNumber++;
            frmSpectrum.Show();

            frmChart = new ChartForm();
            frmChart.MdiParent = this;
            childFormNumber++;
            //frmChart.Show();
        }

        DateTime BeforeFFT;

        public void timerFFT_Tick(object sender, EventArgs e)
        {
            if (FFT.N_FFT != FFT.N_FFTBuffer)
            {
                FFT.N_FFT = FFT.N_FFTBuffer;
                FFT.transformedData = null;
            }

            BeforeFFT = DateTime.Now;
            FFT.transformedData = FFT.FFTWithProcessing(FFT.transformedData);

            for (int r = 0; r < Range.Count; r++)
            {
                Filter(r);

                ApplySubtraction(r);

                Ranges[r].AutoSettings.ApplyAutoSettings();


                lblDelay.Text = "Delays: Gate-" + (DateTime.Now - BeforeFFT).TotalMilliseconds + "ms";
                if (Gate(r))
                {
                    ArduinoForm.Trigger(r);
                    //SetProgressBars(r);
                }
                else
                {
                    //FadeProgressBars(r);
                }
            }

            Task.Run(() => frmSpectrum.Draw());
            Task.Run(() => frmChart.Draw());

            if (AutoSettings.Ranging)
            {
                AutoSettings.CollectFFTData(FFT.transformedData);
            }
            else if (AutoSettings.ReadyToProcess)
            {
                AutoSettings.ReadyToProcess = false;

                //Range1
                //PrintBandAnalysis(Ranges[0].AutoSettings.DoBandAnalysis());
                Ranges[0].AutoSettings.KickSelector();

                //Range2
                Ranges[1].AutoSettings.SnareSelector();

                //Range3
                Ranges[2].AutoSettings.HatSelector();

                //SelectedRange
                frmSpectrum.UpdateControls();

                AutoSettings.Reset();
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

                frmSpectrum.UpdateControls();
                frmChart.UpdateControls();

                lblPreset.Text = FileName.Split('\\').Last().Split('.')[0];
            }

        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;

                FileIO.WriteConfig(FileName);
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
            AudioIn.Dispose();
        }

        private void cboRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            Range.SetActive(cboRange.SelectedIndex);

            frmSpectrum.UpdateControls();
            frmChart.UpdateControls();
        }

        private void nFFTToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem dropDownItem in nFFTToolStripMenuItem.DropDownItems)
            {
                if (dropDownItem.Checked)
                {
                    dropDownItem.Checked = false;
                }
            }
            ((ToolStripMenuItem)(e.ClickedItem)).Checked = true;

            int intVar;

            if (!Int32.TryParse(((ToolStripMenuItem)(e.ClickedItem)).Text, out intVar)) return;
            FFT.N_FFTBuffer = intVar;
            Spectrum.SyncBandsAndFreqs();
            frmSpectrum.UpdateControls();
        }

        private void arduinoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmArduino = new ArduinoForm();
            frmArduino.MdiParent = this;
            childFormNumber++;
            frmArduino.Show();
        }

        private void autoSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAutoSettings = new AutoSettingsForm();
            frmAutoSettings.MdiParent = this;
            childFormNumber++;
            frmAutoSettings.Show();
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
            subtractFrom = Convert.ToInt32(cboSubtractFrom.Text) - 1;
            subtractor = Convert.ToInt32(cboSubtractor.Text) - 1;
            Subtract = btnSubtract.Checked;
        }

        private void msThreshold_Click(object sender, EventArgs e)
        {
            frmChart.AutoThreshold();
        }

        private void btnRawFFT_CheckStateChanged(object sender, EventArgs e)
        {
            FFT.rawFFT = btnRawFFT.Checked;
            Spectrum.SyncBandsAndFreqs();
            frmSpectrum.UpdateControls();
        }
    }
}