
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

        private const bool UseVPL = false;

        private const float numInnerPixels = 1;

        private const int NumBounces = 0;
        public static Vector2 ImageSize = new Vector2(800);
        private float SphereDist = 1000;

        private Perlin perlinTexture;
        public WriteableBitmap WB;
        private TextBlock FPS;
        public Vector3 Eye;

        private List<GeometricObject> Shapes;
        private List<Light> Lights;
        private List<Light> VirtualLights;

        public RayTracer(ref WriteableBitmap wb, ref TextBlock fps, int width, int height)
        {
            perlinTexture = new Perlin();
            WB = wb;
            FPS = fps;

            //Eye = new Vector3(278,273,-800);
            Eye = new Vector3(ImageSize.X / 2.0f, ImageSize.Y / 2.0f - 100, -4000);

            System.Diagnostics.Debug.WriteLine("Picture Path: " + ApplicationData.Current.LocalFolder.Path); 
        }

        public async void Draw()
        {

            await DrawGeometry();

            if (UseVPL)
            {
                VirtualLights = new List<Light>();
                spawnVPL(Lights[0], ImageSize.X, ImageSize.Y);

                Lights.AddRange(VirtualLights);

                //Lights.Remove(Lights[0]);
            }

            int pixelWidth = WB.PixelWidth;
            int pixelHeight = WB.PixelHeight;
            int i = 0;
            while (true)
            {
                DateTime start = DateTime.Now;
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
                /*
                foreach (Light l in Lights)
                {
                    l.position.X -= 5;
                }

                Lights[0].position.X += 5;
                Lights[0].position.Y += 5;
                Lights[0].position.Z += 50;

                ((Sphere)Shapes[2]).position.X += 5;
                ((Sphere)Shapes[2]).position.Z -= 50;
                
                if (((Sphere)Shapes[2]).position.X > ImageSize.X / 2.0f + ((Sphere)Shapes[0]).radius) break;
                */

                TimeSpan time = DateTime.Now - start;

                System.Diagnostics.Debug.WriteLine("Time to render = " + time);


            }
        }

        private byte[] Trace(int width, int height)
        {

            string s = "0 =                                                                                                    100";

            // 4 bytes required for each pixel
            byte[] result = new byte[width * height * 4];
            int resultIndex = 0;

            float totalNum = width * height;

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
                            float ThisVariableDoesAbsolutelyNothingInThisSpotButYouNeedItForTheRefVariable = 0;
                            color += AddRay(ray, 0, 1.0f, ref ThisVariableDoesAbsolutelyNothingInThisSpotButYouNeedItForTheRefVariable);

                        }
                    }



                    color /= (numInnerPixels * numInnerPixels);

                    color *= 255.0f;

                    if (color.X < 0 || color.Y < 0 || color.Z < 0)
                    {
                        color = new Vector3(1, 1, 0);
                    }

                    result[resultIndex++] = Convert.ToByte(color.Z);    // Green value of pixel
                    result[resultIndex++] = Convert.ToByte(color.Y);    // Blue value of pixel
                    result[resultIndex++] = Convert.ToByte(color.X);    // Red value of pixel
                    result[resultIndex++] = Convert.ToByte(255);        // Alpha value of pixel

                    float n = (((float)y) * height + x) / totalNum * 100;

                    if (n == (int)n)
                    {

                        s = s.Replace("= ", " =");

                        System.Diagnostics.Debug.WriteLine(s);
                    }


                }

            }

            /*Parallel.ForEach(tasks, task =>
                {
                    task.RunSynchronously();
                }
            );*/


            return result;
        }


        /// <summary>
        /// Traces rays and calculates their color. Recursive method
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="depth"></param>
        /// <param name="coef"></param>
        /// <param name="specOn">if at any point an object is not specular, all successive recursive calls will ignore specular</param>
        /// <returns></returns>
        private Vector3 AddRay(Ray ray, int depth, float coef, ref float prevDist, float lastRIndex = 1)
        {
            Vector3 curColor = new Vector3(0, 0, 0);
            GeometricObject hitShape = null;
            double hitShapeDist = float.MaxValue;
            Vector3 vNormal;
            Vector3 hp;
            float LightValue = 0.0f;

            foreach (GeometricObject shape in Shapes)
            {
                if (shape == null) continue;
                double t = shape.intersection(ray);

                if (t > 0.0 && t < hitShapeDist)
                {
                    hitShape = shape;
                    hitShapeDist = t;
                    prevDist = (float)t;
                }
            }

            if (hitShape == null)
            {
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

                    const double bumpLevel = 0.2;
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
                if (hitShape is Sphere)
                    if (((Sphere)hitShape).position.X > ImageSize.X / 2.0f)
                    {

                    }

                foreach (Light light in Lights)
                {
                    Vector3 dir = light.position - hp;
                    dir.Normalize();
                    Ray lightRay = new Ray(hp, dir);


                    if ((LightValue = isVisible(light, hp, lightRay)) > 0)
                    {

                        //TODO to add ambient just do Llight[ambient] * hitShape[ambiemt]
                        float lambert = Vector3.Dot(lightRay.Direction, vNormal) * coef;
                        curColor += light.intensity * lambert * ((light.color)) * (hitShape.surface.color);

                        Vector3 blinn = lightRay.Direction - ray.Direction;
                        blinn.Normalize();

                        float blinnValue = (float)Math.Pow(Math.Max(0, Vector3.Dot(vNormal, blinn)), hitShape.surface.SpecExponent);

                        curColor += hitShape.surface.specular * light.intensity * light.color * blinnValue * coef;

                        curColor *= LightValue;
                    }
                }

            }
            if (depth >= NumBounces) return Clamp(curColor);
            else
            {

                //calculate reflections
                Vector3 dir = ray.Direction - (2.0f * Vector3.Dot(ray.Direction, vNormal)) * vNormal;
                dir.Normalize();
                float UGH = 0;
                curColor += AddRay(new Ray(hp, dir), depth + 1, coef * ((float)hitShape.surface.reflectiveness / 100.0f), ref UGH);

                //calculate refraction (nigguh)
                float refr = hitShape.surface.RefractionIndex;
                if (refr > 0)
                {
                    float n = lastRIndex/refr;

                    float cos = Vector3.Dot(ray.Direction, vNormal);
                    float sin = (float)Math.Pow(n, 2) * (1 - (float)Math.Pow(cos,2));
                    if (Math.Pow(sin,2) <= 1)
                    {
                        Vector3 transmissiveDir = n * ray.Direction - (n * cos + (float)Math.Sqrt(1 - (float)Math.Pow(sin, 2))) * vNormal;
                        transmissiveDir.Normalize();
                        float dist = 0;
                        Vector3 theColor = AddRay(new Ray(hp, transmissiveDir), depth + 1, coef, ref dist, refr);

                        Vector3 absorbance = hitShape.surface.color * 0.0000015f * -dist;
                        Vector3 trans = new Vector3((float)Math.Exp(absorbance.X), (float)Math.Exp(absorbance.Y), (float)Math.Exp(absorbance.Z));

                        curColor += theColor * trans;

                    }
                }

            }

            return Clamp(curColor);

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

        private float isVisible(Light L, Vector3 hitPoint, Ray ray)
        {

            float retVal = 0.0f;
            float offset = 5;


            float numSegments = 1;
            float numAlongSegment = 1;

            Vector3 dir;
            Ray r;

            for (float i = 0; i <= 2 * Math.PI; i += 2.0f * (float)Math.PI / (numSegments))
            {

                for (float j = 1; j <= numAlongSegment; j++)
                {

                    dir = L.position + new Vector3((float)Math.Cos(i) * L.radius / j + offset * (Math.Cos(i) < 0 ? 1 : -1), (float)Math.Sin(i) * L.radius / j + offset * (Math.Sin(i) < 0 ? 1 : -1), 0);
                    dir = dir - ray.Start;
                    dir.Normalize();

                    r = new Ray(ray.Start, dir);

                    if (FindClosestShape(r, L) == L)
                        retVal += 1.0f / (numSegments * numAlongSegment);

                }
            }

            return Clamp(retVal, 0, 1);
            

        }

        private GeometricObject FindClosestShape(Ray r, Light l)
        {
            double dist = float.MaxValue;
            GeometricObject closest = null;

            foreach (GeometricObject shape in Shapes)
            {
                double t=shape.intersection(r);
                if (t < dist && t > 0.0f && shape.surface.RefractionIndex < 1)
                {
                    closest = shape;
                    dist = t;
                }

            }

            foreach (Light light in Lights)
            {
                if (light != l) continue;
                double t = light.intersection(r);
                if (light.intersection(r) < dist)
                {
                    closest = light;
                    dist = t;
                }

            }

            return closest;

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
            if (v.X > 1) v.X = 1;
            else if (v.X < 0) v.X = 0;

            if (v.Y > 1) v.Y = 1;
            else if (v.Y < 0) v.Y = 0;

            if (v.Z > 1) v.Z = 1;
            else if (v.Z < 0) v.Z = 0;

            return v;
        }
    }
}
