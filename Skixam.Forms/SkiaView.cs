using SkiaSharp;
using System;

namespace Skixam.Forms
{
    public interface ISkiaViewController 
    {
        void SendDraw(SKCanvas canvas, int width, int height);
    }

    public class SkiaView : ISkiaViewController
    {
        Action<SKCanvas, int, int> onDrawCallback;

        public SkiaView(Action<SKCanvas, int, int> onDrawCallback)
        {
            this.onDrawCallback = onDrawCallback;
        }

        void ISkiaViewController.SendDraw(SKCanvas canvas, int width, int height)
        {
            Draw(canvas, width, height);
        }

        protected virtual void Draw(SKCanvas canvas, int width, int height)
        {
            onDrawCallback(canvas, width, height);
        }
    }
}