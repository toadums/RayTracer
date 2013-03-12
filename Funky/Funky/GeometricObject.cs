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
        public Vector3 color;
        public Vector3 position;
        public Vector3 normal;
    }
}
