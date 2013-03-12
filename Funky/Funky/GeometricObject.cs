using SharpDX;
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

        public virtual double intersection(Ray ray)
        {
            throw new Exception("INTERSECTION NOT IMPLEMENTED!!!");
        }


    }
}
