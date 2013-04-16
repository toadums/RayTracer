using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{

    public class ZoneTab
    {
        public float fCoef, fDeltaFInvSquare, fGamma, fBeta;

        public ZoneTab(float a, float b, float c, float d)
        {
            fCoef = a;
            fDeltaFInvSquare = b;
            fGamma = c;
            fBeta = d;
        }
    }

    public class poly
    {
        public float a, b, c, fDistance, fDeltaFInvSquare;

        public poly(float A, float B, float C, float D, float E)
        {
            a = A;
            b = B;
            c = C;
            fDistance = D;
            fDeltaFInvSquare = E;
        }

        public int isLess(poly p)
        {
            if (fDistance < p.fDistance)
                return -1;
            else if (fDistance == p.fDistance)
                return 0;
            else
                return 1;
        }
    }

    public class Blob : GeometricObject
    {
        public List<Vector3> centerList;
        public float size;
        public float invSizeSquare;
        int zoneNumber = 10;
        List<ZoneTab> zoneTab;

        public Blob(Vector3 a, Vector3 b, Vector3 c, float Size)
        {
            zoneTab = new List<ZoneTab>() { new ZoneTab(10.0f, 0, 0, 0),
                                                          new ZoneTab(5.0f, 0, 0, 0),
                                                          new ZoneTab(3.33333f, 0, 0, 0),
                                                          new ZoneTab(2.5f, 0, 0, 0),
                                                          new ZoneTab(2.0f, 0, 0, 0),
                                                          new ZoneTab(1.66667f, 0, 0, 0),
                                                          new ZoneTab(1.42857f, 0, 0, 0),
                                                          new ZoneTab(1.25f, 0, 0, 0),
                                                          new ZoneTab(1.11111f, 0, 0, 0),
                                                          new ZoneTab(1.0f, 0, 0, 0)
                                                        };

            centerList = new List<Vector3>();
            centerList.Add(a);
            centerList.Add(b);
            centerList.Add(c);
            size = Size;
            invSizeSquare = 1.0f / (size * size);
        }

        public void initBlobZones()
        {
            float fLastGamma, fLastBeta, fLastInvRSquare;
            fLastGamma = fLastBeta = fLastInvRSquare = 0.0f;

            for(int i = 0;i < zoneTab.Count-1;i++)
            {
                float fInvRSquare = 1.0f / zoneTab[i].fCoef;
                zoneTab[i].fDeltaFInvSquare = fInvRSquare - fLastInvRSquare;

                float temp = (fLastInvRSquare - fInvRSquare) / (zoneTab[i].fCoef - zoneTab[i + 1].fCoef);
                zoneTab[i].fGamma = temp - fLastGamma;
                fLastGamma = temp;

                zoneTab[i].fBeta = fInvRSquare - fLastGamma * zoneTab[i + 1].fCoef - fLastBeta;
                fLastBeta = zoneTab[i].fBeta + fLastBeta;

                fLastInvRSquare = fInvRSquare;
            }

            zoneTab[zoneTab.Count - 1].fGamma = 0.0f;
            zoneTab[zoneTab.Count - 1].fBeta = 0.0f;
        }

        public override double intersection(Ray r)
        {
            float t = float.MaxValue;//(r.Direction.X * r.Direction.X + r.Direction.Y * r.Direction.Y + r.Direction.Z * r.Direction.Z);
            List<poly> polynomMap = new List<poly>();

            float rSquare, rInvSquare;
            rSquare = size * size;
            rInvSquare = invSizeSquare;
            float maxEstimatedPotential = 0.0f;

            float A = 0.0f;
            float B = 0.0f;
            float C = 0.0f;

            for (int i= 0; i< centerList.Count; i++)
            {
                Vector3 currentPoint = centerList[i];

                Vector3 vDist = currentPoint - r.Start;
                float aa = 1.0f;
                float bb = - 2.0f * (r.Direction.X*vDist.X + r.Direction.Y*vDist.Y + r.Direction.Z*vDist.Z);
                float cc = (vDist.X*vDist.X + vDist.Y*vDist.Y + vDist.Z*vDist.Z); 

                float BSquareOverFourMinusC = 0.25f * bb * bb - cc;
                float MinusBOverTwo = -0.5f * bb; 
                float ATimeInvSquare = aa * rInvSquare;
                float BTimeInvSquare = bb * rInvSquare;
                float CTimeInvSquare = cc * rInvSquare;

                for (int j=0; j < zoneNumber - 1; j++)
                {
                    float fDelta = BSquareOverFourMinusC + zoneTab[j].fCoef * rSquare;
                    if (fDelta < 0.0f) 
                    {
                        break;
                    }
                    float sqrtDelta = (float)Math.Sqrt(fDelta);
                    float t0 = MinusBOverTwo - sqrtDelta; 
                    float t1 = MinusBOverTwo + sqrtDelta;

                    poly poly0 = new poly(zoneTab[j].fGamma * ATimeInvSquare ,
                                  zoneTab[j].fGamma * BTimeInvSquare , 
                                  zoneTab[j].fGamma * CTimeInvSquare + zoneTab[j].fBeta,
                                  t0,
                                  zoneTab[j].fDeltaFInvSquare); 
                    poly poly1 = new poly(- poly0.a, - poly0.b, - poly0.c, 
                                  t1, 
                                  -poly0.fDeltaFInvSquare);
            
                    maxEstimatedPotential += zoneTab[j].fDeltaFInvSquare;

                    polynomMap.Add(poly0);
                    polynomMap.Add(poly1);
                };
            }

            if (polynomMap.Count < 2 || maxEstimatedPotential < 1.0f)
            {
                return 0;
            }
    
            polynomMap.Sort((x, y) => x.isLess(y));

            maxEstimatedPotential = 0.0f;
            bool bResult = false;
            int it, itNext;

            for(it = 0, itNext = it+1; itNext < polynomMap.Count; it = itNext, itNext++)
            {
                A += polynomMap[it].a;
                B += polynomMap[it].b;
                C += polynomMap[it].c;
                maxEstimatedPotential += polynomMap[it].fDeltaFInvSquare;
                if (maxEstimatedPotential < 1.0f)
                {
                    continue;
                }
                float fZoneStart =  polynomMap[it].fDistance;
                float fZoneEnd = polynomMap[itNext].fDistance;

                if (t > fZoneStart &&  0.01f < fZoneEnd )
                {
                    float fDelta = B * B - 4.0f * A * (C - 1.0f) ;
                    if (fDelta < 0.0f)
                    {
                        continue;
                    }
                    if(A == 0) A = float.MinValue;
                    float fInvA = (0.5f / A);
                    float fSqrtDelta = (float)Math.Sqrt(fDelta);

                    float t0 = fInvA * (- B - fSqrtDelta); 
                    float t1 = fInvA * (- B + fSqrtDelta);
                    if ((t0 > 0.01f ) && (t0 >= fZoneStart ) && (t0 < fZoneEnd) && (t0 <= t ))
                    {
                        t = t0;
                        bResult = true;
                    }
            
                    if ((t1 > 0.01f ) && (t1 >= fZoneStart ) && (t1 < fZoneEnd) && (t1 <= t ))
                    {
                        t = t1;
                        bResult = true;
                    }

                    if (bResult)
                    {
                        return t;
                    }
                }
            }
            return 0;
        }

        //public override n blobInterpolation(Vector3 pos, Blob b, Vector3 vOut)
        public override Vector3 NormalAt(Vector3 i, Vector3 from)
        {
            Vector3 gradient = new Vector3(0.0f,0.0f,0.0f);

            float fRSquare = size * size;
            for (int k= 0; k< centerList.Count; k++)
            {
                Vector3 normal = i - centerList[k];
                float fDistSquare = Vector3.Dot(normal, normal);
                if (fDistSquare <= 0.001f) 
                    continue;
                float fDistFour = fDistSquare * fDistSquare;
                normal = (fRSquare/fDistFour) * normal;
                gradient += normal;

                //gradient = gradient + normal;
            }
            //vOut = gradient;
       
            return gradient;


        }

    }
}
