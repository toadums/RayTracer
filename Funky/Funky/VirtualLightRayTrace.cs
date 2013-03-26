﻿using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{
    partial class RayTracer
    {
        int NumVirtualLights = 1;

        public void spawnVPL(Light light,float width,float height)
        {
            Random r = new Random();

            List<Vector3> virtualLightPositions = new List<Vector3>();
            for (int i = 0; i < NumVirtualLights; i++)
            {
                virtualLightPositions.Add(new Vector3(r.Next(0, (int)width), r.Next(0, (int)height), 0));
            }

            foreach (Vector3 VPLPos in virtualLightPositions)
            {
                Vector3 dir = (new Vector3(VPLPos.X, VPLPos.Y, 0)) - Eye;
                dir.Normalize();
                Ray ray = new Ray(Eye, dir);
                Vector3 newLightPos = calcLightRay(ray, light);

                if (newLightPos.X != float.MaxValue && newLightPos.Y != float.MaxValue && newLightPos.Z != float.MaxValue)
                {

                    //GeometricObject testSphere = new Sphere(ImageSize.Y / 16.0f, newLightPos, new Vector4(0, 0, 0, 255), new SurfaceType(textureType.standard,new Vector3(200, 100, 100), new Vector3(100, 40, 78), new Vector3(50, 50, 50), new Vector3(255, 255, 255), 50));
                    //Shapes.Add(testSphere);

                    VirtualLights.Add(new Light()
                    {
                        position = newLightPos,
                        color = light.color
                    });
                }
            }
        }
    }
}