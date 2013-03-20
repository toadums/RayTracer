﻿using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{
    class Triangle : GeometricObject
    {

        Vector3[] Vertices;

        public Triangle(Vector3 a, Vector3 b, Vector3 c, Vector4 color, SurfaceType st)
        {
            Vertices = new Vector3[] { a, b, c };
            this.color = color;
            this.surface = st;
        }

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            Vertices = new Vector3[] { a, b, c };
            this.color = new Vector4(255,255,255, 255);
            this.surface = new SurfaceType(new Vector3(255, 255, 255), new Vector3(255, 255, 255), new Vector3(255, 255, 255), new Vector3(255, 255, 255), 100);
            
        }


        public override double intersection(Ray ray)
        {
            Vector3 p0 = Vertices[0];
            Vector3 p1 = Vertices[2];
            Vector3 p2 = Vertices[1];

            Vector3 e1 = p1 - p0;
            Vector3 e2 = p2 - p0;

            Vector3 e1e2 = Vector3.Cross(e1, e2);
            Vector3 p = Vector3.Cross(ray.Direction, e2);

            e1e2.Normalize();

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
            return Vector3.Cross(Vertices[0] - Vertices[1], Vertices[2] - Vertices[1]);
        }


        public override string ToString()
        {
            return "Triangle: color: " + this.color.ToString() + "//// radius: ";
        }

    }
}
