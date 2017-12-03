using Android.Graphics;
using Android.Views;
using SkiaSharp;
using Skixam.Forms;

namespace Sample.Droid
{
    public class NativeSkiaView : View
    {
        Bitmap _bitmap;
        SkiaView _skiaView;

        public NativeSkiaView(Android.Content.Context context, SkiaView skiaView) : base(context)
        {
            _skiaView = skiaView;
        }
        public override bool OnTouchEvent(MotionEvent e)
        {
            return base.OnTouchEvent(e);
        }
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (_bitmap == null || _bitmap.Width != canvas.Width || _bitmap.Height != canvas.Height)
            {
                if (_bitmap != null)
                    _bitmap.Dispose();

                _bitmap = Bitmap.CreateBitmap(canvas.Width, canvas.Height, Bitmap.Config.Argb8888);
            }

            try
            {
                var surface = SKSurface.Create(canvas.Width, canvas.Height, SKColorType.Rgba8888, SKAlphaType.Premul, _bitmap.LockPixels(), canvas.Width * 4);
                
                var skcanvas = surface.Canvas;
                (_skiaView as ISkiaViewController).SendDraw(skcanvas, canvas.Width, canvas.Height);

            }
            finally
            {
                _bitmap.UnlockPixels();
            }

            canvas.DrawBitmap(_bitmap, 0, 0, null);
        }
    }
}