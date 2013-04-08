using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{



    class Triangle : GeometricObject
    {
        public Vector2[] TextureCoords;
        public Vector3[] Vertices;
        Vector3 Normal;

        public Triangle()
        {
            TextureCoords = new Vector2[]{new Vector2(float.MaxValue, float.MaxValue)};
            Normal = new Vector3(float.MaxValue);
        }

        /// <summary>
        /// Constructor for a triangle. Points must be added in a clockwise manner
        /// </summary>
        /// <param name="a">first point</param>
        /// <param name="b">second point</param>
        /// <param name="c">third point</param>
        /// <param name="st">the surface texture (color, texture, etc)</param>
        public Triangle(Vector3 a, Vector3 b, Vector3 c, SurfaceType st)
        {
            Vertices = new Vector3[] { a, b, c };
            this.surface = st;

            Normal = new Vector3(float.MaxValue);

        }

        public bool HasTexture{
            get { if (TextureCoords != null && TextureCoords.Length > 0 && TextureCoords[0].X != float.MaxValue)return true; else return false; }
        }

        /// <summary>
        /// Constructor for a triangle. Points must be added in a clockwise manner
        /// </summary>
        /// <param name="a">first point</param>
        /// <param name="b">second point</param>
        /// <param name="c">third point</param>
        /// <param name="st">the surface texture (color, texture, etc)</param>
        public Triangle(Vector3 a, Vector3 b, Vector3 c, Vector2[] textures, SurfaceType st)
        {
            Vertices = new Vector3[] { a, b, c };
            this.surface = st;
            TextureCoords = textures;

            Normal = new Vector3(float.MaxValue);

        }

        public override double intersection(Ray ray)
        {
            Vector3 p0 = Vertices[0];
            Vector3 p1 = Vertices[2];
            Vector3 p2 = Vertices[1];

            Vector3 e1 = p1 - p0;
            Vector3 e2 = p2 - p0;

            Vector3 p = Vector3.Cross(ray.Direction, e2);

            float a = Vector3.Dot(e1, p);

            if (a < 0.000001) return 0.0;

            float f = 1.0f / a;

            Vector3 s = ray.Start - p0;

            float u = f * Vector3.Dot(s, p);

            if (u < 0.0 || u > 1.0) return 0.0;

            Vector3 q = Vector3.Cross(s, e1);
            float v = f * Vector3.Dot(ray.Direction, q);
            if (v < 0.0 || u + v > 1.0) return 0.0;

            float t = f * Vector3.Dot(e2, q);

            return t;
        }

        public override Vector3 NormalAt(Vector3 i, Vector3 from)
        {
            if (Normal.X != float.MaxValue) return Normal;
            else return Vector3.Cross(Vertices[0] - Vertices[1], Vertices[2] - Vertices[1]);
        }


        public override string ToString()
        {
            return "Triangle: 1: " + Vertices[0] + ", 2: " + Vertices[1] + ", 3: " + Vertices[2];
        }

    }
}
