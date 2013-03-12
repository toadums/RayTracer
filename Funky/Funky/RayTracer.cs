
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

        public Vector3 Eye;
        
        private List<GeometricObject> Shapes;

        public RayTracer(ref WriteableBitmap wb, ref TextBlock fps)
        {
            WB = wb;
            FPS = fps;

            Eye = new Vector3(MainPage.ImageSize / 2.0f, -10000);

            Shapes = new List<GeometricObject>();

            Shapes.Add(new Sphere(50,new Vector3(150,200,0), new Vector4(255,0,0,255), 
                new SurfaceType(new Vector3(), new Vector3(), new Vector3())));

            Shapes.Add(new Sphere(75, new Vector3(250, 200, 0), new Vector4(255, 0, 255, 255),
               new SurfaceType(new Vector3(), new Vector3(), new Vector3())));

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

            Random rand = new Random();

            // Plot the Mandelbrot set on x-y plane
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector4 color = new Vector4(0, 0, 0, 0);
                    int hit = 0;

                    foreach (GeometricObject s in Shapes)
                    {

                        double d = s.intersection(new Ray(Eye, (new Vector3(x, y, 0)) - Eye));
                        if (d > 0)
                        {

                            color += s.color;
                            hit++;
                        }

                    }

                    if(hit > 0) color /= (float)hit;

                    //NOTICE the xyzw dont correspond to RGBA.
                    result[resultIndex++] = Convert.ToByte(color.Y); // Green value of pixel
                    result[resultIndex++] = Convert.ToByte(color.Z); // Blue value of pixel
                    result[resultIndex++] = Convert.ToByte(color.X); // Red value of pixel
                    result[resultIndex++] = Convert.ToByte(color.W); // Alpha value of pixel
                }
            }

            return result;
        }

    }
}
