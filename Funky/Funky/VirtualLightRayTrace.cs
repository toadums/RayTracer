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
        int NumVirtualLights = 10;
        bool drawSpheres = false;

        public void spawnVPL(Light light,float width,float height)
        {
            Random r = new Random();

            List<Vector3> virtualLightPositions = new List<Vector3>();
            for (int i = 0; i < NumVirtualLights; i++)
            {
                virtualLightPositions.Add(new Vector3(r.Next(0, (int)width), r.Next(0, (int)height), 0));
            }

            foreach (Vector3 VPLPos in virtualLightPositions)
            {
                Vector3 dir = (new Vector3(VPLPos.X, VPLPos.Y, 0)) - Eye;
                dir.Normalize();
                Ray ray = new Ray(Eye, dir);
                Vector3 newLightPos = calcLightRay(ray);
                SurfaceType hitSurface;

                if (newLightPos.X == float.MaxValue)
                {
                    continue;
                }
                else
                {
                    hitSurface = findSurfaceType(ray);
                }

                Vector3 dir2 = light.position - newLightPos;
                dir2.Normalize();

                Ray ray2 = new Ray(newLightPos, dir2);

                if (isVisible(light, VPLPos, ray2) <= 0)
                {
                    continue;
                }

                if (newLightPos.X != float.MaxValue && newLightPos.Y != float.MaxValue && newLightPos.Z != float.MaxValue)
                {
                    if (drawSpheres)
                    {
                        GeometricObject testSphere = new Sphere(ImageSize.Y / 32.0f, newLightPos, new Vector4(0, 0, 0, 255), new SurfaceType(textureType.standard,new Vector3(200, 100, 100), new Vector3(100, 40, 78), new Vector3(50, 50, 50), new Vector3(255, 255, 255), 0));
                        Shapes.Add(testSphere);
                    }
                    else
                    {
                        VirtualLights.Add(new Light()
                        {
                            position = newLightPos,
                            color = light.color,
                            intensity = (float)(1.0f / (float)NumVirtualLights)
                        });
                    }
                }
            }
        }

        private SurfaceType findSurfaceType(Ray ray)
        {
            GeometricObject hitShape = null;
            double closestShape = float.MaxValue;

            foreach (GeometricObject shape in Shapes)
            {
                double t;
                if (shape is Cube)
                {
                    Tuple<double, Triangle> temp = shape.intersectionCube(ray);
                    t = temp.Item1;
                }
                else
                {
                    t = shape.intersection(ray);
                }

                if (t > 0.0 && t < closestShape)
                {
                    hitShape = shape;
                    closestShape = t;
                }
            }
            if (hitShape != null)
            {
                return hitShape.surface;
            }
            else
                return null;
        }

        private Vector3 calcLightRay(Ray ray)
        {
            GeometricObject hitShape = null;
            double closestShape = float.MaxValue;


            foreach (GeometricObject shape in Shapes)
            {
                double t;
                if (shape is Cube)
                {
                    Tuple<double, Triangle> temp = shape.intersectionCube(ray);
                    t = temp.Item1;
                }
                else
                {
                    t = shape.intersection(ray);
                }

                if (t > 0.0 && t < closestShape)
                {
                    hitShape = shape;
                    closestShape = t;
                }
            }
            if (hitShape != null)
            {
                return FindPointOnRay(ray, closestShape);
            }
            else
                return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        }


    }
}
