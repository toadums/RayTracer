﻿using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{
    public class SurfaceType
    {
        // SurfaceType class
        public Vector3 diffuse;
        public Vector3 ambient;
        public Vector3 specular;
        public Vector3 color;
        public double reflectiveness;
        //public Vector3 transmissive;//?

        public SurfaceType(Vector3 d, Vector3 a, Vector3 s, Vector3 c, double r)
        {
            diffuse = d;
            ambient = a;
            specular = s;
            reflectiveness = r;
            color = c;
        }
    }
}