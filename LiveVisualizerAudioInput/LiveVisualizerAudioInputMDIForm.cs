﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveVisualizerAudioInput
{
    public partial class LiveVisualizerAudioInputMDIForm : Form
    {
        private int childFormCount = 0;

        //Child Forms
        SpectrumForm frmSpectrum;
        ChartForm[] frmChart = new ChartForm[Range.Count];
        GateForm frmGate;
        ArduinoForm frmArduino;
        SettingsForm frmSettings;
        //OscilloscopeForm frmOscilloscope;

        public LiveVisualizerAudioInputMDIForm()
        {

            InitializeComponent();

            //Try to give this process priority. Not sure if it improves performance.
            using (System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess())
                p.PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;

            Range.Init();
            Brushes.Init();
            ML.InitPaths();

            SoundCapture.Init();
            lblPreset.Text = FileIO.InitPathAndGetPreset();
            Arduino.InitPort();

            LoadChildForms();

            InitControls();

            this.SizeChanged += new System.EventHandler(this.frmLiveVisualizerAudioInputMDI_SizeChanged);
            
            //Enable FFT timer after initializing all other components.
            timerFFT.Enabled = true;

            //Int32 port = 13000;
            //TcpClient client = new TcpClient(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString(), port);
            //stream = client.GetStream();
        }

        NetworkStream stream;

        private void SendToServer(string message)
        {
            // Translate the Message into ASCII.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);
            // Bytes Array to receive Server Response.
            data = new Byte[256];
            String response = String.Empty;
            // Read the Tcp Server Response Bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
        }

        private void frmLiveVisualizerAudioInputMDI_SizeChanged(object sender, EventArgs e)
        {
            CustomMDILayout();
        }

        void InitControls()
        {
            for (int i = 0; i < Range.Count; i++)
            {
                cboRange.Items.Add("Range " + (i + 1));
            }

            MakeActive(0);

            lblStatus.Text = "";
        }

        void LoadChildForms()
        {
            for (int i = 0; i < Range.Count; i++)
            {
                frmChart[i] = new ChartForm(i);
                frmChart[i].MdiParent = this;
                childFormCount++;
                frmChart[i].Show();
            }

            //frmOscilloscope = new OscilloscopeForm();
            //frmOscilloscope.MdiParent = this;
            //childFormNumber++;
            //frmOscilloscope.Show();

            frmSpectrum = new SpectrumForm();
            frmSpectrum.MdiParent = this;
            childFormCount++;
            frmSpectrum.Show();

            frmGate = new GateForm();
            frmGate.MdiParent = this;
            childFormCount++;
            frmGate.Show();
        }
        
        public void timerFFT_Tick(object sender, EventArgs e)
        {
            FFT.transformedData = FFT.LogScale(SoundCapture.GetFFTData());

            for (int r = 0; r < Range.Count; r++)
            {
                Gate.Filter(r);

                Range.Ranges[r].AutoSettings.ApplyAutoSettings();

                if (Gate.Pass(r))
                {
                    if (r == 0)
                    {
                        //SendToServer("k");
                    }
                    else if (r == 1)
                    {
                        //SendToServer("s");
                    }

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

            //Task.Run(() => frmOscilloscope.Draw());

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
            childForm.Text = "Window " + childFormCount++;
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

        private void frmLiveVisualizerAudioInputMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            SoundCapture.StopAndDisposeResources();
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
            childFormCount++;
            frmArduino.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings = new SettingsForm();
            frmSettings.MdiParent = this;
            childFormCount++;
            frmSettings.Show();
        }

        private void msAutoRange_Click(object sender, EventArgs e)
        {
            AutoSettings.BeginRanging();
        }
        
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
            frmGate.Width = (ClientSize.Width - 4) / (Range.Count + 2);
            frmGate.Location = new Point(0, h - frmGate.Height - frmSpectrum.Height);
            
            for (int i = 0; i < Range.Count; i++)
            {
                frmChart[i].Height = h / 2;
                frmChart[i].Width = (ClientSize.Width - 4) / (Range.Count + 2);
                frmChart[i].Location = new Point((i + 1) * frmChart[i].Width, h - frmChart[i].Height - frmSpectrum.Height);
            }

            //frmOscilloscope.Height = h / 2;
            //frmOscilloscope.Width = (ClientSize.Width - 4) / (Range.Count + 2);
            //frmOscilloscope.Location = new Point((Range.Count + 1) * frmOscilloscope.Width, h - frmOscilloscope.Height - frmSpectrum.Height);
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
