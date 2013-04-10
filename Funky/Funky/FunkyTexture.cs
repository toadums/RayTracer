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
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using WinRTXamlToolkit.Imaging;

namespace Funky
{
    class FunkyTexture
    {
        public Vector3[,] TexturePixels;
        public Vector2 TexSize;

        public async Task LoadTexture(string filename)
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            var texFile = await folder.GetFileAsync(filename);
            var properties = await texFile.Properties.GetImagePropertiesAsync();

            WriteableBitmap wbmp = new WriteableBitmap((Int32)properties.Width, (Int32)properties.Height);

            await wbmp.LoadAsync(texFile);

            byte[] pixels = new byte[wbmp.PixelWidth * wbmp.PixelHeight * 4];

            using (Stream pixelStream = wbmp.PixelBuffer.AsStream())
            {
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
            }
            wbmp.Invalidate();
            TexturePixels = new Vector3[wbmp.PixelHeight, wbmp.PixelWidth];
            int r = 0, c = 0;
            for (int i = 0; i < pixels.Length; i++)
            {


                TexturePixels[r, c] = new Vector3(pixels[i + 2], pixels[i + 1], pixels[i + 0]) / 255.0f;


                c = (c + 1) % wbmp.PixelWidth;

                if (c == 0)
                {
                    r = (r + 1) % wbmp.PixelHeight;

                }

                i += 3;

            }

            TexSize = new Vector2(wbmp.PixelWidth, wbmp.PixelHeight);

        }
    }
}

