﻿using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{
    public enum textureType { standard, bump };
    public class SurfaceType
    {
        // SurfaceType class
        public textureType type;
        public Vector3 diffuse;
        public Vector3 ambient;
        public Vector3 specular;
        public Vector3 color;
        public double reflectiveness;
        public float SpecExponent;
        public float RefractionIndex = 0;

        //public Vector3 transmissive;

        public SurfaceType()
        {

        }

        public SurfaceType(textureType t, Vector3 d, Vector3 a, Vector3 s, Vector3 c, double r)
        {
            type = t;
            diffuse = d;
            ambient = a;
            specular = s;
            reflectiveness = r;
            color = c;
            SpecExponent = 0;
        }

        public SurfaceType(textureType t, Vector3 d, Vector3 a, Vector3 s, Vector3 c, double r, float se)
        {
            type = t;
            diffuse = d;
            ambient = a;
            specular = s;
            reflectiveness = r;
            color = c;
            SpecExponent = se;
        }
    }
}
