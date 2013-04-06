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

        Vector3 white = new Vector3(.76f, .75f, .5f);
        Vector3 red = new Vector3(.63f, .06f, .04f);
        Vector3 green = new Vector3(.15f, .48f, .09f);

        public Cube()
        {

        }

        //Sphere constructor
        public Cube(Vector3 frontFloor, float h, float w, float d)
        {
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

        public List<GeometricObject> buildCube()
        {
            planes = new List<GeometricObject>();

            //Front left plane
            planes.Add(new Triangle(
                frontFloorPoint, leftFloorPoint, leftTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));

            planes.Add(new Triangle(
                frontFloorPoint, leftTopPoint, frontTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));

            //Front Right plane
            planes.Add(new Triangle(
                rightFloorPoint,frontFloorPoint,rightTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));

            planes.Add(new Triangle(
                rightTopPoint, frontFloorPoint,frontTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));
            
            //Back left plane
            planes.Add(new Triangle(
                backFloorPoint, leftFloorPoint, leftTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));

            planes.Add(new Triangle(
                backFloorPoint, leftTopPoint, backTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));

            //Back right plane
            planes.Add(new Triangle(
                rightFloorPoint,backFloorPoint, rightTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));

            planes.Add(new Triangle(
                rightTopPoint, backFloorPoint, backTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));

            //Top plane
            planes.Add(new Triangle(
                leftTopPoint, rightTopPoint,frontTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));

            planes.Add(new Triangle(
                leftTopPoint, rightTopPoint,backTopPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));

            //Bottom plane
            planes.Add(new Triangle(
                leftFloorPoint, rightFloorPoint, frontFloorPoint,
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));

            planes.Add(new Triangle(
                leftFloorPoint, rightFloorPoint, backFloorPoint, 
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), white, 0)));

            return planes;
        }



            

        //This function draws a sphere.
        public void draw()
        {

        }
    }
}