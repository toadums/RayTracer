using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{
    public class Ray
    {
        // Starting and ending points for the ray.
        public Vector3 Start;
        public Vector3 Direction;

        public Ray(Vector3 s, Vector3 d)
        {
            Start = s;
            Direction = d;
        }

    }
}
