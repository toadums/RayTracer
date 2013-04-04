using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

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

        public async Task CreateTriangles(string filename)
        {

            var folder = Package.Current.InstalledLocation;
            var file = await folder.GetFileAsync(filename);
            var wholeFile = await FileIO.ReadTextAsync(file);

            int count = 0;

            string[] read = wholeFile.Split('\n');

            foreach (string s in read)
            {
                if (s.Length == 0) continue;
                if (s[0] == 'v')
                {
                    count++;
                }
            }

            Vector3[] vertices = new Vector3[count];
            count = 0;

            foreach (string s in read)
            {
                if (s.Length == 0) continue;
                if (s[0] == 'v')
                {
                    string[] temp = s.Split(' ');
                    vertices[count] = new Vector3(float.Parse(temp[1]) * RayTracer.ImageSize.X / 10.0f + RayTracer.ImageSize.X / 2.0f, (float.Parse(temp[2]) * RayTracer.ImageSize.X / 10.0f + RayTracer.ImageSize.X / 2.0f), float.Parse(temp[3]) * RayTracer.ImageSize.X / 10.0f + 2000);
                    count++;
                }
            }
            count = 0;
            foreach (string s in read)
            {
                if (s.Length == 0) continue;
                if (s[0] == 'f')
                {
                    string[] temp = s.Split(' ');
                    int a = int.Parse(temp[1]) - 1;
                    int b = int.Parse(temp[2]) - 1;
                    int c = int.Parse(temp[3]) - 1;

                    float r = new Random().NextFloat(0, 1);
                    Sleep(1);
                    float g = new Random().NextFloat(0, 1);
                    Sleep(1);
                    float bl = new Random().NextFloat(0, 1);

                    Vector3 col = new Vector3(r, g, bl);

                    Triangles.Add(new Triangle(vertices[c], vertices[b], vertices[a],
                        new SurfaceType()
                        {
                            color = col,
                            type = textureType.standard,
                        }));
                    //if (count++ > 100) break;

                }
            }
            
        }

    }
}
