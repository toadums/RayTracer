using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{

    public class Cube : GeometricObject
    {
        Vector3 frontFloorPoint,frontTopPoint;
        Vector3 backTopPoint, backFloorPoint, leftTopPoint, leftFloorPoint, rightTopPoint, rightFloorPoint;
        float height;
        float width;
        float depth;
        List<GeometricObject> planes;
        Triangle hitTri;

        Vector3 white = new Vector3(.76f, .75f, .5f);
        Vector3 red = new Vector3(.63f, .06f, .04f);
        Vector3 green = new Vector3(.15f, .48f, .09f);

        public Cube()
        {

        }

        //Sphere constructor
        public Cube(Vector3 frontFloor, float h, float w, float d)
        {
            

            planes = new List<GeometricObject>();

            hitTri = new Triangle();

            frontFloorPoint = frontFloor;
            height = h;
            width = w;
            depth = d;

            frontTopPoint = frontFloorPoint - new Vector3(0, height, 0);

            leftFloorPoint = new Vector3(frontFloorPoint.X - width, frontFloorPoint.Y, frontFloorPoint.Z + depth);
            backFloorPoint = new Vector3(frontFloorPoint.X, frontFloorPoint.Y, frontFloorPoint.Z + depth + depth);
            rightFloorPoint = new Vector3(frontFloorPoint.X + width, frontFloorPoint.Y, frontFloorPoint.Z + depth);

            leftTopPoint = leftFloorPoint - new Vector3(0, height, 0);
            backTopPoint = backFloorPoint - new Vector3(0, height, 0);
            rightTopPoint = rightFloorPoint - new Vector3(0, height, 0);



        }

        public void buildCube()
        {

            //Front left plane
            planes.Add(new Triangle(
                frontFloorPoint, leftFloorPoint, leftTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));

            planes.Add(new Triangle(
                frontFloorPoint, leftTopPoint, frontTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));

            //Front Right plane
            planes.Add(new Triangle(
                rightFloorPoint,frontFloorPoint,rightTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));

            planes.Add(new Triangle(
                rightTopPoint, frontFloorPoint,frontTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));
            
            //Back left plane
            planes.Add(new Triangle(
                backFloorPoint, leftFloorPoint, leftTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));

            planes.Add(new Triangle(
                backFloorPoint, leftTopPoint, backTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));

            //Back right plane
            planes.Add(new Triangle(
                rightFloorPoint,backFloorPoint, rightTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));

            planes.Add(new Triangle(
                rightTopPoint, backFloorPoint, backTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));

            //Top plane
            planes.Add(new Triangle(
                leftTopPoint, rightTopPoint,frontTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));

            planes.Add(new Triangle(
                leftTopPoint, rightTopPoint,backTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));

            //Bottom plane
            planes.Add(new Triangle(
                leftFloorPoint, rightFloorPoint, frontFloorPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));

            planes.Add(new Triangle(
                leftFloorPoint, rightFloorPoint, backFloorPoint, 
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), surface.color, 0)));

            //return planes;
        }

        public override Tuple<double,Triangle> intersectionCube(Ray ray)
        {
            float closest = float.PositiveInfinity;

            foreach (Triangle triangle in planes)
            {
                Vector3 p0 = triangle.Vertices[0];
                Vector3 p1 = triangle.Vertices[2];
                Vector3 p2 = triangle.Vertices[1];

                Vector3 e1 = p1 - p0;
                Vector3 e2 = p2 - p0;

                Vector3 p = Vector3.Cross(ray.Direction, e2);

                float a = Vector3.Dot(e1, p);

                if (a < 0.000001) continue;// return 0.0;

                float f = 1.0f / a;

                Vector3 s = ray.Start - p0;

                float u = f * Vector3.Dot(s, p);

                if (u < 0.0 || u > 1.0) continue;// return 0.0;

                Vector3 q = Vector3.Cross(s, e1);
                float v = f * Vector3.Dot(ray.Direction, q);
                if (v < 0.0 || u + v > 1.0) continue;// return 0.0;

                float t = f * Vector3.Dot(e2, q);

                //return t;
                if (t < closest)
                {
                    closest = t;
                    hitTri = triangle;
                }
            }

            if (closest < float.PositiveInfinity)
            {
                return new Tuple<double,Triangle>(closest,hitTri);
            }
            else
                return new Tuple<double, Triangle>(0.0, null) ;


        }

        public override Vector3 NormalAtCube(Vector3 i, Vector3 from, Triangle tri)
        {
            if (tri.Normal.X != float.MaxValue) return tri.Normal;
            else return Vector3.Cross(tri.Vertices[0] - tri.Vertices[1], tri.Vertices[2] - tri.Vertices[1]);
        }


    }
}