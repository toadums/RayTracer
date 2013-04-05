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

        public async Task CreateTriangles(string filename, float scale, Vector3 position)
        {

            var folder = Package.Current.InstalledLocation;
            var file = await folder.GetFileAsync(filename);
            var wholeFile = await FileIO.ReadTextAsync(file);

            int count = 0;

            string[] read = wholeFile.Split('\n');
            Vector2 X = new Vector2(float.MaxValue, float.MinValue), Y = new Vector2(float.MaxValue, float.MinValue), Z = new Vector2(float.MaxValue, float.MinValue);
            foreach (string s in read)
            {
                if (s.Length == 0) continue;
                if (s[0] == 'v')
                {
                    count++;
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
            }

            Vector3[] vertices = new Vector3[count];
            count = 0;

            foreach (string s in read)
            {
                if (s.Length == 0) continue;
                if (s[0] == 'v')
                {
                    string[] temp = s.Split(' ');

                    float a = float.Parse(temp[1]);
                    float b = -float.Parse(temp[2]);
                    float c = float.Parse(temp[3]);

                    vertices[count] = new Vector3(

                        (a >= 0 ? (a / X.Y) : -(a / X.X)) * scale + position.X,
                        (b >= 0 ? (b / Y.Y) : -(b / Y.X)) * scale + position.Y,
                        c + position.Z
                        );
                    System.Diagnostics.Debug.WriteLine(vertices[count]);
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

                    Triangles.Add(new Triangle(vertices[a], vertices[b], vertices[c],
                        new SurfaceType()
                        {
                            color = new Vector3(0.64253f, 0.12354f, 0986345f),
                            type = textureType.standard,
                            specular = new Vector3(1,1,1),
                            SpecExponent = 500,
                        }));
                    //if (count++ > 100) break;

                }
            }
            
        }

    }
}
