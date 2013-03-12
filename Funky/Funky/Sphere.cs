using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{

    public class Sphere : GeometricObject
    {
        public float radius;
        public float INTERSECTION_TOLERANCE = 10.0f;

        //Sphere constructor
        public Sphere(float r, Vector3 p, Vector4 c, SurfaceType s)
        {
            radius = r;

            // position is the point at the centre of the sphere.
            position = p;
            color = c;
            surface = s;
        }

        //This function draws a sphere.
        public void draw()
        {

        }

        public override double intersection(Ray ray)
        {
            // sRay is the position of the eye/camera, dRay is the direction of the ray.
            // the new start and end of the ray for these calculations.
            Vector3 sRay = new Vector3();
            Vector3 dRay = ray.Direction;
            dRay.Normalize();

            //coefficients of quadratic equation, and discriminant
            double a, b, c, d, t;

            // transform ray to put sphere centre at origin to make calculations cleaner.
            sRay = ray.Start - position;

            /*
            a = Vector3.Dot(ray.Direction, ray.Direction);
            b = 2 * Vector3.Dot(ray.Direction, ray.Start);
            c = Vector3.Dot(ray.Start, ray.Start) - (radius * radius);
            */

            a = Vector3.Dot(dRay, dRay);
            b = 2*Vector3.Dot(dRay, sRay);
            c = Vector3.Dot(sRay, sRay) - radius * radius;


            //Find discriminant
            d = b * b - 4 * a * c;

            // if discriminant < 0, then the ray did not intersect the object.
            if (d < 0.0)
            {
                return 0.0;
            }
            else
            {
                d = Math.Sqrt(d);

                // If there are one or two real values then chose the smallest, 
                // non-negative value, as the intersection point.
                t = (-b - d) / (2.0 * a);

                if (t < INTERSECTION_TOLERANCE) t = (-b + d) / (2.0 * a);

                if (t < INTERSECTION_TOLERANCE) return 0.0;
                else return t;
            }
        }

        // calculate the normal of the sphere at the point of 
        // intersection, i, from the eye/camera, from.	
        public Vector3 normal(Vector3 i, Vector3 from)
        {
            Vector3 normal = new Vector3();
            normal.X = (i.X - position.X) / radius;
            normal.Y = (i.Y - position.Y) / radius;
            normal.Z = (i.Z - position.Z) / radius;

            Vector3 r = new Vector3();
            r.X = i.X - from.X;
            r.Y = i.Y - from.Y;
            r.Z = i.Z - from.Z;

            // if the ray has some portion travelling in the same direction then it is coming from
            // inside the sphere, therefore we must flip the normal.
            if (Vector3.Dot(normal, r) > 0)
            {
                normal.X *= -1;
                normal.Y *= -1;
                normal.Z *= -1;
            }
            return normal;
        }
    }
}
