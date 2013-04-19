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

        Vector3 white = new Vector3(.7f,.7f,.7f);
        Vector3 red = new Vector3(1,0,0);
        Vector3 blue = new Vector3(0, 171f/255f, 1);
        Vector3 yellow = new Vector3(1, 1, 0);

        public static void Sleep(int ms)
        {
            new System.Threading.ManualResetEvent(false).WaitOne(ms);

        }

        private async Task DrawGeometry(){
            
            Shapes = new List<GeometricObject>();
            
            Lights = new List<Light>();
          /*
            Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X * .25f + ImageSize.X / 2.0f, ImageSize.Y - (ImageSize.X / 8.0f), SphereDist+500),
                radius = ImageSize.X / 8.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 0.4f, 1),
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = yellow,
                    reflectiveness = 0,
                    SpecExponent = 1000,
                    RefractionIndex = 0
                }
            });
            */


            Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X / 3, ImageSize.Y / 2.0f, SphereDist),
                radius = ImageSize.X / 8.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 0.4f, 1),
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = new Vector3(.3f, .3f, .3f),
                    reflectiveness = 0,
                    SpecExponent = 500,
                    RefractionIndex = 1.0f
                }
            });

            for (float i = ImageSize.X / 50.0f; i < ImageSize.X - ImageSize.X / 50.0f; i += ImageSize.X / 5)
            {
                for (float j = ImageSize.X / 50.0f; j < ImageSize.X - ImageSize.X / 50.0f; j += ImageSize.X / 5)
                {

                    float r = new Random().NextFloat(0, 1);
                    Sleep(100);
                    float g = new Random().NextFloat(0, 1);
                    Sleep(100);
                    float b = new Random().NextFloat(0, 1);

                    Vector3 col = new Vector3(r, g, b);

                    Shapes.Add(new Sphere()
                    {
                        position = new Vector3(i, j, SphereDist * 2.0f),
                        radius = ImageSize.X / 20.0f,
                        surface = new SurfaceType()
                        {
                            type = textureType.standard,
                            ambient = new Vector3(.4f, .4f, .4f),
                            diffuse = new Vector3(.4f, .4f, .4f),
                            specular = new Vector3(1, 1, 1),
                            color = col,
                            reflectiveness = new Random().Next(0, 0),
                            SpecExponent = new Random().NextFloat(1000, 3000),
                            RefractionIndex = 0
                        }
                    });


                }
            }



            /*
            Shapes.Add(new Sphere()
            {
                position = new Vector3(-ImageSize.X * .25f + ImageSize.X / 2.0f, ImageSize.Y - (ImageSize.X / 6.0f), SphereDist + 700),
                radius = ImageSize.X/6f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 0.4f, 1),
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = white,
                    reflectiveness = 0,
                    SpecExponent = 100,
                    RefractionIndex = 0
                }
            });
            */
            /*Blob myBlob = new Blob(new Vector3(ImageSize.X / 1.6f + ImageSize.X/5, ImageSize.Y / 2.0f + ImageSize.Y/12, SphereDist + 500),
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

            Shapes.Add(myBlob);*/

            //CUBEEEEEEE
            /*Cube cube = new Cube(new Vector3(ImageSize.X * .3f, ImageSize.Y, 1500), ImageSize.Y / 2f, ImageSize.X * .1875f, 200)
                {
                    surface = new SurfaceType()
                    {
                        type = textureType.standard,
                        ambient = new Vector3(0, 0.4f, 1),
                        diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                        specular = new Vector3(0.2f, 0.2f, 0.2f),
                        color = yellow,
                        reflectiveness = 0,
                        SpecExponent = 70,
                        RefractionIndex = 0
                    }
                };

            cube.buildCube();

            Shapes.Add(cube);*/




            /*
            Lights.Add(new Light()
            {
                position = new Vector3(ImageSize.X / 2.0f, 40, SphereDist),
                //position = new Vector3(ImageSize.X/2 , ImageSize.Y/2, -2000),
                color = new Vector3(1),
                //color = new Vector3(1, 1, 1),
                radius = ImageSize.X / 10,
                intensity = 1.0f
            });
            */

            Lights.Add(new Light()
            {
                position = new Vector3(ImageSize.X-400, ImageSize.Y / 2.0f , 600),
                //position = new Vector3(ImageSize.X/2 , ImageSize.Y/2, -2000),
                color = new Vector3(1),
                //color = new Vector3(1, 1, 1),
                radius = ImageSize.X / 20,
                intensity = 1.0f
            });


           //DrawBox();
        }

        private void DrawBox()
        {
            /*
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
                   */     
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
            /*
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
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(1, 0, 1), new Vector3(0, 0, 0), blue, 0)));


            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, 0, SphereDist * 2),
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, ImageSize.X, SphereDist * 2),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(1, 0, 1), new Vector3(0, 0, 0), blue, 0)));
        
            */
              }

    }
}
