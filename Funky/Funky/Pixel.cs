using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{
    
    class Pixel 
    {
        public byte R, G, B, A;

        public Pixel(Byte r, Byte g, Byte b, Byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }

    class Pixels
    {
        private byte[] m_Pixels;

        public Pixels(int size)
        {
            m_Pixels = new byte[size];
        }

        public Pixel this[int i, int j]
        {
            get { return new Pixel(m_Pixels[i * j], m_Pixels[i * j], m_Pixels[i * j], m_Pixels[i * j]); }
            set { m_Pixels[i * j] = value.R; m_Pixels[i * j] = value.G; m_Pixels[i * j] = value.B; m_Pixels[i * j] = value.A; }
        }

    }

}
