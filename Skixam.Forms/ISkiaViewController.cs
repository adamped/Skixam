using SkiaSharp;

namespace Skixam.Forms
{
    public interface ISkiaViewController
    {
        void SendDraw(SKCanvas canvas, int width, int height);
    }
}
