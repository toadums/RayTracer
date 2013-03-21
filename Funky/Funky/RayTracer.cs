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
using System.Linq;

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


    partial class RayTracer
    {
        private const int SphereDist = 2000;
        private const float numInnerPixels = 1;
        private const int NumBounces = 2;
        public static Vector2 ImageSize = new Vector2(400);

        private Perlin perlinTexture;
        public WriteableBitmap WB;
        private TextBlock FPS;
        public Vector3 Eye;

        private List<GeometricObject> Shapes;
        private List<Light> Lights;

        public RayTracer(ref WriteableBitmap wb, ref TextBlock fps, int width, int height)
        {
            perlinTexture = new Perlin();
            WB = wb;
            FPS = fps;

            Eye = new Vector3(ImageSize.X/2.0f, ImageSize.Y* 0.8f, -10000);

            //Drawing Objects is done in the DrawGeometry.cs file
            DrawGeometry();
        }

        public async void Draw()
        {

            int pixelWidth = WB.PixelWidth;
            int pixelHeight = WB.PixelHeight;
            int i = 0;
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

                StorageFolder folder = ApplicationData.Current.LocalFolder;

                await WriteableBitmapSaveExtensions.SaveToFile(WB, folder, "img" + i++ + ".jpg");


                // Redraw the WriteableBitmap
                WB.Invalidate();
                FPS.Text = "FPS = " + Utility.CalculateFrameRate().ToString();

                foreach (Light l in Lights)
                {
                    //l.position.X -= 5;
                }

                //Shapes[1].position.X -= 1;
                //Shapes[1].position.Z += 2;
                //Shapes[2].position.X += 1;
                //Shapes[2].position.Z -= 2;

               // if (Shapes[2].position.X > ImageSize.X / 2.0f + ((Sphere)Shapes[0]).radius) break;

            }
        }

        private byte[] Trace(int width, int height)
        {
            // 4 bytes required for each pixel
            byte[] result = new byte[width * height * 4];
            int resultIndex = 0;

            Vector3 color = new Vector3(0, 0, 0);


          
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    color = new Vector3(0, 0, 0);
                    for (float innerPixelY = 1.0f / numInnerPixels; innerPixelY <= 1; innerPixelY += 1.0f / numInnerPixels)
                    {
                        for (float innerPixelX = 1.0f / numInnerPixels; innerPixelX <= 1; innerPixelX += 1.0f / numInnerPixels)
                        {
                            Vector3 dir = (new Vector3(x + (innerPixelX - (1.0f / numInnerPixels * 2.0f)), y + (innerPixelY - (1.0f / numInnerPixels * 2.0f)), 0)) - Eye;
                            dir.Normalize();
                            Ray ray = new Ray(Eye, dir);

                            color += AddRay(ray, 0, 1.0f);

                        }
                    }

                    color /= (numInnerPixels * numInnerPixels);

                    if (color.X < 0 || color.Y < 0 || color.Z < 0)
                    {
                        color = new Vector3(255, 255, 0);
                    }

                    result[resultIndex++] = Convert.ToByte(color.Z);    // Green value of pixel
                    result[resultIndex++] = Convert.ToByte(color.Y);    // Blue value of pixel
                    result[resultIndex++] = Convert.ToByte(color.X);    // Red value of pixel
                    result[resultIndex++] = Convert.ToByte(255);        // Alpha value of pixel
                
                }
            }

            return result;
        }

        private Vector3 AddRay(Ray ray, int depth, float coef)
        {
            Vector3 curColor = new Vector3(0, 0, 0);
            GeometricObject hitShape = null;
            double hitShapeDist = float.MaxValue;
            Vector3 vNormal;
            Vector3 hp;

            foreach (GeometricObject shape in Shapes)
            {
                double t = shape.intersection(ray);

                if (t > 0.0 && t < hitShapeDist)
                {
                    hitShape = shape;
                    hitShapeDist = t;
                }
            }

            if (hitShape == null){
                if (depth == 0) return new Vector3(-1, -1, -1);
                else return new Vector3(0, 0, 0);
            }
            else
            {
                hp = FindPointOnRay(ray, hitShapeDist);
                vNormal = hitShape.NormalAt(hp, Eye);
                vNormal.Normalize();
                if (hitShape.surface.type == textureType.bump)
                {

                    const double bumpLevel = 0.5;
                    double noiseX = perlinTexture.noise(0.1 * (double)hp.X, 0.1 * (double)hp.Y, 0.1 * (double)hp.Z);
                    double noiseY = perlinTexture.noise(0.1 * (double)hp.Y, 0.1 * (double)hp.Z, 0.1 * (double)hp.X);
                    double noiseZ = perlinTexture.noise(0.1 * (double)hp.Z, 0.1 * (double)hp.X, 0.1 * (double)hp.Y);

                    vNormal.X = (float)((1.0 - bumpLevel) * vNormal.X + bumpLevel * noiseX);
                    vNormal.Y = (float)((1.0 - bumpLevel) * vNormal.Y + bumpLevel * noiseY);
                    vNormal.Z = (float)((1.0 - bumpLevel) * vNormal.Z + bumpLevel * noiseZ);

                    double temp = Vector3.Dot(vNormal, vNormal);
                    if (temp != 0.0)
                    {
                        temp = 1.0 / Math.Sqrt(temp);
                        vNormal = (float)temp * vNormal;
                    }
                }


                foreach (Light light in Lights)
                {
                    Vector3 dir = light.position - hp;
                    float dist = dir.Length();
                    dir.Normalize();
                    Ray lightRay = new Ray(hp, dir);

                    if (isVisible(light, hp, lightRay))
                    {
                        float lambert = Vector3.Dot(lightRay.Direction, vNormal) * coef;
                        curColor += lambert * (light.color / 255.0f) * (hitShape.surface.color / 255.0f);
                        curColor *= 255.0f;
                    }
                }

            }
            if (depth >= NumBounces) return Clamp(curColor);
            else
            {
                Vector3 dir = ray.Direction - (2.0f * Vector3.Dot(ray.Direction, vNormal)) * vNormal;
                dir.Normalize();
                return Clamp(curColor + AddRay(new Ray(hp, dir), depth+1, coef * ((float)hitShape.surface.reflectiveness/100.0f)));
            }
        }

        private Vector3 calcLightRay(Ray ray, Light light)
        {
            GeometricObject hitShape = null;
            double closestShape = float.MaxValue;

            foreach (GeometricObject shape in Shapes)
            {
                double t = shape.intersection(ray);

                if (t > 0.0 && t < closestShape)
                {
                    hitShape = shape;
                    closestShape = t;
                }
            }
            if (hitShape != null)
            {
                return FindPointOnRay(ray, closestShape);
            }
            else
                return light.position;
        }

        private bool isVisible(Light L, Vector3 hitPoint, Ray ray)
        {
            Vector3 objectLight = L.position - hitPoint;
            double rayLength = objectLight.Length();
            objectLight.Normalize();

            foreach (GeometricObject shape in Shapes)
            {
                double t = shape.intersection(ray);
                if (t < rayLength && t != 0.0)
                {
                    // something is in the way.
                    return false;
                }

            }
            // there is nothing in the way.
            return true;
        }

        // Find the point along the ray vector where the hit occurs.
        private Vector3 FindPointOnRay(Ray ray, double t)
        {

            Vector3 intersect;

            intersect.X = (float)(ray.Start.X + t * ray.Direction.X);
            intersect.Y = (float)(ray.Start.Y + t * ray.Direction.Y);
            intersect.Z = (float)(ray.Start.Z + t * ray.Direction.Z);

            return intersect;

        }

        private float Clamp(float val, float min, float max)
        {
            if (val < min) return min;
            else if (val > max) return max;
            else return val;
        }

        private Vector3 Clamp(Vector3 v)
        {
            if (v.X > 255) v.X = 255;
            else if (v.X < 0) v.X = 0;

            if (v.Y > 255) v.Y = 255;
            else if (v.Y < 0) v.Y = 0;

            if (v.Z > 255) v.Z = 255;
            else if (v.Z < 0) v.Z = 0;

            return v;
        }
    }
}
