﻿using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{
    partial class RayTracer
    {

        public static void Sleep(int ms)
        {
            new System.Threading.ManualResetEvent(false).WaitOne(ms);

        }

        public FunkyTexture BackgroundTexture;

        private async Task DrawGeometry(){
            
            Shapes = new List<GeometricObject>();
            
            Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X / 2.0f, ImageSize.Y / 2.0f, -SphereDist),
                radius = ImageSize.X / 4.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(.4f, .4f, .4f),
                    diffuse = new Vector3(.4f, .4f, .4f),
                    specular = new Vector3(1, 1, 1),
                    color = new Vector3(.4f, .4f, .4f),
                    reflectiveness = 0,
                    SpecExponent = 1000000,
                    RefractionIndex = 1.0f
                }
            });
         
            
            Lights = new List<Light>();
            Lights.Add(new Light()
            {
                position = new Vector3(ImageSize.X / 2.0f, ImageSize.Y / 2.0f , 0),
                color = new Vector3(1, 1, 1),
                radius = ImageSize.X / 20,
                intensity = 1.0f
            });
               /*
            objLoader loader = new objLoader();

            Task tas = loader.CreateTriangles(@"Capsule.obj", ImageSize.X / 3.0f, new Vector3(ImageSize.X / 2.0f, ImageSize.Y/ 2.0f, 0));
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
                    Sleep(100);
                    float g = new Random().NextFloat(0, 1);
                    Sleep(100);
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
                            reflectiveness = new Random().Next(0,100),
                            SpecExponent = new Random().NextFloat(1000,3000),
                            RefractionIndex = 0
                        }
                    });


                }
            }
            */

            /*
            
            Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X / 2.0f, ImageSize.Y / 2.0f - ImageSize.Y / 8.0f, SphereDist * 2.0f),
                radius = ImageSize.X / 15.0f,
                surface = new SurfaceType()
                {
                    type = textureType.standard,
                    ambient = new Vector3(0, 0.4f, 1),
                    diffuse = new Vector3(0.4f, 0.1f, 0.2f),
                    specular = new Vector3(0.2f, 0.2f, 0.2f),
                    color = new Vector3(0.234f, 0.62f, 0.1256f),
                    reflectiveness = 50,
                    SpecExponent = 5,
                    RefractionIndex = 0
                }
            });

            Shapes.Add(new Sphere()
            {
                position = new Vector3(ImageSize.X / 2.0f, ImageSize.Y / 2.0f + ImageSize.Y /4.0f, SphereDist * 2.0f),
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
                position = new Vector3(ImageSize.X / 2.0f + ImageSize.X / 5.0f, ImageSize.Y / 2.0f, SphereDist * 2.0f),
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
                position = new Vector3(ImageSize.X / 2.0f - ImageSize.X / 8.0f, ImageSize.Y / 2.0f, SphereDist * 2.0f),
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
                    color = new Vector3(0.312321312312312f, 0.36f, 0.38f),
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
                    diffuse = new Vector3(0.12412f, 0.442f, 0.323f),
                    specular = new Vector3(0.5f, 0.5f, 0.5f),
                    color = new Vector3(0.891283f, 0.0412412f, 0.34f),
                    reflectiveness = 50,
                    SpecExponent = 5,
                    RefractionIndex = 0
                }
            });
            */

            

            
            //Lights.Add(new Light() { position = new Vector3(ImageSize.X / 2.0f, 20, 1000), color = new Vector3(1, 1, 1), radius = ImageSize.X / 20, intensity = 1.0f });
             /* Lights.Add(new Light() { 
                  position = new Vector3(ImageSize.X / 2.0f, ImageSize.Y - 20, 1000), 
                  color = new Vector3(1, 1, 1), 
                  radius = ImageSize.X / 20 });
           */
            await DrawBox();
        }


        private async Task DrawBox()
        {


            
                BackgroundTexture = new FunkyTexture();
                await BackgroundTexture.LoadTexture("nature.jpg");



            //Left side 1
            Triangle t3 = new Triangle(
                new Vector3(0, ImageSize.Y, -SphereDist * 4),
                new Vector3(0, 0, 0),
                new Vector3(0, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 1), 0))
                {
                    TextureCoords = new Vector2[] { new Vector2(1.0f/3.0f, 1), new Vector2(0, 0), new Vector2(0,1), },
                        Tag = "t3"

                };

            t3.SetTexture(ref BackgroundTexture);
            t3.TCC = TextureCoordConst.X;


            //Left side 2
            Triangle t4 = new Triangle(
                new Vector3(0, ImageSize.Y, -SphereDist * 4),
                new Vector3(0, 0, -SphereDist * 4),
                new Vector3(0, 0, 0),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 1), 0))
            {
                TextureCoords = new Vector2[] { new Vector2(1.0f / 3.0f, 1), new Vector2(1.0f / 3.0f, 0), new Vector2(0, 0), },
                Tag = "t4"

            };

            t4.SetTexture(ref BackgroundTexture);
            t4.TCC = TextureCoordConst.X;


            //back1
            Triangle t1 = new Triangle(
                new Vector3(0, 0, -SphereDist * 4),
                new Vector3(0, ImageSize.Y, -SphereDist * 4),
                new Vector3(ImageSize.X, ImageSize.Y, -SphereDist * 4),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 0, 0), 0) { SpecExponent = 1111111110 })
            {
                TextureCoords = new Vector2[] { new Vector2(1.0f / 3.0f, 0), new Vector2(1.0f / 3.0f, 1), new Vector2(2.0f / 3.0f, 1) },
                Tag = "t1"
            };
            //back2
            t1.SetTexture(ref BackgroundTexture);
            t1.TCC = TextureCoordConst.Z;

            Triangle t2 = new Triangle(

                new Vector3(0, 0, -SphereDist * 4),
                new Vector3(ImageSize.X, ImageSize.Y, -SphereDist * 4),
                new Vector3(ImageSize.X, 0, -SphereDist * 4),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 0, 0), 0) { SpecExponent = 111111110 })
            {
                TextureCoords = new Vector2[] { new Vector2(1.0f / 3.0f, 0), new Vector2(2.0f / 3.0f, 1), new Vector2(2.0f / 3.0f, 0), },
                Tag = "t2"

            };

            t2.SetTexture(ref BackgroundTexture);
            t2.TCC = TextureCoordConst.Z;

            //Right side 1
            Triangle t5 = new Triangle(
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, ImageSize.Y, -SphereDist * 4),
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 1), 0))
            {
                TextureCoords = new Vector2[] { new Vector2(1, 0), new Vector2(2.0f / 3.0f, 1), new Vector2(1, 1), },
                Tag = "t5"

            };

            t5.SetTexture(ref BackgroundTexture);
            t5.TCC = TextureCoordConst.X;


            //Right side 2
            Triangle t6 = new Triangle(
                new Vector3(ImageSize.X, 0, -SphereDist * 4),
                new Vector3(ImageSize.X, ImageSize.Y, -SphereDist * 4),
                new Vector3(ImageSize.X, 0, 0),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 1), 0))
            {
                TextureCoords = new Vector2[] { new Vector2(2.0f / 3.0f, 0), new Vector2(2.0f / 3.0f, 1), new Vector2(1, 0), },
                Tag = "t6"

            };

            t6.SetTexture(ref BackgroundTexture);
            t6.TCC = TextureCoordConst.X;




            //Back side 1
            Shapes.Add(t1);

            //Back side 2
            Shapes.Add(t2);

            //Left side 1
            Shapes.Add(t3);

            //Left side 2
            Shapes.Add(t4);

            //Right side 1
            Shapes.Add(t5);

            //Right side 2
            Shapes.Add(t6);

            /*
            

            //Bottom side 1
            Shapes.Add(new Triangle(
                new Vector3(0, ImageSize.Y, -SphereDist * 4),
                new Vector3(0, ImageSize.Y, 0),
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0), 0)));


            //Bottom side 2
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, ImageSize.Y, -SphereDist * 4),
                new Vector3(0, ImageSize.Y, -SphereDist * 4),
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new SurfaceType(textureType.standard, new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0), 0)));
            */


            /*
            
            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, -SphereDist * 4),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 1), 0)));

            //top side 2
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(0, 0, -SphereDist * 4),
                new Vector3(ImageSize.X, 0, -SphereDist * 4),
                new SurfaceType(textureType.standard, new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 1), 0)));

            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, ImageSize.Y, 0),
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, ImageSize.X, -SphereDist * 4),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 1), new Vector3(1, 0, 1), new Vector3(0, 0, 0), new Vector3(1, 0, 1), 0)));

            //Top side 1
            Shapes.Add(new Triangle(
                new Vector3(ImageSize.X, 0, 0),
                new Vector3(ImageSize.X, 0, -SphereDist * 4),
                new Vector3(ImageSize.X, ImageSize.X, -SphereDist * 4),
                new SurfaceType(textureType.standard, new Vector3(1, 0, 1), new Vector3(1, 0, 1), new Vector3(0, 0, 0), new Vector3(1, 0, 1), 0)));
            
            */

        }

    }
}
