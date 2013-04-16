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
            /*
            
            */
            
            /*objLoader loader = new objLoader();

            Task tas = loader.CreateTriangles(@"teapot.obj", ImageSize.X / 5.0f, new Vector3(ImageSize.X / 2.0f, ImageSize.Y/ 2.0f, SphereDist));
            await tas;
            Task addTask = Task.Factory.StartNew(new Action(() =>
            {
                foreach (Triangle t in loader.Triangles)
                {
                    Shapes.Add(t);
                }

            }));

            await addTask;
            */
            


            /*
            for (float i = ImageSize.X / 50.0f; i < ImageSize.X - ImageSize.X / 50.0f; i += ImageSize.X / 10)
            {
                for (float j = ImageSize.X / 50.0f; j < ImageSize.X - ImageSize.X / 50.0f; j += ImageSize.X / 10)
                {

                    float r = new Random().NextFloat(0, 1);
                    Sleep(ImageSize.X*.25f);
                    float g = new Random().NextFloat(0, 1);
                    Sleep(ImageSize.X*.25f);
                    float b = new Random().NextFloat(0, 1);

                    Vector3 col = new Vector3(r,g,b);

                    Shapes.Add(new Sphere()
                    {
                        position = new Vector3(i, j, SphereDist * 2.0f),
                        radius = ImageSize.X / 30,
                        surface = new SurfaceType()
                        {
                            type = textureType.standard,
                            ambient = new Vector3(.4f, .4f, .4f),
                            diffuse = new Vector3(.4f, .4f, .4f),
                            specular = new Vector3(1, 1, 1),
                            color = col,
                            reflectiveness = new Random().Next(0,ImageSize.X*.25f),
                            SpecExponent = new Random().NextFloat(1000,3000),
                            RefractionIndex = 0
                        }
                    });


                }
            }
            */

            
            
            /*Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X / 2.0f, ImageSize.Y / 2.0f - ImageSize.Y / 8.0f, SphereDist * 2.0f),
                radius = ImageSize.X / 15.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 0.4f, 1),
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = new Vector3(0.234f, 0.62f, 0.1ImageSize.X*.0624f6f),
                    reflectiveness = 50,
                    SpecExponent = 5,
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
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = new Vector3(0.734f,0.3f, 0.12344f),
                    reflectiveness = 50,
                    SpecExponent = 5,
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
                    RefractionIndex = 0
                }
            });
            Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X / 2.0f, ImageSize.Y / 2.0f, SphereDist),
                radius = ImageSize.X / 15.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 0.4f, 1),
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = new Vector3(0.414f, 0.62314f, 0.99999f),
                    reflectiveness = 50,
                    SpecExponent = 5,
                    RefractionIndex = 0
                }
            });*/


            Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X * .25f + ImageSize.X / 2.0f, ImageSize.Y - (ImageSize.X / 8.0f), SphereDist + 500),
                radius = ImageSize.X / 8.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 0.4f, 1),
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = white,
                    reflectiveness = 0,
                    SpecExponent = 1000,
                    RefractionIndex = 0
                }
            });

            Blob myBlob = new Blob(new Vector3(ImageSize.X / 2.0f + ImageSize.X/5, ImageSize.Y / 2.0f, SphereDist + 200),
                                new Vector3(ImageSize.X / 2.0f, ImageSize.Y / 3.0f, SphereDist + 200),
                                new Vector3(ImageSize.X / 2.0f - ImageSize.X/5, ImageSize.Y / 2.0f + ImageSize.Y/8, SphereDist + 200),
                                ImageSize.X/6)
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
            Cube cube = new Cube(new Vector3(ImageSize.X * .25f, ImageSize.Y, 1300), ImageSize.Y/2f, ImageSize.X * .1875f, 200);

            //Shapes.AddRange(cube.buildCube());
            cube.buildCube();

            Shapes.Add(cube);

            /*Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X / 2.0f + ImageSize.X / 3.0f, ImageSize.Y / 2.0f, SphereDist),
                radius = ImageSize.X / 15.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 0.4f, 1),
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = new Vector3(0,1f, .3f),
                    reflectiveness = 0,
                    SpecExponent = 1000,
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
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = new Vector3(1f, 0, .33f),
                    reflectiveness = 0,
                    SpecExponent = 1000,
                    RefractionIndex = 0
                }
            });*/

            /*Lights.Add(new Light()
            {
                position = new Vector3(278,545.7f,279.5f),
                color = new Vector3(1, .85f, .43f),
                radius = ImageSize.X / 20,
                intensity = 1.0f
            });*/

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

            

            

            /*//Cornell Box
            Shapes.Add(new Triangle(
                new Vector3(556,0,0),
                new Vector3(0,0,0),
                new Vector3(0,0,0),
                new SurfaceType(textureType.standard, 
                    new Vector3(1, 0, 1), 
                    new Vector3(1, 0, 1), 
                    new Vector3(0, 0, 0), 
                    white, 0)));

            Shapes.Add(new Triangle(
                new Vector3(556, 0, 0),
                new Vector3(0, 0, 559.2f),
                new Vector3(556, 0, 559.2f),
                new SurfaceType(textureType.standard,
                    new Vector3(1, 0, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 0, 0),
                    white, 0)));

            Shapes.Add(new Triangle(
                new Vector3(556, 548.8f, 0),
                new Vector3(556, 548.8f, 559.2f),
                new Vector3(0, 548.8f, 559.2f),
                new SurfaceType(textureType.standard,
                    new Vector3(1, 0, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 0, 0),
                    white, 0)));

            Shapes.Add(new Triangle(
                new Vector3(556, 548.8f, 0),
                new Vector3(0, 548.8f, 559.2f),
                new Vector3(0, 548.8f, 0),
                new SurfaceType(textureType.standard,
                    new Vector3(1, 0, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 0, 0),
                    white, 0)));

            Shapes.Add(new Triangle(
                new Vector3(556, 0, 559.2f),
                new Vector3(0, 0, 559.2f),
                new Vector3(0, 548.8f, 559.2f),
                new SurfaceType(textureType.standard,
                    new Vector3(1, 0, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 0, 0),
                    white, 0)));

            Shapes.Add(new Triangle(
                new Vector3(556, 0, 559.2f),
                new Vector3(0, 548.8f, 559.2f),
                new Vector3(556, 548.8f, 559.2f),
                new SurfaceType(textureType.standard,
                    new Vector3(1, 0, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 0, 0),
                    white, 0)));

            Shapes.Add(new Triangle(
                new Vector3(556, 0, 559.2f),
                new Vector3(0, 0, 559.2f),
                new Vector3(0, 548.8f, 559.2f),
                new SurfaceType(textureType.standard,
                    new Vector3(1, 0, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 0, 0),
                    white, 0)));

            Shapes.Add(new Triangle(
                new Vector3(556, 0, 0),
                new Vector3(556, 0, 559.2f),
                new Vector3(556, 548.8f, 559.2f),
                new SurfaceType(textureType.standard,
                    new Vector3(1, 0, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 0, 0),
                    red, 0)));

            Shapes.Add(new Triangle(
                new Vector3(556, 0, 0),
                new Vector3(556, 548.8f, 559.2f),
                new Vector3(556, 548.8f, 0),
                new SurfaceType(textureType.standard,
                    new Vector3(1, 0, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 0, 0),
                    red, 0)));

            Shapes.Add(new Triangle(
                new Vector3(0, 0, 559.2f),
                new Vector3(0, 0, 0),
                new Vector3(0, 548.8f, 0),
                new SurfaceType(textureType.standard,
                    new Vector3(1, 0, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 0, 0),
                    green, 0)));

            Shapes.Add(new Triangle(
                new Vector3(0, 0, 559.2f),
                new Vector3(0, 548.8f, 0),
                new Vector3(0, 548.8f, 559.2f),
                new SurfaceType(textureType.standard,
                    new Vector3(1, 0, 1),
                    new Vector3(1, 0, 1),
                    new Vector3(0, 0, 0),
                    green, 0)));

            */
        }

    }
}
