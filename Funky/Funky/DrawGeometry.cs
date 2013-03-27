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

            Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X / 2.0f, ImageSize.Y / 2.0f, SphereDist),
                radius = ImageSize.X / 4.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(100, 100, 100),
                    diffuse = new Vector3(100, 100, 100),
                    specular = new Vector3(255, 255, 255),
                    color = new Vector3(100, 100, 100),
                    reflectiveness = 100,
                    SpecExponent = 5,
                    Refraction = 0,
                    RefractionIndex = 0
                }
            });

            Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X / 2.0f + ImageSize.X / 3.0f, ImageSize.Y / 2.0f, SphereDist),
                radius = ImageSize.X / 15.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 100, 255),
                    diffuse = new Vector3(100, 40, 78),
                    specular = new Vector3(50, 50, 50),
                    color = new Vector3(12, 235, 92),
                    reflectiveness = 50,
                    SpecExponent = 5,
                    Refraction = 0,
                    RefractionIndex = 0
                }
            });

            Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X / 2.0f - ImageSize.X / 3.0f, ImageSize.Y / 2.0f, SphereDist),
                radius = ImageSize.X / 15.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 100, 255),
                    diffuse = new Vector3(100, 40, 78),
                    specular = new Vector3(50, 50, 50),
                    color = new Vector3(235, 2, 92),
                    reflectiveness = 50,
                    SpecExponent = 5,
                    Refraction = 0,
                    RefractionIndex = 0
                }
            });

            /*Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X / 2.0f - ImageSize.X / 3.0f, ImageSize.Y / 2.0f, SphereDist + 200),
                radius = ImageSize.X / 15.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 100, 255),
                    diffuse = new Vector3(100, 40, 78),
                    specular = new Vector3(50, 50, 50),
                    color = new Vector3(235, 2, 92),
                    reflectiveness = 50,
                    SpecExponent = 0,
                    Refraction = 0,
                    RefractionIndex = 0
                }
            });*/

            Lights = new List<Light>();
            Lights.Add(new Light() { position = new Vector3(ImageSize.X / 2.0f, ImageSize.Y -20, 0), color = new Vector3(255, 255, 255), radius = ImageSize.X / 20, intensity = 1.0f });
            //Lights.Add(new Light() { position = new Vector3(ImageSize.X / 2.0f, 20, 1000), color = new Vector3(255, 255, 255), radius = ImageSize.X / 20, intensity = 1.0f });
             /* Lights.Add(new Light() { 
                  position = new Vector3(ImageSize.X / 2.0f, ImageSize.Y - 20, 1000), 
                  color = new Vector3(255, 255, 255), 
                  radius = ImageSize.X / 20 });
           */
            DrawBox();
        }

        private void DrawBox()
        {


            //Left side 1
            Shapes.Add(new Triangle(
                new Vector3(0, 0, 0),
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new Vector3(0, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 255), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 255, 255), 0)));


            //Left side 2
            Shapes.Add(new Triangle(
                new Vector3(0, 0, 0),
                new Vector3(0, 0, SphereDist * 2),
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 255), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 255, 255), 0)));


            //Bottom side 1
            Shapes.Add(new Triangle(
                new Vector3(0, ImageSize.Y, 0),
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(0, 255, 0), new Vector3(0, 255, 0), new Vector3(0, 0, 0), new Vector3(0, 255, 0), 0)));


            //Bottom side 2
            Shapes.Add(new Triangle(
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(0, 255, 0), new Vector3(0, 255, 0), new Vector3(0, 0, 0), new Vector3(0, 255, 0), 0)));

            //Back side 1
            Shapes.Add(new Triangle(
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new Vector3(0, 0, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(255, 0, 0), new Vector3(255, 0, 0), new Vector3(0, 0, 0), new Vector3(255, 0, 0), 0)));

            //Back side 2
            Shapes.Add(new Triangle(
                new Vector3(0, 0, SphereDist * 2),
                new Vector3(ImageSize.X, 0, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(255, 0, 0), new Vector3(255, 0, 0), new Vector3(0, 0, 0), new Vector3(255, 0, 0), 0)));


            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(0, 0, 0),
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(0, 0, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 255), new Vector3(0, 0, 255), new Vector3(0, 0, 0), new Vector3(0, 0, 255), 0)));

            //top side 2
            Shapes.Add(new Triangle(
                new Vector3(0, 0, SphereDist * 2),
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, 0, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 255), new Vector3(0, 0, 255), new Vector3(0, 0, 0), new Vector3(0, 0, 255), 0)));

            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new Vector3(ImageSize.X, ImageSize.X, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(255, 0, 255), new Vector3(255, 0, 255), new Vector3(0, 0, 0), new Vector3(255, 0, 255), 0)));

            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, 0, SphereDist * 2),
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, ImageSize.X, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(255, 0, 255), new Vector3(255, 0, 255), new Vector3(0, 0, 0), new Vector3(255, 0, 255), 0)));



        }

    }
}
