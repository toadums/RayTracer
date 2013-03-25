using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{
    partial class RayTracer
    {
        public List<VirtualLight> VirtualLights;

        int NumVirtualLights = 50;

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
                Vector3 newLightPos = calcLightRay(ray, light, Shapes);

                if (newLightPos.X != float.MaxValue && newLightPos.Y != float.MaxValue && newLightPos.Z != float.MaxValue)
                {
                    GeometricObject VPLSurface = calcVPLSurface(ray, light, Shapes);

                    GeometricObject testSphere = new Sphere(ImageSize.Y / 16.0f, newLightPos, new Vector4(0, 0, 0, 255), new SurfaceType(textureType.standard,new Vector3(200, 100, 100), new Vector3(100, 40, 78), new Vector3(50, 50, 50), new Vector3(255, 255, 255), 50));
                    Shapes.Add(testSphere);

                    /*VirtualLights.Add(new VirtualLight()
                    {
                        position = newLightPos,
                        color = light.color,
                        intensity = 1f,
                        VPLSurface = VPLSurface
                    });*/
                }
            }
        }

        private Vector3 calcLightRay(Ray ray, Light light, List<GeometricObject> Shapes)
        {
            GeometricObject hitShape = null;
            double closestShape = float.MaxValue;

            foreach (GeometricObject shape in Shapes)
            {
                double t = shape.intersection(ray);

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

        private GeometricObject calcVPLSurface(Ray ray, Light light, List<GeometricObject> Shapes)
        {
            GeometricObject hitShape = null;
            double closestShape = float.MaxValue;

            foreach (GeometricObject shape in Shapes)
            {
                double t = shape.intersection(ray);

                if (t > 0.0 && t < closestShape)
                {
                    hitShape = shape;
                    closestShape = t;
                }
            }
            if (hitShape != null)
            {
                return hitShape;
            }
            else
                return null;
        }
    }
}
