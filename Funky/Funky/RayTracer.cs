
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using SharpDX;
using System.Collections.Generic;

namespace Funky
{
    class Utility
    {
        #region Basic Frame Counter

        private static int lastTick;
        private static int lastFrameRate;
        private static int frameRate;

        public static int CalculateFrameRate()
        {
            if (System.Environment.TickCount - lastTick >= 1000)
            {
                lastFrameRate = frameRate;
                frameRate = 0;
                lastTick = System.Environment.TickCount;
            }
            frameRate++;
            return lastFrameRate;
        }
        #endregion

    }


    class RayTracer
    {

        public WriteableBitmap WB;
        private TextBlock FPS;

        public Vector3 Eye;
        
        private List<GeometricObject> Shapes;

        public RayTracer(ref WriteableBitmap wb, ref TextBlock fps)
        {
            WB = wb;
            FPS = fps;

            Eye = new Vector3(MainPage.ImageSize / 2.0f, -10000);

            Shapes = new List<GeometricObject>();

            Shapes.Add(new Sphere(50,new Vector3(150,200,200), new Vector4(255,0,0,255), 
                new SurfaceType(new Vector3(200,100,100), new Vector3(100,100,100), new Vector3(50,50,50),50)));
            /*
            Shapes.Add(new Sphere(75, new Vector3(250, 200, 100), new Vector4(255, 255, 0, 255),
                new SurfaceType(new Vector3(), new Vector3(), new Vector3(),50)));
            */
            Shapes.Add(new Sphere(15, new Vector3(0, 200, 200), new Vector4(255, 255, 255, 255),
                new SurfaceType(new Vector3(), new Vector3(), new Vector3(), 50), true));

        }


        public async void Draw()
        {

            int pixelWidth = WB.PixelWidth;
            int pixelHeight = WB.PixelHeight;

            while (true)
            {
                // Asynchronously graph the Mandelbrot set on a background thread
                byte[] result = null;
                await ThreadPool.RunAsync(new WorkItemHandler(
                    (IAsyncAction action) =>
                    {
                        result = Trace(pixelWidth, pixelHeight);
                    }
                    ));

                // Open a stream to copy the graph to the WriteableBitmap's pixel buffer
                using (Stream stream = WB.PixelBuffer.AsStream())
                {
                    await stream.WriteAsync(result, 0, result.Length);
                }

                // Redraw the WriteableBitmap
                WB.Invalidate();
                FPS.Text = "FPS = " + Utility.CalculateFrameRate().ToString();

            }
        }


        private byte[] Trace(int width, int height)
        {
            // 4 bytes required for each pixel
            byte[] result = new byte[width * height * 4];
            int resultIndex = 0;

            Random rand = new Random();

            // Plot the Mandelbrot set on x-y plane
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector4 color = new Vector4(0, 0, 0, 0);
                    /*
                    Ray ray = new Ray(Eye, (new Vector3(x, y, 0)) - Eye);
                    //TODO: Possibly want to normalize ray.Direction?? (not sure yet)
                    



                    GeometricObject obj = RayColor(ray, ref color, 0);
                    System.Diagnostics.Debug.WriteLine(color);
                    //NOTICE the xyzw dont correspond to RGBA.
                    */
                    Ray ray = new Ray(Eye, (new Vector3(x, y, 0)) - Eye);
                    color = RayColor(ray, 0);
                                        

                    result[resultIndex++] = Convert.ToByte(color.Z); // Green value of pixel
                    result[resultIndex++] = Convert.ToByte(color.Y); // Blue value of pixel
                    result[resultIndex++] = Convert.ToByte(color.X); // Red value of pixel
                    result[resultIndex++] = Convert.ToByte(255); // Alpha value of pixel
                     
                }
            }

