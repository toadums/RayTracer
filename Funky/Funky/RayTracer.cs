﻿
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using SharpDX;
using System.Collections.Generic;

namespace Funky
{
    class Utility
    {
        #region Basic Frame Counter

        private static int lastTick;
        private static int lastFrameRate;
        private static int frameRate;

        public static int CalculateFrameRate()
        {
            if (System.Environment.TickCount - lastTick >= 1000)
            {
                lastFrameRate = frameRate;
                frameRate = 0;
                lastTick = System.Environment.TickCount;
            }
            frameRate++;
            return lastFrameRate;
        }
        #endregion

    }


    class RayTracer
    {

        public WriteableBitmap WB;
        private TextBlock FPS;
        private int REFLECTION_FACTOR = 1;
        public Vector3 Eye;
        
        private List<GeometricObject> Shapes;
        private List<Light> Lights;

        public RayTracer(ref WriteableBitmap wb, ref TextBlock fps)
        {
            WB = wb;
            FPS = fps;

            Eye = new Vector3(MainPage.ImageSize / 2.0f, -10000);

            Shapes = new List<GeometricObject>();
            Lights = new List<Light>(){new Light(){position = new Vector3(300, 200, 200)}};

            Shapes.Add(new Sphere(50,new Vector3(50,200,200), new Vector4(255,0,0,255), 
                new SurfaceType(new Vector3(200,100,100), new Vector3(100,100,100), new Vector3(50,50,50),50)));
            /*
            Shapes.Add(new Sphere(75, new Vector3(250, 200, 100), new Vector4(255, 255, 0, 255),
                new SurfaceType(new Vector3(), new Vector3(), new Vector3(),50)));
            */

        }


        public async void Draw()
        {

            int pixelWidth = WB.PixelWidth;
            int pixelHeight = WB.PixelHeight;

            while (true)
            {
                // Asynchronously graph the Mandelbrot set on a background thread
                byte[] result = null;
                await ThreadPool.RunAsync(new WorkItemHandler(
                    (IAsyncAction action) =>
                    {
                        result = Trace(pixelWidth, pixelHeight);
                    }
                    ));

                // Open a stream to copy the graph to the WriteableBitmap's pixel buffer
                using (Stream stream = WB.PixelBuffer.AsStream())
                {
                    await stream.WriteAsync(result, 0, result.Length);
                }

                // Redraw the WriteableBitmap
                WB.Invalidate();
                FPS.Text = "FPS = " + Utility.CalculateFrameRate().ToString();

            }
        }


        private byte[] Trace(int width, int height)
        {
            // 4 bytes required for each pixel
            byte[] result = new byte[width * height * 4];
            int resultIndex = 0;

            Vector3 color = new Vector3(0, 0, 0);
            // Plot the Mandelbrot set on x-y plane
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 dir = (new Vector3(x, y, 0)) - Eye;
                    dir.Normalize();
                    Ray ray = new Ray(Eye, dir);
                    color = AddRay(ray, 0);

                    result[resultIndex++] = Convert.ToByte(color.Z); // Green value of pixel
                    result[resultIndex++] = Convert.ToByte(color.Y); // Blue value of pixel
                    result[resultIndex++] = Convert.ToByte(color.X); // Red value of pixel
                    result[resultIndex++] = Convert.ToByte(255); // Alpha value of pixel
                     
                }
            }

            return result;
        }

        private Vector3 AddRay(Ray ray, int depth)
        {
            foreach (GeometricObject shape in Shapes)
            {
                double t = shape.intersection(ray);

                if (t > 0.0)
                {
                    foreach (Light light in Lights)
                    {
                        if (!isVisible(light, FindPointOnRay(ray, t)))
                        {
                            return new Vector3(0, 0, 255);
                        }
                        else
                        {
                            return new Vector3(255, 0, 0);
                        }
                    }
                }
            }
            return new Vector3(0, 255, 0);
        }

        private bool isVisible(Light L, Vector3 hitPoint)
        {
            Vector3 objectLight = L.position - hitPoint;
            double rayLength = objectLight.Length();
            objectLight.Normalize();

            foreach (GeometricObject shape in Shapes)
            {
                double t = shape.intersection(new Ray(hitPoint, objectLight));
                if ( t < rayLength && t != 0.0)
                {
                    // something is in the way.
                    return false;
                }

            }
            // there is nothing in the way.
            return true;
        }

        // Find the point along the ray vector where the hit occurs.
        private Vector3 FindPointOnRay(Ray ray, double t) {
            
            Vector3 intersect;
            
            intersect.X = (float)(ray.Start.X + t * ray.Direction.X);
            intersect.Y = (float)(ray.Start.Y + t * ray.Direction.Y);
            intersect.Z = (float)(ray.Start.Z + t * ray.Direction.Z);

            return intersect;

      }





    }
}
