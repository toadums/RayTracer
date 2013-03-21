using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{
    partial class RayTracer
    {
        private void DrawGeometry(){
            Shapes = new List<GeometricObject>();

            Shapes.Add(new Sphere(ImageSize.Y / 4.0f, new Vector3(ImageSize.X / 2.0f, ImageSize.Y / 2.0f, SphereDist), new Vector4(100, 0, 0, 255),
                new SurfaceType(textureType.standard, new Vector3(100, 100, 100), new Vector3(100, 100, 100), new Vector3(255, 255, 255), new Vector3(100, 100, 100), 100,50)));

            Shapes.Add(new Sphere(ImageSize.Y / 15.0f, new Vector3(ImageSize.X / 2.0f + ImageSize.X / 3.0f, ImageSize.Y / 2.0f, SphereDist), new Vector4(255, 0, 0, 255),
                new SurfaceType(textureType.standard, new Vector3(0, 100, 255), new Vector3(100, 40, 78), new Vector3(50, 50, 50), new Vector3(0, 0, 255), 50, 0)));

            Shapes.Add(new Sphere(ImageSize.Y / 15.0f, new Vector3(ImageSize.X / 2.0f - ImageSize.X / 3.0f, ImageSize.Y / 2.0f, SphereDist), new Vector4(29, 43, 200, 255),
                new SurfaceType(textureType.standard, new Vector3(33, 212, 43), new Vector3(100, 40, 78), new Vector3(50, 50, 50), new Vector3(12, 235, 92), 50, 0)));
            
            Lights = new List<Light>() { new Light() { position = new Vector3(ImageSize.X/2.0f, ImageSize.Y / 2.0f, 10),color = new Vector3(255, 255, 255)}};
           
            DrawBox();
        }

        private void DrawBox()
        {


            //Left side 1
            Shapes.Add(new Triangle(
               new Vector3(0, 0, 0),
                   new Vector3(0, ImageSize.Y, SphereDist * 2),
               new Vector3(0, ImageSize.Y, 0),
               new Vector4(0, 255, 0, 255),
               new SurfaceType(textureType.standard, new Vector3(0, 0, 255), new Vector3(0, 0, 0), new Vector3(255, 255, 255), new Vector3(0, 255, 255), 0)));


            //Left side 2
            Shapes.Add(new Triangle(
               new Vector3(0, 0, 0),
                   new Vector3(0, 0, SphereDist * 2),
               new Vector3(0, ImageSize.Y, SphereDist * 2),
               new Vector4(0, 255, 0, 255),
               new SurfaceType(textureType.standard, new Vector3(0, 0, 255), new Vector3(0, 0, 0), new Vector3(255, 255, 255), new Vector3(0, 255, 255), 0)));


            //Bottom side 1
            Shapes.Add(new Triangle(
               new Vector3(0, ImageSize.Y, 0),
                   new Vector3(0, ImageSize.Y, SphereDist * 2),
               new Vector3(ImageSize.X, ImageSize.Y, 0),
               new Vector4(0, 255, 0, 255),
               new SurfaceType(textureType.standard, new Vector3(0, 255, 0), new Vector3(0, 255, 0), new Vector3(255, 255, 255), new Vector3(0, 255, 0), 0)));


            //Bottom side 2
            Shapes.Add(new Triangle(
               new Vector3(0, ImageSize.Y, SphereDist * 2),
                   new Vector3(ImageSize.X, ImageSize.Y, SphereDist * 2),
               new Vector3(ImageSize.X, ImageSize.Y, 0),
               new Vector4(0, 255, 0, 255),
               new SurfaceType(textureType.standard, new Vector3(0, 255, 0), new Vector3(0, 255, 0), new Vector3(255, 255, 255), new Vector3(0, 255, 0), 0)));



            //Back side 1
            Shapes.Add(new Triangle(
               new Vector3(0, ImageSize.Y, SphereDist * 2),
                   new Vector3(0, 0, SphereDist * 2),
               new Vector3(ImageSize.X, ImageSize.Y, SphereDist * 2),
                              new Vector4(255, 0, 0, 255),
               new SurfaceType(textureType.standard, new Vector3(255, 0, 0), new Vector3(255, 0, 0), new Vector3(255, 0, 0), new Vector3(255, 0, 0), 0)));

            //Back side 2
            Shapes.Add(new Triangle(
               new Vector3(0, 0, SphereDist * 2),
                   new Vector3(ImageSize.X, 0, SphereDist * 2),
               new Vector3(ImageSize.X, ImageSize.Y, SphereDist * 2),
                              new Vector4(255, 0, 0, 255),
               new SurfaceType(textureType.standard, new Vector3(255, 0, 0), new Vector3(255, 0, 0), new Vector3(255, 0, 0), new Vector3(255, 0, 0), 0)));


            //Top side 1
            Shapes.Add(new Triangle(
               new Vector3(0, 0, 0),
               new Vector3(ImageSize.X, 0, 0),
                   new Vector3(0, 0, SphereDist * 2),
                              new Vector4(0, 0, 255, 255),
               new SurfaceType(textureType.standard, new Vector3(0, 0, 255), new Vector3(0, 0, 255), new Vector3(0, 0, 255), new Vector3(0, 0, 255), 0)));

            //top side 2
            Shapes.Add(new Triangle(
               new Vector3(0, 0, SphereDist * 2),
                   new Vector3(ImageSize.X, 0, 0),
               new Vector3(ImageSize.X, 0, SphereDist * 2),
                            new Vector4(0, 0, 255, 255),
               new SurfaceType(textureType.standard, new Vector3(0, 0, 255), new Vector3(0, 0, 255), new Vector3(0, 0, 255), new Vector3(0, 0, 255), 0)));

            //Top side 1
            Shapes.Add(new Triangle(
               new Vector3(ImageSize.X, 0, 0),
               new Vector3(ImageSize.X, ImageSize.Y, 0),
                   new Vector3(ImageSize.X, ImageSize.X, SphereDist * 2),
                              new Vector4(255, 0, 255, 255),
               new SurfaceType(textureType.standard, new Vector3(255, 0, 255), new Vector3(255, 0, 255), new Vector3(255, 0, 255), new Vector3(255, 0, 255), 0)));

            //Top side 1
            Shapes.Add(new Triangle(
                   new Vector3(ImageSize.X, 0, SphereDist * 2),
               new Vector3(ImageSize.X, 0, 0),
                   new Vector3(ImageSize.X, ImageSize.X, SphereDist * 2),
                              new Vector4(255, 0, 255, 255),
               new SurfaceType(textureType.standard, new Vector3(255, 0, 255), new Vector3(255, 0, 255), new Vector3(255, 0, 255), new Vector3(255, 0, 255), 0)));



        }

    }
}
