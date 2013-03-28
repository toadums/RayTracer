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
                    ambient = new Vector3(.4f, .4f, .4f),
                    diffuse = new Vector3(.4f, .4f, .4f),
                    specular = new Vector3(1, 1, 1),
                    color = new Vector3(.4f, .4f, .4f),
                    reflectiveness = 100,
                    SpecExponent = 100,
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
                    ambient = new Vector3(0, 0.4f, 1),
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = new Vector3(0.312321312312312f, 0.96f, 0.38f),
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
                    ambient = new Vector3(0, 0.4f, 1),
                    diffuse = new Vector3(0.12412f, 0.442f, 0.323f),
                    specular = new Vector3(0.5f, 0.5f, 0.5f),
                    color = new Vector3(0.891283f, 0.0412412f, 0.34f),
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
                    ambient = new Vector3(0, 100, 1),
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
            Lights.Add(new Light() { position = new Vector3(ImageSize.X/2.0f, ImageSize.Y /2.0f , 5), color = new Vector3(1, 1, 1), radius = ImageSize.X / 20, intensity = 1.0f });
            //Lights.Add(new Light() { position = new Vector3(ImageSize.X / 2.0f, 20, 1000), color = new Vector3(1, 1, 1), radius = ImageSize.X / 20, intensity = 1.0f });
             /* Lights.Add(new Light() { 
                  position = new Vector3(ImageSize.X / 2.0f, ImageSize.Y - 20, 1000), 
                  color = new Vector3(1, 1, 1), 
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
                new SurfaceType(textureType.standard, new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 1), 0)));


            //Left side 2
            Shapes.Add(new Triangle(
                new Vector3(0, 0, 0),
                new Vector3(0, 0, SphereDist * 2),
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 1), 0)));


            //Bottom side 1
            Shapes.Add(new Triangle(
                new Vector3(0, ImageSize.Y, 0),
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0), 0)));


            //Bottom side 2
            Shapes.Add(new Triangle(
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0), 0)));

            //Back side 1
            Shapes.Add(new Triangle(
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new Vector3(0, 0, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 0, 0), 0)));

            //Back side 2
            Shapes.Add(new Triangle(
                new Vector3(0, 0, SphereDist * 2),
                new Vector3(ImageSize.X, 0, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 0, 0), 0)));


            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(0, 0, 0),
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(0, 0, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 1), 0)));

            //top side 2
            Shapes.Add(new Triangle(
                new Vector3(0, 0, SphereDist * 2),
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, 0, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 1), 0)));

            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new Vector3(ImageSize.X, ImageSize.X, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 1), new Vector3(1, 0, 1), new Vector3(0, 0, 0), new Vector3(1, 0, 1), 0)));

            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, 0, SphereDist * 2),
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, ImageSize.X, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 1), new Vector3(1, 0, 1), new Vector3(0, 0, 0), new Vector3(1, 0, 1), 0)));



        }

    }
}
