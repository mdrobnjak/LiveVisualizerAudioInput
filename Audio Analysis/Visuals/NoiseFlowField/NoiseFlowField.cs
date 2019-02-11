﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace AudioAnalyzer
{
    class NoiseFlowField : IVFX
    {
        const int maxCubeLength = 2;
        double cubeLength = maxCubeLength;

        const int numCells = 8;
        const float cellSize = 5;

        const double maxMoveSpeed = 2;
        double moveSpeed = maxMoveSpeed;

        FastNoise fastNoise;
        Vector3 gridSize;
        float increment;
        Vector3 offset, offsetSpeed;
        Vector3[,,] flowfieldDirection;

        DateTime lastDraw;
        float deltaTime;
        int numCubes = 20;
        List<Cube> cubes;
        Random r = new Random();

        public NoiseFlowField()
        {
            gridSize.X = gridSize.Y = gridSize.Z = numCells * cellSize;

            cubes = new List<Cube>(numCubes);
            
            for(int i = 0; i < numCubes; i++)
            {
                cubes.Add(new Cube(cellSize,
                    r.NextDouble() * gridSize.X,
                    r.NextDouble() * gridSize.Y,
                    r.NextDouble() * gridSize.Z));
            }
        }

        void CalculateFlowFieldDirections()
        {
            float deltaTime = (float)((DateTime.Now - lastDraw).TotalMilliseconds * 1000);

            offset = new Vector3(offset.X + (offsetSpeed.X * deltaTime), offset.Y + (offsetSpeed.Y * deltaTime), offset.Z + (offsetSpeed.Z * deltaTime));
            
            float noise;
            float xOff = 0f;
            for (int x = 0; x < gridSize.X; x++)
            {
                float yOff = 0f;

                for (int y = 0; y < gridSize.Y; y++)
                {
                    float zOff = 0f;

                    for (int z = 0; z < gridSize.Z; z++)
                    {
                        noise = fastNoise.GetSimplex(xOff + offset.X, yOff + offset.Y, zOff + offset.Z) + 1;
                        Vector3 noiseDirection = new Vector3((float)Math.Cos(noise * Math.PI), (float)Math.Sin(noise * Math.PI), (float)Math.Cos(noise * Math.PI));
                        flowfieldDirection[x, y, z] = Vector3.Normalize(noiseDirection);
                        zOff += increment;
                    }

                    yOff += increment;
                }

                xOff += increment;
            }
        }

        void CubeBehavior()
        {
        }

        //void CubeBehavior()
        //{
        //    foreach (Cube c in cubes)
        //    {
        //        ////check edges - x
        //        //if (c.transform.position.x > this.transform.position.x + (gridSize.X * cellSize))
        //        //{
        //        //    c.transform.position = new Vector3(this.transform.position.x, c.transform.position.y, c.transform.position.z);
        //        //}
        //        //if (c.transform.position.x < this.transform.position.x)
        //        //{
        //        //    c.transform.position = new Vector3(this.transform.position.x + (gridSize.x * cellSize), c.transform.position.y, c.transform.position.z);
        //        //}
        //        //// y
        //        //if (c.transform.position.y > this.transform.position.y + (gridSize.Y * cellSize))
        //        //{
        //        //    c.transform.position = new Vector3(c.transform.position.x, this.transform.position.y, c.transform.position.z);
        //        //}
        //        //if (c.transform.position.y < this.transform.position.y)
        //        //{
        //        //    c.transform.position = new Vector3(c.transform.position.x, this.transform.position.y + (gridSize.y * cellSize), c.transform.position.z);
        //        //}
        //        //// z
        //        //if (c.transform.position.z > this.transform.position.z + (gridSize.Z * cellSize))
        //        //{
        //        //    c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, this.transform.position.z);
        //        //}
        //        //if (c.transform.position.z < this.transform.position.z)
        //        //{
        //        //    c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, this.transform.position.z + (gridSize.z * cellSize));
        //        //}

        //        //Vector3Int particlePos = new Vector3Int(
        //        //    Mathf.FloorToInt(Mathf.Clamp((c.transform.position.x - this.transform.position.x) / cellSize, 0, gridSize.x - 1)),
        //        //    Mathf.FloorToInt(Mathf.Clamp((c.transform.position.y - this.transform.position.y) / cellSize, 0, gridSize.y - 1)),
        //        //    Mathf.FloorToInt(Mathf.Clamp((c.transform.position.z - this.transform.position.z) / cellSize, 0, gridSize.z - 1))
        //        //    );

        //        c.ApplyRotation(flowfieldDirection[particlePos.x, particlePos.y, particlePos.z], particleRotateSpeed);
        //        c.moveSpeed = particleMoveSpeed;
        //        //p.transform.localScale = new Vector3(particleScale,particleScale,particleScale);
        //    }
        //}


        public void PreDraw()
        {
            GL.Translate(-gridSize.X/2, -gridSize.X / 2, -50.0);
        }

        public void Draw()
        {
            GL.Begin(PrimitiveType.Quads);

            foreach (Cube c in cubes)
            {
                //Position
                c.xOffset += moveSpeed;
                if (c.xOffset > gridSize.X) c.xOffset = 0;



                c.SetLength(cubeLength);
                c.SpecifyVertices();
            }

            GL.End();
        }

        public void PostDraw()
        {
            lastDraw = DateTime.Now;
            if(moveSpeed > 0.15)moveSpeed /= 1.5;
            if(cubeLength > 0.1)cubeLength -= 0.03;
        }

        public void Trigger1()
        {
            moveSpeed = maxMoveSpeed;
        }

        public void Trigger2()
        {
            cubeLength = maxCubeLength;
        }
    }
}

