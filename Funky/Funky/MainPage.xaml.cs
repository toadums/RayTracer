
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Funky
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {

        WriteableBitmap WB;
        RayTracer Tracer;


        public static Vector2 WindowSize;

        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += (s, fe) =>
            {
                WindowSize = new Vector2((float)this.ActualWidth, (float)this.ActualHeight);

                WB = new WriteableBitmap((int)RayTracer.ImageSize.X, (int)RayTracer.ImageSize.Y);

                TheShit.Source = WB;


                Tracer = new RayTracer(ref WB, ref FPS, (int)RayTracer.ImageSize.X, (int)RayTracer.ImageSize.Y);

                Tracer.Draw();

                

                   

            };
        }

    }
}
