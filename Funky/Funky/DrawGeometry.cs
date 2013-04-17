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

        Vector3 white = new Vector3(.76f, .75f, .5f);
        Vector3 red = new Vector3(.63f, .06f, .04f);
        Vector3 green = new Vector3(.15f, .48f, .09f);

        public static void Sleep(int ms)
        {
            new System.Threading.ManualResetEvent(false).WaitOne(ms);

        }

        private async Task DrawGeometry(){
            
            Shapes = new List<GeometricObject>();
            
            Lights = new List<Light>();
          
            Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X * .05f + ImageSize.X / 2.0f, ImageSize.Y - (ImageSize.X / 8.0f), SphereDist),
                radius = ImageSize.X / 8.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 0.4f, 1),
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = white,
                    reflectiveness = 0,
                    SpecExponent = 50,
                    RefractionIndex = 0
                }
            });

            Blob myBlob = new Blob(new Vector3(ImageSize.X / 1.6f + ImageSize.X/5, ImageSize.Y / 2.0f + ImageSize.Y/12, SphereDist + 500),
                                new Vector3(ImageSize.X / 1.6f + ImageSize.X/14, ImageSize.Y / 3.0f, SphereDist + 500),
                                new Vector3(ImageSize.X / 1.6f - ImageSize.X / 5, ImageSize.Y / 4.0f - ImageSize.Y / 12, SphereDist + 500),
                                ImageSize.X/7)
                                {
                                    surface = new SurfaceType()
                                    {
                                        type = textureType.standard,
                                        ambient = new Vector3(0, 0.4f, 1),
                                        diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                                        specular = new Vector3(0.2f, 0.2f, 0.2f),
                                        color = red,
                                        reflectiveness = 0,
                                        SpecExponent = 70,
                                        RefractionIndex = 0
                                    }
                                };

            myBlob.initBlobZones();

            Shapes.Add(myBlob);

            //CUBEEEEEEE
            Cube cube = new Cube(new Vector3(ImageSize.X * .3f, ImageSize.Y, 1500), ImageSize.Y / 2f, ImageSize.X * .1875f, 200)
                {
                    surface = new SurfaceType()
                    {
                        type = textureType.standard,
                        ambient = new Vector3(0, 0.4f, 1),
                        diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                        specular = new Vector3(0.2f, 0.2f, 0.2f),
                        color = green,
                        reflectiveness = 0,
                        SpecExponent = 70,
                        RefractionIndex = 0
                    }
                };

            cube.buildCube();

            Shapes.Add(cube);

            Lights.Add(new Light()
            {
                position = new Vector3(ImageSize.X / 2.0f, 5, SphereDist),
                //position = new Vector3(ImageSize.X/2 , ImageSize.Y/2, -2000),
                color = new Vector3(1, .85f, .43f),
                //color = new Vector3(1, 1, 1),
                radius = ImageSize.X / 20,
                intensity = 1.0f
            });

           DrawBox();
        }

        private void DrawBox()
        {
            //Left side 1
            Shapes.Add(new Triangle(
                new Vector3(0, -1, 0),
                new Vector3(0, ImageSize.Y, (SphereDist * 2) + 1),
                new Vector3(0, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(1,0,0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), red, 0)));
            
            //Left side 2
            Shapes.Add(new Triangle(
                new Vector3(0, 0, 0),
                new Vector3(0, 0, SphereDist * 2),
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(1,0,0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), red, 0)));
                        
            //Bottom side 1
            Shapes.Add(new Triangle(
                new Vector3(0, ImageSize.Y, 0),
                new Vector3(0, ImageSize.Y, 1+(SphereDist * 2)),
                new Vector3(ImageSize.X + 1, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(1,0,0), new Vector3(0, 1, 0), new Vector3(0, 0, 0), white, 0)));

            //Bottom side 2
            Shapes.Add(new Triangle(
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(1,0,0), new Vector3(0, 1, 0), new Vector3(0, 0, 0), white, 0)));


            //Back side 1
            Shapes.Add(new Triangle(
                new Vector3(0, ImageSize.Y, SphereDist * 2),
                new Vector3(0, 0, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 0), white, 0)));


            //Back side 2
            Shapes.Add(new Triangle(
                new Vector3(-1, 0, SphereDist * 2),
                new Vector3(ImageSize.X, 0, SphereDist * 2),
                new Vector3(ImageSize.X, ImageSize.Y+1, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 0), white, 0)));
            
            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(0, 0, 0),
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(0, 0, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(1,0,0), new Vector3(0, 0, 1), new Vector3(0, 0, 0), white, 0)));


            //top side 2
            Shapes.Add(new Triangle(
                new Vector3(-1, 0, SphereDist * 2),
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, 0, (SphereDist * 2)+1),
                new SurfaceType(textureType.standard, new Vector3(1,0,0), new Vector3(0, 0, 1), new Vector3(0, 0, 0), white, 0)));


            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, -1, 0),
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new Vector3(ImageSize.X, ImageSize.X, (SphereDist * 2) + 1),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(1, 0, 1), new Vector3(0, 0, 0), green, 0)));


            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, 0, SphereDist * 2),
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, ImageSize.X, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(1, 0, 1), new Vector3(0, 0, 0), green, 0)));
        }

    }
}