            return result;
        }

        private Vector4 RayColor(Ray ray, int depth)
        {
            Vector4 intersectionPoint = new Vector4(-1);
            GeometricObject closest = FindClosestShape(ray, ref intersectionPoint);

            if (intersectionPoint.W != 0)
            {
                //color = closest.color;
                return Shade(intersectionPoint, closest, ray, depth);
            }
            else
            {
                return new Vector4(0,0,0,255);
            }
        }

        private GeometricObject FindClosestShape(Ray ray, ref Vector4 intersectionPoint)
        {
            double closest = double.MaxValue;
            GeometricObject closestObject = null;

            foreach (GeometricObject s in Shapes)
            {
                if (s.IsLight) continue;
                double d = s.intersection(ray);
                if (d > 0)
                {
                    if (d < closest)
                    {
                        closestObject = s;
                        closest = d;
                    }
                }
            }

            if (closestObject == null)
            {
                intersectionPoint.W = 0;
                return null;
            }
            else if (closestObject.IsLight)
            {
                intersectionPoint.W = 0;
                return null;
            }
            else
            {
                intersectionPoint = FindPointOnRay(ray, closest);
                return closestObject;
            }
        }

        double distance(Vector4 p, GeometricObject light)
        {
            return Math.Sqrt(Math.Pow((p.X - light.position.X), 2) + Math.Pow((p.Y - light.position.Y), 2) + Math.Pow((p.Z - light.position.Z), 2));
        }


        GeometricObject traceToLight(Ray r, ref Vector4 p) {
		    
            GeometricObject closest = null;
            double bestT = double.MaxValue;

	        foreach(GeometricObject shape in Shapes){
	
		
		        double t = shape.intersection(r);
		        if (t > GeometricObject.INTERSECTION_TOLERANCE && t < bestT ) {

			        closest = shape;
			        bestT = t;
		        }
	        }

	        if (closest == null) {
		        // Nothing hit, set to infinite point
		        p.W = 0.0f;
		        return null;
	        } else if (closest.IsLight){
                p = FindPointOnRay(r, bestT);
		        p.W = 0.0f;
		        return closest;	
	        }else {
		        // Return closest point that was hit
		        p = FindPointOnRay(r, bestT);
		        return closest;
	        }
        }


        double visibility(Vector4 p, GeometricObject light, ref Ray visRay){
	
	        Vector4 newP;

            visRay = new Ray(new Vector3(p.X, p.Y, p.Z), light.position - new Vector3(p.X, p.Y, p.Z));
	
	        newP = p;
	
	        GeometricObject visTrace = traceToLight(visRay, ref newP);
	
	        if(visTrace == null)
		        return 0;
	        else
		        if(visTrace != light){ 
		
			        Vector3 v;
			
			        v = new Vector3(p.X, p.Y, p.Z) - new Vector3(newP.X, newP.Y, newP.Z);			

			        return v.Length();
					
		        }else 
			        return 0;
        }

        private Vector4 Shade(Vector4 p, GeometricObject shape, Ray r, int depth)
        {
            Vector3 p3D = new Vector3(p.X,p.Y,p.Z);
            Vector3 norm = shape.NormalAt(p3D, shape.position);
	        SurfaceType m = shape.surface;

            SurfaceType newSurface = new SurfaceType(new Vector3(0), new Vector3(150, 150f, 150), new Vector3(0), 0);
	        float shine = 50;
	        Vector3 PE = Eye -p3D;
	        foreach(GeometricObject temp in Shapes){
		
		        if( temp.IsLight ){
			
			        double dist = distance(p, temp);//TODO CHECK IF BLOCKED;
			        Ray visRay = new Ray(new Vector3(0), new Vector3(0));
			        
			        double visD = visibility(p, temp, ref visRay);

			        Vector3 v = p3D-shape.position;

			        double theta = Math.Acos(Vector3.Dot(v, norm)/(v.Length()*norm.Length()));//TODO MAYBE n SHOULD BE norm INSTEAD?!
			        double cosine = (Math.Cos(theta) < 0)?0:Math.Cos(theta);		

			        Vector3 refVector = v*(-1) + norm*2*Vector3.Dot(v, norm);
			        refVector.Normalize();

			        double preStuff;
			        if(visD > 0)
				        preStuff = (1/(1+Math.Pow(dist,2)))*(2/visD);
			        else 
				        preStuff = 0;
																						
			        newSurface.diffuse.X += (float)(preStuff*(shape.color.X*(temp.color.X)*cosine));	
			        newSurface.specular.X += (float)(preStuff*(temp.color.X)*m.specular.X*Math.Pow(Vector3.Dot(norm/norm.Length(), refVector), shine));
			
			        newSurface.diffuse.Y += (float)(preStuff*(shape.color.Y*(temp.color.Y)*cosine));
                    newSurface.specular.Y += (float)(preStuff * (temp.color.Y) * m.specular.Y * Math.Pow(Vector3.Dot(norm / norm.Length(), refVector), shine));
			
			        newSurface.diffuse.Z += (float)(preStuff*(shape.color.Z*(temp.color.Z)*cosine));
                    newSurface.specular.Z += (float)(preStuff * (temp.color.Z) * m.specular.Z * Math.Pow(Vector3.Dot(norm / norm.Length(), refVector), shine));
						
		        }
	        }

	        Ray refRay;
            Vector4 color = new Vector4(0, 0, 0, 0);
	        
	        PE = p3D - Eye;
            PE.Normalize();
            Vector3 endP = PE - norm / norm.Length() * Vector3.Dot(PE, norm / norm.Length()) * 2;

	        Vector3 q = endP - p3D;
	
            refRay = new Ray(p3D, endP);

	        
	        if(depth < 1){
		        Vector4 refC = RayColor(refRay, depth + 1);

                color = new Vector4(new Vector3(shape.color.X, shape.color.Y, shape.color.Z) * newSurface.ambient + (float)(1-m.reflectiveness/2.0f) * newSurface.diffuse + (float)m.reflectiveness*(new Vector3(refC.X, refC.Y, refC.Z) + newSurface.specular),1);

	        }else{

                color = new Vector4(new Vector3(shape.color.X, shape.color.Y, shape.color.Z) * newSurface.ambient + newSurface.diffuse + newSurface.specular,1);
	
	        }

            //System.Diagnostics.Debug.WriteLine(iterer++);
	
	
	        // Clamp color values to 1.0
            if (color.X > 255.0) color.X = 255.0f;
            if (color.Y > 255.0) color.Y = 255.0f;
            if (color.Z > 255.0) color.Z = 255.0f;

            if (color.X < 0) color.X = 0;
            if (color.Y < 0) color.Y = 0;
            if (color.Z < 0) color.Z = 0;

            if (float.IsNaN(color.X)) color.X = 0.0f;
            if (float.IsNaN(color.Y)) color.Y = 0.0f;
            if (float.IsNaN(color.Z)) color.Z = 0.0f;

            return color;

        }

        private Vector4 FindPointOnRay(Ray ray, double t) {
            
            Vector4 intersect;
            
            intersect.X = (float)(ray.Start.X + t * ray.Direction.X);
            intersect.Y = (float)(ray.Start.Y + t * ray.Direction.Y);
            intersect.Z = (float)(ray.Start.Z + t * ray.Direction.Z);
	        intersect.W = 1.0f;

            return intersect;

      }





    }
}
