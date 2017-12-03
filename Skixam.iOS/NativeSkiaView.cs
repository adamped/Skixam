using System;
using CoreGraphics;
using UIKit;
using SkiaSharp;
using Skixam.Forms;

namespace Skixam.iOS
{
    public class NativeSkiaView : UIView
    {
        const int bitmapInfo = ((int)CGBitmapFlags.ByteOrder32Big) | ((int)CGImageAlphaInfo.PremultipliedLast);

        ISkiaViewController skiaView;

        public NativeSkiaView(SkiaView skiaView) => this.skiaView = skiaView;

        public override void Draw(CGRect rect)
        {

            base.Draw(rect);

            var screenScale = UIScreen.MainScreen.Scale;

            var width = (int)(Bounds.Width * screenScale);

            var height = (int)(Bounds.Height * screenScale);

            IntPtr buff = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(width * height * 4);

            try
            {

                var surface = SKSurface.Create(width, height, SKColorType.Rgba8888, SKAlphaType.Premul, buff, width * 4);

                var skcanvas = surface.Canvas;

                new SKAutoCanvasRestore(skcanvas, true);
                skiaView.SendDraw(skcanvas, width, height);

                using (var colorSpace = CGColorSpace.CreateDeviceRGB())
                using (var bContext = new CGBitmapContext(buff, width, height, 8, width * 4, colorSpace, (CGImageAlphaInfo)bitmapInfo))
                using (var image = bContext.ToImage())
                using (var context = UIGraphics.GetCurrentContext())
                {
                    context.TranslateCTM(0, Frame.Height);

                    context.ScaleCTM(1, -1);

                    context.DrawImage(Bounds, image);
                }
            }
            finally
            {
                if (buff != IntPtr.Zero)
                    System.Runtime.InteropServices.Marshal.FreeCoTaskMem(buff);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            SetNeedsDisplay();
        }

    }
}
