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
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using WinRTXamlToolkit.Imaging;

namespace Funky
{
    class objLoader
    {

        public static void Sleep(int ms)
        {
            new System.Threading.ManualResetEvent(false).WaitOne(ms);

        }

        public List<Triangle> Triangles { get; set; }

        public objLoader()
        {
            Triangles = new List<Triangle>();
        }

        public async Task CreateTriangles(string filename, float scale, Vector3 position)
        {

            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var file = await folder.GetFileAsync(filename);
            var wholeFile = await FileIO.ReadTextAsync(file);

            int v = 0;
            int t = 0;
            int n = 0;
            string[] read = wholeFile.Split('\n');
            byte[] pixels = new byte[1];
            WriteableBitmap wbmp = null;
            Vector2 X = new Vector2(float.MaxValue, float.MinValue), Y = new Vector2(float.MaxValue, float.MinValue), Z = new Vector2(float.MaxValue, float.MinValue);

            FunkyTexture texture = new FunkyTexture();

            string mtlFilename = string.Empty;

            foreach (string s in read)
            {
                if (s.Length == 0) continue;
                if (s[0] == 'v' && s[1] == ' ')
                {
                    v++;
                    string[] temp = s.Split(' ');
                    float x = float.Parse(temp[1]);
                    if (x < X.X) X.X = x;
                    if (x > X.Y) X.Y = x;

                    float y = float.Parse(temp[2]);
                    if (y < Y.X) Y.X = y;
                    if (y > Y.Y) Y.Y = y;

                    float z = float.Parse(temp[3]);
                    if (z < Z.X) Z.X = z;
                    if (z > Z.Y) Z.Y = z;

                }
                else if (s[0] == 'v' && s[1] == 't')
                {
                    t++;
                }
                else if (s[0] == 'v' && s[1] == 'n')
                {
                    n++;
                }
                else if (s.Contains("mtllib"))
                {

                    mtlFilename = s.Split(' ')[1];
                    var mtlFile = await folder.GetFileAsync(mtlFilename);
                    var mtlText = await FileIO.ReadTextAsync(mtlFile);

                    foreach (string ms in mtlText.Trim().Split('\n'))
                    {
                        if (ms.Contains("map_Kd"))
                        {
                            string texFilename = ms.Split(' ')[1];

                            var texFile = await folder.GetFileAsync(texFilename);
                            var properties = await texFile.Properties.GetImagePropertiesAsync();

                            wbmp = new WriteableBitmap((Int32)properties.Width, (Int32)properties.Height);

                            await wbmp.LoadAsync(texFile);

                            pixels = new byte[wbmp.PixelWidth * wbmp.PixelHeight * 4];

                            using (Stream pixelStream = wbmp.PixelBuffer.AsStream())
                            {
                                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                            }
                            wbmp.Invalidate();
                            texture.TexturePixels = new Vector3[wbmp.PixelHeight, wbmp.PixelWidth];
                            int r = 0, c = 0;
                            for (int i = 0; i < pixels.Length; i++)
                            {

 
                           texture.TexturePixels[r, c] = new Vector3(pixels[i+2], pixels[i+1], pixels[i+0])/255.0f;


                                c = (c + 1) % wbmp.PixelWidth;

                                if (c == 0)
                                {
                                    r = (r + 1) % wbmp.PixelHeight;

                                }

                                i+=3;

                            }

                            texture.TexSize = new Vector2(wbmp.PixelWidth, wbmp.PixelHeight);

                        }
                    }
                }

            }
            if (X.X == 0) X.X = float.MinValue;
            if (X.Y == 0) X.Y = float.MinValue;

            if (Y.X == 0) Y.X = float.MinValue;
            if (Y.Y == 0) Y.Y = float.MinValue;

            if (Z.X == 0) Z.X = float.MinValue;
            if (Z.Y == 0) Z.Y = float.MinValue;

            Vector3[] vertices = new Vector3[v];
            Vector3[] Normals = new Vector3[n];
            Vector2[] Texture = new Vector2[t];

            int cV = 0, cN = 0, cT = 0;

            foreach (string s in read)
            {
                if (s.Length == 0) continue;
                if (s[0] == 'v')
                {
                    if (s[1] == ' ')
                    {
                        string[] temp = s.Split(' ');

                        float a = float.Parse(temp[1]);
                        float b = -float.Parse(temp[2]);
                        float c = float.Parse(temp[3]);

                        vertices[cV] = new Vector3(

                            (a >= 0 ? (a / X.Y) : -(a / X.X)) * scale + position.X,
                            (b >= 0 ? (b / Y.Y) : -(b / Y.X)) * scale + position.Y,
                            (c >= 0 ? (c / Z.Y) : -(c / Z.X)) * scale + position.Z
                            );
                        cV++;
                    }
                    if (s[1] == 't')
                    {
                        string[] temp = s.Split(' ');

                        float a = float.Parse(temp[1]);
                        float b = float.Parse(temp[2]);

                        Texture[cT] = new Vector2(a, b);
                        cT++;
                    }

                    if (s[1] == 'n')
                    {
                        string[] temp = s.Split(' ');

                        float a = float.Parse(temp[1]);
                        float b = float.Parse(temp[2]);
                        float c = float.Parse(temp[3]);

                        Normals[cN] = new Vector3(a, b, c);
                        cN++;
                    }
                }
            }

            int count = 0;
            foreach (string s in read)
            {
                if (s.Length == 0) continue;
                if (s[0] == 'f')
                {
                    int a, b, c;
                    if (s.Contains("/"))
                    {
                        string[] temp = s.Split(' ');

                        a = int.Parse(temp[1].Split('/')[0]) - 1;
                        b = int.Parse(temp[2].Split('/')[0]) - 1;
                        c = int.Parse(temp[3].Split('/')[0]) - 1;

                        int aa = int.Parse(temp[1].Split('/')[1]) - 1;
                        int bb = int.Parse(temp[2].Split('/')[1]) - 1;
                        int cc = int.Parse(temp[3].Split('/')[1]) - 1;

                        int aaa = int.Parse(temp[1].Split('/')[2]) - 1;
                        int bbb = int.Parse(temp[2].Split('/')[2]) - 1;
                        int ccc = int.Parse(temp[3].Split('/')[2]) - 1;

                       
                        Vector3 v1 = vertices[b - a];
                        Vector3 v2 = vertices[c - a];

                        v1 = Vector3.Cross(v1,v2);
                        v1.Normalize();

                        v2= new Vector3(RayTracer.ImageSize.X/2.0f, RayTracer.ImageSize.Y/2.0f,0) - RayTracer.Eye;
                        v2.Normalize();

                      Triangles.Add(new Triangle(vertices[a], vertices[b], vertices[c], new Vector2[]{
                            Texture[aa], Texture[bb], Texture[cc]},
                       new SurfaceType()
                       {
                           color = (new Vector3(1, 1, 0)),

                           type = textureType.standard,
                           specular = new Vector3(1, 1, 1),
                           SpecExponent = 9999999999,
                       }) {TextureToUse = texture, TCC = TextureCoordConst.Z} );

                    }
                    else
                    {

                        string[] temp = s.Split(' ');
                         a = int.Parse(temp[1]) - 1;
                         b = int.Parse(temp[2]) - 1;
                         c = int.Parse(temp[3]) - 1;

                         Triangles.Add(new Triangle(vertices[a], vertices[b], vertices[c],
                         new SurfaceType()
                         {
                             color = new Vector3(0.64253f, 0.12354f, 0986345f),
                             type = textureType.standard,
                             specular = new Vector3(1, 1, 1),
                             SpecExponent = 0,
                         }));

                    }

                    
                    //if (count++ > 100) break;

                }
            }
            
        }

    }
}
