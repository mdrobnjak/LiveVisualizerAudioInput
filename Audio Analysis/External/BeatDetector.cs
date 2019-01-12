﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AudioAnalysis
{
    public class BeatDetector
    {
        

        private int _evalLength = 0;
        private List<double> bassHis;

        public void InitDetector(int evaluateLength)
        {
            _evalLength = evaluateLength;
            bassHis = new List<double>(evaluateLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">Must be transformed data (DFT/FFT)</param>
        public bool Scan(double[] data, int from, int to, ref double outAccumBass, ref double outCurBass)
        {
            double newBass = 0;
            for (int i = from; i <= to; i++)
            {
                newBass += data[i];
            }
            double accumBass = 0;
            bool beatDetected = false;
            if (bassHis.Count < _evalLength)
                bassHis.Add(newBass);
            else
            {
                foreach (var item in bassHis)
                {
                    accumBass += item;
                }
                double aveBass = accumBass / bassHis.Count;
                if (newBass > aveBass * 1.3d)
                    beatDetected = true;
                bassHis.RemoveAt(0);
                bassHis.Add(newBass);
            }
            outAccumBass = accumBass;
            outCurBass = newBass;
            return beatDetected;
        }
    }
}
