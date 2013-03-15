﻿using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{
    public abstract class GeometricObject
    {
        public SurfaceType surface;
        public Vector4 color;
        public Vector3 position;
        public Vector3 normal;
        public bool IsLight;
        public static float INTERSECTION_TOLERANCE = 10.0f;

        public virtual double intersection(Ray ray)
        {
            throw new Exception("INTERSECTION NOT IMPLEMENTED!!!");
        }

        public virtual Vector3 NormalAt(Vector3 i, Vector3 from)
        {
            throw new Exception("INTERSECTION NOT IMPLEMENTED!!!");
        }

    }
}