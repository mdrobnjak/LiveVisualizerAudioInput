﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveVisualizerAudioInput
{
    public class Range
    {
        public Range()
        {
        }

        public static void Init()
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                MakeActive(i);
                Active.AutoSettings.Range = Active;
            }
        }

        public static void MakeActive(int i)
        {
            ActiveIndex = i;
        }

        public static void SetCenterActive(int iCenter)
        {
            Active.LowCutAbsolute = Math.Max(iCenter - Active.AutoSettings.Bandwidth / 2, 0);
            Active.HighCutAbsolute = Active.LowCutAbsolute + Active.AutoSettings.Bandwidth;
        }

        public const int Count = 3;

        int Index;

        public static int ActiveIndex;

        public static Range[] Ranges = new Range[Count]
        {
            new Range()
            {
                Index = 0,

                LowFreq = 0, HighFreq = 150,

                AutoSettings = new AutoSettings()
                {
                    Bandwidth = 1,
                    ThresholdMultiplier = 0.6F
                },

                Color = Color.Pink,
                DarkColor = Color.Red
            },
            new Range()
            {
                Index = 1,

                LowFreq = 1000, HighFreq = 14000,

                AutoSettings = new AutoSettings()
                {
                    Bandwidth = 30,
                    ThresholdMultiplier = 0.6F
                },

                Color = Color.LightBlue,
                DarkColor = Color.BlueViolet
            },
            new Range()
            {
                Index = 2,

                LowFreq = 150, HighFreq = 1000,

                AutoSettings = new AutoSettings()
                {
                    Bandwidth = 50,
                    ThresholdMultiplier = 0.6F
                },

                Color = Color.LightGoldenrodYellow,
                DarkColor = Color.Gold
            },
            //new Range()
            //{
            //    Index = 3,

            //    LowFreq = 0, HighFreq = 150,

            //    AutoSettings = new AutoSettings()
            //    {
            //        Bandwidth = 1,
            //        ThresholdMultiplier = 0.6F
            //    },

            //    Color = Color.LightGreen,
            //    DarkColor = Color.Lime
            //}
        };

        public static ref Range Active
        {
            get { return ref Ranges[ActiveIndex]; }
        }

        public int Threshold { get; set; }

        private int lowCutAbsolute;
        public int LowCutAbsolute
        {
            get
            {
                return lowCutAbsolute;
            }
            set
            {
                if (value < 0) return;
                lowCutAbsolute = value;
                Spectrum.FreqOfBand.TryGetValue(value, out lowCutFreq);
            }
        }
        private int lowCutFreq;
        public int LowCutFreq
        {
            get
            {
                return lowCutFreq;
            }
            set
            {
                lowCutFreq = value;
                LowCutAbsolute = Spectrum.GetBandForFreq(value);
            }
        }
        public int LowCutIndex
        {
            get
            {
                return Spectrum.Full ? lowCutAbsolute : Math.Max(0, lowCutAbsolute - NumBandsBefore);
            }
            set
            {
                LowCutAbsolute = Spectrum.Full ? value : value + NumBandsBefore;
            }
        }

        private int highCutAbsolute;
        public int HighCutAbsolute
        {
            get
            {
                return highCutAbsolute;
            }
            set
            {
                highCutAbsolute = value;
                Spectrum.FreqOfBand.TryGetValue(value, out highCutFreq);
            }
        }
        private int highCutFreq;
        public int HighCutFreq
        {
            get
            {
                return highCutFreq;
            }
            set
            {
                highCutFreq = value;
                HighCutAbsolute = Spectrum.GetBandForFreq(value);
            }
        }
        public int HighCutIndex
        {
            get
            {
                return Spectrum.Full ? highCutAbsolute : Math.Max(0, highCutAbsolute - NumBandsBefore);
            }
            set
            {
                HighCutAbsolute = Spectrum.Full ? value : value + NumBandsBefore;
            }
        }

        public int LowFreq;
        public int LowFreqIndex
        {
            get
            {
                return Spectrum.GetBandForFreq(LowFreq);
            }
        }

        public int HighFreq;
        public int HighFreqIndex
        {
            get
            {
                return Spectrum.GetBandForFreq(HighFreq);
            }
        }

        private List<float> audios = new List<float>(new float[] { 0 });
        private float audio;
        public float Audio
        {
            get
            {
                return audio;
            }
            set
            {
                audio = value;

                audios.Add(value);
                if (audios.Count > 199) audios.RemoveAt(0);
            }
        }

        public float GetMaxAudioFromLast200()
        {
            return audios.Max() + 0.1f;
        }

        public AutoSettings AutoSettings;

        public int NumBands
        {
            get
            {
                return Spectrum.GetNumBandsForFreqRange(LowFreq, HighFreq);
            }
        }

        public int NumBandsBefore
        {
            get
            {
                return Spectrum.GetNumBandsForFreqRange(0, LowFreq);
            }
        }

        public Color Color;
        public Color DarkColor;
    }
}
