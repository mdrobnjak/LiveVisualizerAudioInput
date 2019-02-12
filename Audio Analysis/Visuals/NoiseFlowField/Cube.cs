﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    class Cube
    {
        double[] color;

        double val = 0.5;

        public Position position;
        public Angle angle = new Angle(0.0, 0.0, 0.0);
        
        public Cube(double xPosition, double yPosition, double zPosition)
        {
            position = new Position(xPosition, yPosition, zPosition);
            color = new double[] { Rand.NextDouble(), Rand.NextDouble(), Rand.NextDouble() };
        }
        
        public void Draw()
        {
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(color);

            //left
            GL.Normal3(-1.0,0.0,0.0);
            GL.Vertex3(-val, val, val);
            GL.Vertex3(-val, val, -val);
            GL.Vertex3(-val, -val, -val);
            GL.Vertex3(-val, -val, val);

            //right
            GL.Normal3(1.0, 0.0, 0.0);
            GL.Vertex3(val, val, val);
            GL.Vertex3(val, val, -val);
            GL.Vertex3(val, -val, -val);
            GL.Vertex3(val, -val, val);

            //bottom
            GL.Normal3(0.0, -1.0, 0.0);
            GL.Vertex3(val, -val, val);
            GL.Vertex3(val, -val, -val);
            GL.Vertex3(-val, -val, -val);
            GL.Vertex3(-val, -val, val);

            //top
            GL.Normal3(0.0, 1.0, 0.0);
            GL.Vertex3(val, val, val);
            GL.Vertex3(val, val, -val);
            GL.Vertex3(-val, val, -val);
            GL.Vertex3(-val, val, val);

            //back
            GL.Normal3(0.0, 0.0, -1.0);
            GL.Vertex3(val, val, -val);
            GL.Vertex3(val, -val, -val);
            GL.Vertex3(-val, -val, -val);
            GL.Vertex3(-val, val, -val);

            //front
            GL.Normal3(0.0, 0.0, 1.0);
            GL.Vertex3(val, val, val);
            GL.Vertex3(val, -val, val);
            GL.Vertex3(-val, -val, val);
            GL.Vertex3(-val, val, val);

            GL.End();
        }


    }
}