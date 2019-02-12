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
        const float maxBrightness = 1.0f;
        float brightness = maxBrightness;

        const double maxCubeScale = 3.0;
        double cubeScale = maxCubeScale;

        const int numCells = 8;
        const float cellSize = 5;

        const double maxMoveSpeed = 1;
        double moveSpeed = maxMoveSpeed;

        FastNoise fastNoise;
        Vector3 gridSize;
        float increment;
        Vector3 offset, offsetSpeed;
        Vector3[,,] flowfieldDirection;

        DateTime lastDraw;
        float deltaTime;
        int numCubes = 30;
        List<Cube> cubes;

        public NoiseFlowField()
        {
            gridSize.X = gridSize.Y = gridSize.Z = numCells * cellSize;

            cubes = new List<Cube>(numCubes);

            for (int i = 0; i < numCubes; i++)
            {
                cubes.Add(new Cube(
                    Rand.NextDouble() * gridSize.X,
                    Rand.NextDouble() * gridSize.Y,
                    Rand.NextDouble() * gridSize.Z));
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
            foreach (Cube c in cubes)
            {
                ////check edges - x
                //if (c.transform.position.x > this.transform.position.x + (gridSize.X * cellSize))
                //{
                //    c.transform.position = new Vector3(this.transform.position.x, c.transform.position.y, c.transform.position.z);
                //}
                //if (c.transform.position.x < this.transform.position.x)
                //{
                //    c.transform.position = new Vector3(this.transform.position.x + (gridSize.x * cellSize), c.transform.position.y, c.transform.position.z);
                //}
                //// y
                //if (c.transform.position.y > this.transform.position.y + (gridSize.Y * cellSize))
                //{
                //    c.transform.position = new Vector3(c.transform.position.x, this.transform.position.y, c.transform.position.z);
                //}
                //if (c.transform.position.y < this.transform.position.y)
                //{
                //    c.transform.position = new Vector3(c.transform.position.x, this.transform.position.y + (gridSize.y * cellSize), c.transform.position.z);
                //}
                //// z
                //if (c.transform.position.z > this.transform.position.z + (gridSize.Z * cellSize))
                //{
                //    c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, this.transform.position.z);
                //}
                //if (c.transform.position.z < this.transform.position.z)
                //{
                //    c.transform.position = new Vector3(c.transform.position.x, c.transform.position.y, this.transform.position.z + (gridSize.z * cellSize));
                //}

                Vector3 particlePos = new Vector3(
                    (float)Math.Floor((c.position.x) / cellSize),
                    (float)Math.Floor((c.position.y) / cellSize),
                    (float)Math.Floor((c.position.z) / cellSize)
                    );

                //c.ApplyRotation(flowfieldDirection[particlePos.X, particlePos.Y, particlePos.Z], particleRotateSpeed);
                //c.moveSpeed = particleMoveSpeed;
                //p.transform.localScale = new Vector3(particleScale,particleScale,particleScale);
            }
        }


        public void PreDraw()
        {
            VisEnv.DoLighting(brightness);
            
            GL.Translate(-gridSize.X / 2, -gridSize.X / 2, -50.0); //Create desired perspective by drawing everything far away and centering the grid
        }



        public void Draw()
        {

            foreach (Cube c in cubes)
            {
                GL.PushMatrix(); //Save current matrix


                if (cubeScale > maxCubeScale / 2)
                {
                    c.position.x += Rand.NextDoubleNeg() / 4;
                }

                c.position.y -= moveSpeed; //Increment X Position
                if (c.position.y < 0) c.position.y = gridSize.Y; //Check X Limit

                GL.Translate(c.position.x, c.position.y, c.position.z); //Set origin to center of cube


                c.angle.y += Rand.NextDouble(); //Adjust Angle

                //Execute Rotation
                GL.Rotate(c.angle.x, 1.0, 0.0, 0.0);
                GL.Rotate(c.angle.y, 0.0, 1.0, 0.0);
                GL.Rotate(c.angle.z, 0.0, 0.0, 1.0);



                GL.Scale(cubeScale, cubeScale, cubeScale); //Set scale of cube

                c.Draw(); //Draw cube

                GL.PopMatrix(); //Restore previously saved matrix
            }

        }

        public void PostDraw()
        {
            lastDraw = DateTime.Now;
            if (moveSpeed > 0.15) moveSpeed /= 1.5;
            if (cubeScale > 0.1) cubeScale -= 0.05;
            if (brightness > 0.05f) brightness -= 0.05f/3;
        }

        public void Trigger1()
        {
            cubeScale = maxCubeScale;
            brightness = maxBrightness;
        }

        public void Trigger2()
        {
            moveSpeed = maxMoveSpeed;
        }
    }
}
