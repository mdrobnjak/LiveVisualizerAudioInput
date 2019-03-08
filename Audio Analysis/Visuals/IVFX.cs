﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public interface IVFX
    {
        void PreDraw();

        void Draw();

        void PostDraw();

        void Trigger1();

        void Trigger2();

        void Trigger3(float amplitude);

        void Trigger4(int index, float amplitude = 0.0f);
    }
}
